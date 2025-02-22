// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Client.Container.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Netmap;
using System;
using System.Linq;
using System.Threading;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestPutContainerOnlyOne()
        {
            using var client = new Client.Client(key, host);
            var replica1 = new Replica(1, "loc1");
            var replica2 = new Replica(1, "loc2");
            var selector1 = new Selector("loc1", "Location", Clause.Same, 1, "loc1");
            var selector2 = new Selector("loc2", "Location", Clause.Same, 1, "loc2");
            var filter1 = new Filter("loc1", "Location", "Shanghai", Netmap.Operation.Eq);
            var filter2 = new Filter("loc2", "Location", "Shanghai", Netmap.Operation.Ne);
            var policy = new PlacementPolicy(1, new Replica[] { replica1, replica2 }, new Selector[] { selector1, selector2 }, new Filter[] { filter1, filter2 });
            var container = new Container.Container
            {
                Version = Refs.Version.SDKVersion(),
                OwnerId = key.OwnerID(),
                Nonce = Guid.NewGuid().ToByteString(),
                BasicAcl = BasicAcl.PublicBasicRule,
                PlacementPolicy = policy,
            };
            container.Attributes.Add(new Container.Container.Types.Attribute
            {
                Key = "CreatedAt",
                Value = DateTime.UtcNow.ToString(),
            });
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var cid = client.PutContainer(container, context: source.Token).Result;
            Console.WriteLine(cid.String());
            Assert.AreEqual(container.CalCulateAndGetId, cid);
        }

        [TestMethod]
        public void TestPutContainerSelect()
        {
            using var client = new Client.Client(key, host);
            // var replica = new Replica(2, ""); //not in policy
            var replica = new Replica(2, ""); // in policy with others
            // var replica = new Replica(1, ""); // test only one node put container size
            var policy = new PlacementPolicy(1, new Replica[] { replica }, null, null);
            var container = new Container.Container
            {
                Version = Refs.Version.SDKVersion(),
                OwnerId = key.OwnerID(),
                Nonce = Guid.NewGuid().ToByteString(),
                BasicAcl = BasicAcl.PublicBasicRule,
                PlacementPolicy = policy,
            };
            container.Attributes.Add(new Container.Container.Types.Attribute
            {
                Key = "CreatedAt",
                Value = DateTime.UtcNow.ToString(),
            });
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var cid = client.PutContainer(container, context: source.Token).Result;
            Console.WriteLine(cid.String());
            Assert.AreEqual(container.CalCulateAndGetId, cid);
        }

        [TestMethod]
        public void TestGetContainer()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var container = client.GetContainer(cid, context: source.Token).Result;
            Assert.AreEqual(cid, container.Container.CalCulateAndGetId);
            Console.WriteLine(container.Container);
        }

        [TestMethod]
        public void TestDeleteContainer()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            client.DeleteContainer(cid, context: source.Token).Wait();
        }

        [TestMethod]
        public void TestListContainer()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var cids = client.ListContainers(key.OwnerID(), context: source.Token).Result;
            Console.WriteLine(string.Join(", ", cids.Select(p => p.String())));
            Assert.AreEqual(1, cids.Count);
            Assert.AreEqual(cid.String(), cids[0].String());
        }

        [TestMethod]
        public void TestGetExtendedACL()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var eacl = client.GetEAcl(cid, context: source.Token).Result;
            Console.WriteLine(eacl.Table.ToString());
        }

        [TestMethod]
        public void TestSetExtendedACL()
        {
            using var client = new Client.Client(key, host);
            var target = new EACLRecord.Types.Target
            {
                Role = Role.Others,
            };
            var record = new EACLRecord
            {
                Operation = API.Acl.Operation.Delete,
                Action = API.Acl.Action.Allow,
            };
            record.Targets.Add(target);
            var eacl = new EACLTable
            {
                Version = Refs.Version.SDKVersion(),
                ContainerId = cid,
            };
            eacl.Records.Add(record);
            using var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            client.SetEACL(eacl, context: source.Token).Wait();
        }
    }
}
