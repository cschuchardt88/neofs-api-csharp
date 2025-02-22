// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Container.cs file belongs to the neo project and is free
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
using Neo.FileStorage.API.Netmap;
using System;

namespace Neo.FileStorage.API.UnitTests.TestContainer
{
    [TestClass]
    public class UT_Container
    {
        [TestMethod]
        public void TestContainerSerialize()
        {
            var key = "Kwk6k2eC3L3QuPvD8aiaNyoSXgQ2YL1bwS5CP1oKoA9waeAze97s".LoadWif();
            var container = new Container.Container
            {
                Version = new Refs.Version
                {
                    Major = 1,
                    Minor = 2,
                },
                OwnerId = key.OwnerID(),
                Nonce = ByteString.CopyFrom("1234".HexToBytes()),
                BasicAcl = 0u,
                PlacementPolicy = new PlacementPolicy(1, null, null, null),
            };
            Assert.AreEqual("0a0408011002121b0a1935ce67af47d9157014c8db22dc18769be12b1b136a394ef1db1a02123432021001", container.ToByteArray().ToHexString());
        }

        [TestMethod]
        public void TestContainerDeserialize()
        {
            var data = Array.Empty<byte>();
            var c = Container.Container.Parser.ParseFrom(data);
            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void TestNativeNameZone()
        {
            Container.Container container = new();
            Assert.AreEqual("", container.NativeName);
            Assert.AreEqual("", container.NativeZone);
            container.NativeName = "c1";
            container.NativeZone = Container.Container.Types.Attribute.SysAttributeZoneDefault;
            Assert.AreEqual("c1", container.NativeName);
            Assert.AreEqual(Container.Container.Types.Attribute.SysAttributeZoneDefault, container.NativeZone);
        }
    }
}
