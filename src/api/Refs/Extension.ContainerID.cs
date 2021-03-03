using Google.Protobuf;
using NeoFS.API.v2.Cryptography;

namespace NeoFS.API.v2.Refs
{
    public partial class ContainerID
    {
        public static ContainerID FromSha256Bytes(byte[] hash)
        {
            if (hash.Length != Crypto.Sha256HashLength) throw new System.InvalidOperationException("ContainerID must be a hash256");
            return new ContainerID
            {
                Value = ByteString.CopyFrom(hash)
            };
        }

        public static ContainerID FromBase58String(string id)
        {
            return FromSha256Bytes(Base58.Decode(id));
        }

        public string ToBase58String()
        {
            return Base58.Encode(Value.ToByteArray());
        }
    }
}
