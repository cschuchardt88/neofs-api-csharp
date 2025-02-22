// Copyright (C) 2015-2025 The Neo Project.
//
// UT_NodeInfo.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Refs;
using static Neo.FileStorage.API.Netmap.NodeInfo.Types;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_NodeInfo
    {
        [TestMethod]
        public void TestSubnetsZeroFalse()
        {
            NodeInfo n = new();
            n.Attributes.Add(new Attribute { Key = NodeInfo.SubnetAttributeKey(SubnetID.Zero), Value = Attribute.AttrSubnetValExit });
            var subnets = n.Subnets;
            Assert.AreEqual(1, subnets.Count);
            Assert.AreEqual(SubnetID.Zero, subnets[0]);
        }

        [TestMethod]
        public void TestSubnetsZeroInexist()
        {
            NodeInfo n = new();
            var subnets = n.Subnets;
            Assert.AreEqual(1, subnets.Count);
            Assert.AreEqual(SubnetID.Zero, subnets[0]);
        }

        [TestMethod]
        public void TestSubnets()
        {
            NodeInfo n = new();
            for (int i = 1; i <= 5; i++)
                n.Attributes.Add(new Attribute { Key = NodeInfo.SubnetAttributeKey(new() { Value = (uint)i }), Value = i > 3 ? Attribute.AttrSubnetValExit : Attribute.AttrSubnetValEntry });
            var subnets = n.Subnets;
            Assert.AreEqual(4, subnets.Count);
        }
    }
}
