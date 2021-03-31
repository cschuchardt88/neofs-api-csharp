using Google.Protobuf;
using Neo.Cryptography;
using Neo.IO.Json;
using static Neo.FileStorage.API.Cryptography.Helper;

namespace Neo.FileStorage.API.Refs
{
    public partial class ContainerID
    {
        public static ContainerID FromSha256Bytes(byte[] hash)
        {
            if (hash.Length != Sha256HashLength) throw new System.FormatException("ContainerID must be a hash256");
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

        public JObject ToJson()
        {
            var json = new JObject();
            json["value"] = ToBase58String();
            return json;
        }
    }
}
