// Copyright (C) 2015-2025 The Neo Project.
//
// Client.Reputation.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Reputation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task AnnounceTrust(ulong epoch, List<Trust> trusts, CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            if (trusts is null) throw new ArgumentNullException(nameof(trusts));
            var req = new AnnounceLocalTrustRequest
            {
                Body = new AnnounceLocalTrustRequest.Types.Body
                {
                    Epoch = epoch,
                }
            };
            req.Body.Trusts.AddRange(trusts);
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);
            var resp = await ReputationClient.AnnounceLocalTrustAsync(req, cancellationToken: context);
            ProcessResponse(resp);
        }

        public async Task AnnounceTrust(AnnounceLocalTrustRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ReputationClient.AnnounceLocalTrustAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
        }

        public async Task AnnounceIntermediateTrust(ulong epoch, uint iter, PeerToPeerTrust trust, CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            if (trust is null) throw new ArgumentNullException(nameof(trust));
            var req = new AnnounceIntermediateResultRequest
            {
                Body = new AnnounceIntermediateResultRequest.Types.Body
                {
                    Epoch = epoch,
                    Iteration = iter,
                    Trust = trust,
                },
                MetaHeader = opts.GetRequestMetaHeader()
            };
            opts.Key.Sign(req);
            var resp = await ReputationClient.AnnounceIntermediateResultAsync(req, cancellationToken: context);
            if (!resp.Verify())
                throw new FormatException("invalid announce intermediate trust response");
            CheckStatus(resp);
        }

        public async Task AnnounceIntermediateTrust(AnnounceIntermediateResultRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ReputationClient.AnnounceIntermediateResultAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
        }
    }
}
