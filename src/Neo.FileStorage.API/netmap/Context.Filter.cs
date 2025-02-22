// Copyright (C) 2015-2025 The Neo Project.
//
// Context.Filter.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;

namespace Neo.FileStorage.API.Netmap
{
    public partial class Context
    {
        public const string MainFilterName = "*";

        public bool ApplyFilter(string name, Node n)
        {
            return name == MainFilterName || Match(Filters[name], n);
        }

        public void ProcessFilters(PlacementPolicy policy)
        {
            foreach (var filter in policy.Filters)
            {
                ProcessFilter(filter, true);
            }
        }

        private void ProcessFilter(Filter filter, bool top)
        {
            if (filter is null) throw new ArgumentNullException(nameof(filter));
            if (filter.Name == MainFilterName) throw new ArgumentException($"{ErrInvalidFilterName}: '*' is reversed");
            if (top && filter.Name == "") throw new ArgumentException(ErrUnnamedTopFilter);
            if (!top && filter.Name != "" && !Filters.ContainsKey(filter.Name))
                throw new ArgumentException($"{ErrFilterNotFound}: {filter.Name}");
            switch (filter.Op)
            {
                case Operation.And:
                case Operation.Or:
                    {
                        foreach (var fl in filter.Filters)
                        {
                            ProcessFilter(fl, false);
                        }
                        break;
                    }
                default:
                    {
                        if (0 < filter.Filters.Count) throw new ArgumentException(ErrNonEmptyFilters);
                        if (!top && filter.Name != "") return;
                        switch (filter.Op)
                        {
                            case Operation.Eq:
                            case Operation.Ne:
                                break;
                            case Operation.Gt:
                            case Operation.Ge:
                            case Operation.Lt:
                            case Operation.Le:
                                {
                                    if (!ulong.TryParse(filter.Value, out ulong n))
                                        throw new ArgumentException($"{ErrInvalidNumber}: {filter.Value}");
                                    NumCache[filter] = n;
                                    break;
                                }
                            default:
                                throw new InvalidOperationException($"{ErrInvalidFilterOp}: {filter.Op}");
                        }
                        break;
                    }
            }
            if (top)
                Filters[filter.Name] = filter;
        }

        private bool MatchKeyValue(Filter filter, Node n)
        {
            switch (filter.Op)
            {
                case Operation.Eq:
                    if (n.Attributes.TryGetValue(filter.Key, out string value))
                    {
                        return value == filter.Value;
                    }
                    return false;
                case Operation.Ne:
                    if (n.Attributes.TryGetValue(filter.Key, out value))
                    {
                        return value != filter.Value;
                    }
                    return true;
                default:
                    {
                        ulong attribute;
                        switch (filter.Key)
                        {
                            case Node.AttributePrice:
                                attribute = n.Price;
                                break;
                            case Node.AttributeCapacity:
                                attribute = n.Capacity;
                                break;
                            default:
                                {
                                    if (!n.Attributes.ContainsKey(filter.Key))
                                        return false;
                                    if (!ulong.TryParse(n.Attributes[filter.Key], out attribute))
                                        return false;
                                    break;
                                }
                        }
                        switch (filter.Op)
                        {
                            case Operation.Gt:
                                return NumCache[filter] < attribute;
                            case Operation.Ge:
                                return NumCache[filter] <= attribute;
                            case Operation.Lt:
                                return attribute < NumCache[filter];
                            case Operation.Le:
                                return attribute <= NumCache[filter];
                        }
                        break;
                    }
            }
            return false;
        }

        public bool Match(Filter filter, Node n)
        {
            switch (filter.Op)
            {
                case Operation.And:
                case Operation.Or:
                    {
                        foreach (var fl in filter.Filters)
                        {
                            Filter f = fl;
                            if (fl.Name != "")
                                f = Filters[fl.Name];
                            var r = Match(f, n);
                            if (r == (filter.Op == Operation.Or))
                                return r;
                        }
                        return filter.Op == Operation.And;
                    }
                default:
                    return MatchKeyValue(filter, n);
            }
        }
    }
}
