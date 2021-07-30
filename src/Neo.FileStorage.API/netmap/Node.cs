using System;
using System.Collections.Generic;
using System.Linq;
using Neo.FileStorage.API.Cryptography;
using Neo.IO.Json;

namespace Neo.FileStorage.API.Netmap
{
    public class Node : IEquatable<Node>, IComparable<Node>
    {
        public const string AttributeCapacity = "Capacity";
        public const string AttributePrice = "Price";
        public const string AttributeSubnet = "Subnet";
        public const string AttributeUNLOCODE = "UN-LOCODE";
        public const string AttributeCountryCode = "ConuntryCode";
        public const string AttributeCountry = "Country";
        public const string AttributeLocation = "Location";
        public const string AttributeSubDivCode = "SubDivCode";
        public const string AttributeSubDiv = "SubDiv";
        public const string AttributeContinent = "Continent";

        public ulong ID;
        public ulong Capacity;
        public ulong Price;
        public int Index;
        public NodeInfo Info;
        public Dictionary<string, string> Attributes = new();
        public double Weight;
        public ulong Distance;

        public ulong Hash => ID;
        public List<string> NetworkAddresses => Info.Addresses.ToList();
        public byte[] PublicKey => Info.PublicKey.ToByteArray();

        public Node(int index, NodeInfo ni)
        {
            Index = index;
            ID = ni.PublicKey.ToByteArray().Murmur64(0);
            Info = ni;
            foreach (var attr in ni.Attributes)
            {
                if (attr.Key == AttributeCapacity)
                    Capacity = ulong.Parse(attr.Value);
                else if (attr.Key == AttributePrice)
                    Price = ulong.Parse(attr.Value);
                Attributes.Add(attr.Key, attr.Value);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Node))
                return false;
            return Equals((Node)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(Node n)
        {
            return ID == n.ID;
        }

        public int CompareTo(Node n)
        {
            if (n == null)
                return 1;
            else
                return ID.CompareTo(n.ID);
        }

        public static Node FromJson(JObject json)
        {
            int index = int.Parse(json["index"].AsString());
            NodeInfo ni = NodeInfo.FromJson(json["nodeinfo"]);
            return new(index, ni);
        }

        public JObject ToJson()
        {
            JObject json = new();
            json["id"] = ID;
            json["capacity"] = Capacity;
            json["index"] = Index;
            json["price"] = Price;
            json["nodeinfo"] = Info.ToJson();
            return json;
        }
    }
}
