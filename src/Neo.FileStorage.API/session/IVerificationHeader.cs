// Copyright (C) 2015-2025 The Neo Project.
//
// IVerificationHeader.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.Session
{
    public interface IVerificationHeader : IMessage
    {
        Signature BodySignature { get; set; }
        Signature MetaSignature { get; set; }
        Signature OriginSignature { get; set; }
        IVerificationHeader GetOrigin();
        void SetOrigin(IVerificationHeader verificationHeader);
    }
}
