// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.Selector.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Netmap
{
    public partial class Selector
    {
        public Selector(string name, string attr, Clause clause, uint count, string filter)
        {
            name_ = name;
            attribute_ = attr;
            clause_ = clause;
            count_ = count;
            filter_ = filter;
        }
    }
}
