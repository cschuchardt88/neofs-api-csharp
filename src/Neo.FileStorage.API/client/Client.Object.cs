using Google.Protobuf;
using Grpc.Core;
using Neo.FileStorage.API.Client.ObjectParams;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<Object.Object> GetObject(GetObjectParams param, CallOptions options = null, CancellationToken context = default)
        {
            var object_address = param.Address;
            if (object_address is null) throw new ArgumentException(nameof(GetObjectParams) + " missing address");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRequest
            {
                Body = new GetRequest.Types.Body
                {
                    Raw = param.Raw,
                    Address = object_address,
                }
            };
            var meta = opts.GetRequestMetaHeader();
            AttachObjectSessionToken(opts, meta, object_address, ObjectSessionContext.Types.Verb.Get);
            req.MetaHeader = meta;
            opts.Key.SignRequest(req);

            using var call = ObjectClient.Get(req, cancellationToken: context);
            var obj = new Object.Object();
            var payload = Array.Empty<byte>();
            int offset = 0;
            while (await call.ResponseStream.MoveNext())
            {
                var resp = call.ResponseStream.Current;
                if (!resp.VerifyResponse())
                    throw new InvalidOperationException("invalid object get response");
                switch (resp.Body.ObjectPartCase)
                {
                    case GetResponse.Types.Body.ObjectPartOneofCase.Init:
                        {
                            obj.ObjectId = resp.Body.Init.ObjectId;
                            obj.Signature = resp.Body.Init.Signature;
                            obj.Header = resp.Body.Init.Header;
                            payload = new byte[obj.PayloadSize];
                            break;
                        }
                    case GetResponse.Types.Body.ObjectPartOneofCase.Chunk:
                        {
                            if (payload.Length == 0) throw new InvalidOperationException("missing init");
                            var chunk = resp.Body.Chunk;
                            if (obj.PayloadSize < (ulong)(offset + chunk.Length))
                                throw new InvalidOperationException("data exceeds PayloadSize");
                            resp.Body.Chunk.CopyTo(payload, offset);
                            offset += chunk.Length;
                            break;
                        }
                    case GetResponse.Types.Body.ObjectPartOneofCase.SplitInfo:
                        {
                            throw new SplitInfoException(resp.Body.SplitInfo);
                        }
                    default:
                        throw new FormatException("malformed object get reponse");
                }
            }
            if ((ulong)offset < obj.PayloadSize)
                throw new InvalidOperationException("data is less than PayloadSize");
            obj.Payload = ByteString.CopyFrom(payload);
            return obj;
        }

        public async Task<ObjectID> PutObject(PutObjectParams param, CallOptions options = null, CancellationToken context = default)
        {
            var obj = param.Object;
            if (obj is null) throw new ArgumentException($"No Object in {nameof(PutObjectParams)}");
            if (obj.Header is null) throw new ArgumentException($"No Header in {nameof(PutObjectParams)}");
            if (obj.ObjectId is null) throw new ArgumentException($"No ObjectID in {nameof(PutObjectParams)}");
            if (obj.Payload is null || obj.Payload.Length == 0) throw new ArgumentException($"No Payload in {nameof(PutObjectParams)}");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new PutRequest();
            var body = new PutRequest.Types.Body();
            req.Body = body;

            var address = new Address
            {
                ContainerId = obj.Header.ContainerId,
                ObjectId = obj.ObjectId,
            };
            var meta = opts.GetRequestMetaHeader();
            AttachObjectSessionToken(opts, meta, address, ObjectSessionContext.Types.Verb.Put);
            req.MetaHeader = meta;
            var init = new PutRequest.Types.Body.Types.Init
            {
                ObjectId = obj.ObjectId,
                Signature = obj.Signature,
                Header = obj.Header,
            };
            req.Body.Init = init;
            opts.Key.SignRequest(req);

            using var call = ObjectClient.Put(cancellationToken: context);
            await call.RequestStream.WriteAsync(req);

            int offset = 0;
            while (offset < obj.Payload.Length)
            {
                var end = offset + Object.Object.ChunkSize > obj.Payload.Length ? obj.Payload.Length : offset + Object.Object.ChunkSize;
                var chunk = ByteString.CopyFrom(obj.Payload.ToByteArray()[offset..end]);
                var chunk_body = new PutRequest.Types.Body
                {
                    Chunk = chunk,
                };
                req.Body = chunk_body;
                req.VerifyHeader = null;
                opts.Key.SignRequest(req);
                await call.RequestStream.WriteAsync(req);
                offset = end;
            }
            await call.RequestStream.CompleteAsync();
            var resp = await call.ResponseAsync;
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid object put response");
            return resp.Body.ObjectId;
        }

        public async Task<Address> DeleteObject(DeleteObjectParams param, CallOptions options = null, CancellationToken context = default)
        {
            var object_address = param.Address;
            if (object_address is null) throw new ArgumentException($"No Address in {nameof(DeleteObjectParams)}");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new DeleteRequest
            {
                Body = new DeleteRequest.Types.Body
                {
                    Address = object_address,
                }
            };
            var meta = opts.GetRequestMetaHeader();
            AttachObjectSessionToken(opts, meta, object_address, ObjectSessionContext.Types.Verb.Delete);
            req.MetaHeader = meta;
            opts.Key.SignRequest(req);

            var resp = await ObjectClient.DeleteAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid object delete response");
            return resp.Body.Tombstone;
        }

        public async Task<Object.Object> GetObjectHeader(ObjectHeaderParams param, CallOptions options = null, CancellationToken context = default)
        {
            var object_address = param.Address;
            if (object_address is null) throw new ArgumentException($"No Address in {nameof(ObjectHeaderParams)}");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var minimal = param.Short;
            var req = new HeadRequest
            {
                Body = new HeadRequest.Types.Body
                {
                    Address = object_address,
                    MainOnly = minimal,
                    Raw = param.Raw,
                }
            };
            var meta = opts.GetRequestMetaHeader();
            AttachObjectSessionToken(opts, meta, object_address, ObjectSessionContext.Types.Verb.Head);
            req.MetaHeader = meta;
            opts.Key.SignRequest(req);

            var resp = await ObjectClient.HeadAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid object get header response");
            var header = new Header();
            var sig = new Signature();
            switch (resp.Body.HeadCase)
            {
                case HeadResponse.Types.Body.HeadOneofCase.ShortHeader:
                    {
                        if (!minimal) throw new FormatException("expect full header received short");
                        var short_header = resp.Body.ShortHeader;
                        if (short_header is null)
                            throw new FormatException("malformed object header");
                        header.PayloadLength = short_header.PayloadLength;
                        header.Version = short_header.Version;
                        header.OwnerId = short_header.OwnerId;
                        header.ObjectType = short_header.ObjectType;
                        header.CreationEpoch = short_header.CreationEpoch;
                        header.PayloadHash = short_header.PayloadHash;
                        header.HomomorphicHash = short_header.HomomorphicHash;
                        break;
                    }
                case HeadResponse.Types.Body.HeadOneofCase.Header:
                    {
                        if (minimal) throw new FormatException("expect short header received full");
                        var full_header = resp.Body.Header;
                        if (full_header is null)
                            throw new FormatException("malformed object header");
                        header = full_header.Header;
                        sig = full_header.Signature;
                        if (!sig.VerifyMessagePart(object_address.ObjectId))
                        {
                            throw new InvalidOperationException(nameof(GetObjectHeader) + " invalid signature");
                        }
                        break;
                    }
                case HeadResponse.Types.Body.HeadOneofCase.SplitInfo:
                    {
                        throw new SplitInfoException(resp.Body.SplitInfo);
                    }
                default:
                    throw new FormatException("malformed object header response");
            }
            var obj = new Object.Object
            {
                ObjectId = object_address.ObjectId,
                Header = header,
                Signature = sig,
            };
            return obj;
        }

        public async Task<byte[]> GetObjectPayloadRangeData(RangeDataParams param, CallOptions options = null, CancellationToken context = default)
        {
            var object_address = param.Address;
            if (object_address is null) throw new ArgumentException($"No Address in {nameof(RangeDataParams)}");
            var range = param.Range;
            if (range is null) throw new ArgumentException($"No Range in {nameof(RangeDataParams)}");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRangeRequest
            {
                Body = new GetRangeRequest.Types.Body
                {
                    Address = object_address,
                    Range = range,
                    Raw = param.Raw,
                }
            };
            var meta = opts.GetRequestMetaHeader();
            AttachObjectSessionToken(opts, meta, object_address, ObjectSessionContext.Types.Verb.Range);
            opts.Key.SignRequest(req);

            var stream = ObjectClient.GetRange(req, cancellationToken: context).ResponseStream;
            var payload = new byte[range.Length];
            var offset = 0;
            while (await stream.MoveNext())
            {
                var resp = stream.Current;
                if (!resp.VerifyResponse())
                    throw new FormatException("invalid object range response");
                var chunk = resp.Body.Chunk;
                chunk.CopyTo(payload, offset);
                offset += chunk.Length;
            }
            return payload;
        }

        public async Task<List<byte[]>> GetObjectPayloadRangeHash(RangeChecksumParams param, CallOptions options = null, CancellationToken context = default)
        {
            var object_address = param.Address;
            if (object_address is null) throw new ArgumentException($"No Address in {nameof(RangeChecksumParams)}");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRangeHashRequest
            {
                Body = new GetRangeHashRequest.Types.Body
                {
                    Address = object_address,
                    Salt = param.Salt is null ? ByteString.Empty : ByteString.CopyFrom(param.Salt),
                    Type = param.Type,
                }
            };
            req.Body.Ranges.AddRange(param.Ranges);
            var meta = opts.GetRequestMetaHeader();
            AttachObjectSessionToken(opts, meta, object_address, ObjectSessionContext.Types.Verb.Rangehash);
            req.MetaHeader = meta;
            opts.Key.SignRequest(req);

            var resp = await ObjectClient.GetRangeHashAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new FormatException("invalid object range hash response");
            return resp.Body.HashList.Select(p => p.ToByteArray()).ToList();
        }

        public async Task<List<ObjectID>> SearchObject(SearchObjectParams param, CallOptions options = null, CancellationToken context = default)
        {
            if (param.ContainerID is null) throw new ArgumentException($"No ContainerID in {nameof(SearchObjectParams)}");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new SearchRequest
            {
                Body = new SearchRequest.Types.Body
                {
                    ContainerId = param.ContainerID,
                    Version = SearchObjectVersion,
                }
            };
            req.Body.Filters.AddRange(param.Filters.Filters);
            var meta = opts.GetRequestMetaHeader();
            AttachObjectSessionToken(opts, meta, new Address { ContainerId = param.ContainerID }, ObjectSessionContext.Types.Verb.Search);
            req.MetaHeader = meta;
            opts.Key.SignRequest(req);

            var stream = ObjectClient.Search(req, cancellationToken: context).ResponseStream;
            var result = new List<ObjectID>();
            while (await stream.MoveNext())
            {
                var resp = stream.Current;
                if (!resp.VerifyResponse())
                    throw new FormatException("invalid object search response");
                result = result.Concat(resp.Body.IdList).ToList();
            }
            return result;
        }

        private void AttachObjectSessionToken(CallOptions options, RequestMetaHeader meta, Address address, ObjectSessionContext.Types.Verb verb,
            ulong exp = 0, ulong nbf = 0, ulong iat = 0)
        {
            if (options.Session is null) return;
            if (options.Session.Signature != null)
            {
                meta.SessionToken = options.Session;
                return;
            }

            var token = new SessionToken
            {
                Body = options.Session.Body
            };

            var ctx = new ObjectSessionContext
            {
                Address = address,
                Verb = verb,
            };

            var lt = new SessionToken.Types.Body.Types.TokenLifetime
            {
                Iat = iat,
                Exp = exp,
                Nbf = nbf,
            };

            token.Body.Object = ctx;
            token.Body.Lifetime = lt;
            token.Signature = options.Key.SignMessagePart(token.Body);

            meta.SessionToken = token;
        }
    }
}
