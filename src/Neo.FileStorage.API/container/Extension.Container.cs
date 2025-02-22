// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.Container.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using System;

namespace Neo.FileStorage.API.Container
{
    public partial class Container
    {
        public const string AttributeName = "Name";
        public const string AttributeTimestamp = "Timestamp";

        private ContainerID _id;
        public ContainerID CalCulateAndGetId
        {
            get
            {
                if (_id is null)
                    _id = new ContainerID
                    {
                        Value = this.Sha256()
                    };
                return _id;
            }
        }

        public Guid NonceUUID
        {
            get
            {
                return new Guid(nonce_.ToByteArray());
            }
            set
            {
                nonce_ = ByteString.CopyFrom(value.ToByteArray());
            }
        }

        public static partial class Types
        {
            public sealed partial class Attribute
            {
                private const string SysAttributePrefix = "__NEOFS__";
                public const string SysAttributeName = SysAttributePrefix + "NAME";
                public const string SysAttributeZone = SysAttributePrefix + "ZONE";
                public const string SysAttributeZoneDefault = "container";
            }
        }

        public string NativeName
        {
            get
            {
                foreach (var attr in Attributes)
                    if (attr.Key == Types.Attribute.SysAttributeName)
                        return attr.Value;
                return "";
            }

            set
            {
                foreach (var attr in Attributes)
                    if (attr.Key == Types.Attribute.SysAttributeName)
                    {
                        attr.Value = value;
                        return;
                    }
                Attributes.Add(new Types.Attribute
                {
                    Key = Types.Attribute.SysAttributeName,
                    Value = value,
                });
            }
        }

        public string NativeZone
        {
            get
            {
                foreach (var attr in Attributes)
                    if (attr.Key == Types.Attribute.SysAttributeZone)
                        return attr.Value;
                return "";
            }

            set
            {
                foreach (var attr in Attributes)
                    if (attr.Key == Types.Attribute.SysAttributeZone)
                    {
                        attr.Value = value;
                        return;
                    }
                Attributes.Add(new Types.Attribute
                {
                    Key = Types.Attribute.SysAttributeZone,
                    Value = value,
                });
            }
        }
    }
}
