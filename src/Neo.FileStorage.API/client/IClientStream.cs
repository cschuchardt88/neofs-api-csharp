// Copyright (C) 2015-2025 The Neo Project.
//
// IClientStream.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Session;
using System;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public interface IClientStream : IDisposable
    {
        Task Write(IRequest request);

        Task<IResponse> Close();
    }
}
