// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.SessionToken.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Cryptography;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Session
{
    public sealed partial class SessionToken
    {
        public static partial class Types
        {
            public sealed partial class Body
            {
                public static partial class Types
                {
                    public sealed partial class TokenLifetime
                    {
                    }
                }
            }
        }

        public void Sign(ECDsa key)
        {
            Signature = key.SignMessagePart(Body);
        }

        public bool VerifySignature()
        {
            return Signature.VerifyMessagePart(Body);
        }
    }
}
