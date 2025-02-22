// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Client.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Cryptography.Tz;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using System;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    [TestClass]
    public partial class UT_Client
    {
        // private readonly string host = "https://st1.storage.fs.neo.org:8082"; //mainnet
        private readonly string host = "http://st1.t5.fs.neo.org:8080"; //testnet
        // private readonly string host = "http://s01.neofs.devenv:8080";
        // private readonly string host = "http://localhost:8081";
        private readonly ContainerID cid = ContainerID.FromString("CGrjkpGeE7iVYS4x3SgmTRovJkB78T671M3zxMSPokGJ");//testnet t5
        private readonly ObjectID oid = ObjectID.FromString("C9tTwyJYgFPvEoNTMr6WQYvBmYMsGcvQM4ropu59mLZR");//testnet t5

        // private readonly ContainerID cid = ContainerID.FromString("B9q3Njur7g38A9M3AvyuJLdQDLV4eyLgQRg79Titzyag");//not in policy
        // private readonly ObjectID oid = ObjectID.FromString("CkQsya9r9cAgiHqk5GLXY34zdTWUKwQ7KmVSsMXfrKVe");//small
        // private readonly ObjectID oid = ObjectID.FromString("");//big
        // private readonly ObjectID oid = ObjectID.FromString("HVXgYePb1FKSidNWwdHD6tHNnFN6kfN2J6GUvc8Tg8ym");//split
        // private readonly ContainerID cid = ContainerID.FromString("CDiEce9WSF9FySHKUJtuzHbVLhFTzSXwiyj91HaZAg4A");//the only one in policy
        // private readonly ObjectID oid = ObjectID.FromString("3Y7k2oTXzAHSzaiMHnFJDBuaX6SxuByVPdRUsYcvYUbs");//small
        // private readonly ObjectID oid = ObjectID.FromString("");//big
        // private readonly ObjectID oid = ObjectID.FromString("JDYccUD7eSk3MNhdGWRK3Hw3vgfSjnBj3KuvopE87m6P");//split
        // private readonly ContainerID cid = ContainerID.FromString("GxDJqnCTJBYGqKRqo6AbNWwmmj8jNNmeEX88cbrB1Myu");//with others in policy 
        // private readonly ObjectID oid = ObjectID.FromString("EBYdX5bZtGa1PxjwbWrR2pcsgubhn7h3NhWeP4YGWAL6");//small
        // private readonly ContainerID cid = ContainerID.FromString("9meDqcox8VAyC6qBomYAevA8rM79idsBxbYMcr5qJb4R");// only one go node
        // private readonly ObjectID oid = ObjectID.FromString("6GXG98kCZoZLmU3CBeYoNfZcQ4QQr25cLc3YzQKeZbD4"); //small
        private readonly ECDsa key = "Kzj1LbTtmfbyJjn4cZhD6U4pdq74iHcmKmGRRBiLQoQzPBRWLEKz".LoadWif();//s1 storage NiXweMv91Vz512bQw7jFNHAGBg8upVS8Qo testnet
        // private readonly ECDsa key = "Kz3KJfsLRjwEGLbhZXGjiZ2jbDgziTj5w3sUVVT8JtLGHdCn5jXJ".LoadWif();//other NbVGqFbFADgKYiiJhUbj3tYKibwtraWRmT
        // private readonly ECDsa key2 = "KxyjQ8eUa4FHt3Gvioyt1Wz29cTUrE4eTqX3yFSk1YFCsPL8uNsY".LoadWif();//ir Nhfg3TbpwogLvDGVvAvqyThbsHgoSUKwtn
        private Address Address => new(cid, oid);

        public Object.Object RandomFullObject(int len = 1024)
        {
            var rand = new Random();
            var payload = new byte[len];
            rand.NextBytes(payload);
            var obj = new Object.Object
            {
                Header = new Header
                {
                    Version = Refs.Version.SDKVersion(),
                    OwnerId = key.OwnerID(),
                    ContainerId = cid,
                    ObjectType = ObjectType.Regular,
                    PayloadHash = new Checksum
                    {
                        Type = ChecksumType.Sha256,
                        Sum = ByteString.CopyFrom(payload.Sha256()),
                    },
                    HomomorphicHash = new Checksum
                    {
                        Type = ChecksumType.Tz,
                        Sum = ByteString.CopyFrom(new TzHash().ComputeHash(payload)),
                    },
                    PayloadLength = (ulong)payload.Length,
                },
                Payload = ByteString.CopyFrom(payload),
            };
            obj.Header.Attributes.Add(new Header.Types.Attribute
            {
                Key = "category",
                Value = "test"
            });
            obj.ObjectId = obj.CalculateID();
            obj.Signature = obj.CalculateIDSignature(key);
            return obj;
        }
    }
}
