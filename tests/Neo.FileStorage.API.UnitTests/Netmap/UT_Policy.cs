// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Policy.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using System.Linq;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_Policy
    {
        [TestMethod]
        public void TestPlacementPolicyCBFWithEmptySelector()
        {
            var nodes = new Node[]{
                Helper.GenerateTestNode(0,("ID", "1"), ("Attr", "Same")),
                Helper.GenerateTestNode(1,("ID", "2"), ("Attr", "Same")),
                Helper.GenerateTestNode(2,("ID", "3"), ("Attr", "Same")),
                Helper.GenerateTestNode(3,("ID", "4"), ("Attr", "Same")),
            };
            var p1 = new PlacementPolicy(0, new Replica[] { new Replica(2, "") }, null, null);
            var p2 = new PlacementPolicy(3, new Replica[] { new Replica(2, "") }, null, null);
            var p3 = new PlacementPolicy(3, new Replica[] { new Replica(2, "X") }, new Selector[] { new Selector("X", "", Clause.Distinct, 2, "*") }, null);
            var p4 = new PlacementPolicy(3, new Replica[] { new Replica(2, "X") }, new Selector[] { new Selector("X", "Attr", Clause.Same, 2, "*") }, null);

            var nm = new NetMap(nodes.ToList());
            var v1 = nm.GetContainerNodes(p1, null);
            Assert.AreEqual(4, v1.Flatten().Count);
            var v2 = nm.GetContainerNodes(p2, null);
            Assert.AreEqual(4, v2.Flatten().Count);
            var v3 = nm.GetContainerNodes(p3, null);
            Assert.AreEqual(4, v3.Flatten().Count);
            var v4 = nm.GetContainerNodes(p4, null);
            Assert.AreEqual(4, v4.Flatten().Count);

            var pivot = "29ed239ce0247236bdf393ef9faa1676068ed1aec2e77adc37d9ac5519617b5e".HexToBytes();
            v1 = nm.GetContainerNodes(p1, pivot);
            var result = v1.Flatten();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Index);
            Assert.AreEqual(2, result[1].Index);
            Assert.AreEqual(1, result[2].Index);
            Assert.AreEqual(3, result[3].Index);

            v2 = nm.GetContainerNodes(p2, pivot);
            result = v2.Flatten();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Index);
            Assert.AreEqual(2, result[1].Index);
            Assert.AreEqual(1, result[2].Index);
            Assert.AreEqual(3, result[3].Index);

            v3 = nm.GetContainerNodes(p3, pivot);
            result = v3.Flatten();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Index);
            Assert.AreEqual(2, result[1].Index);
            Assert.AreEqual(1, result[2].Index);
            Assert.AreEqual(3, result[3].Index);

            v4 = nm.GetContainerNodes(p4, pivot);
            result = v4.Flatten();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, result[0].Index);
            Assert.AreEqual(1, result[1].Index);
            Assert.AreEqual(2, result[2].Index);
            Assert.AreEqual(3, result[3].Index);
        }
    }
}
