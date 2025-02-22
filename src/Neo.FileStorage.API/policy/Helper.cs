// Copyright (C) 2015-2025 The Neo Project.
//
// Helper.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Netmap;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.FileStorage.API.Policy
{
    public static class Helper
    {
        public const char ExprSpliter = '|';

        public static Operation ToOperation(this string str)
        {
            var s = str.ToUpper();
            return s switch
            {
                "EQ" => Operation.Eq,
                "NE" => Operation.Ne,
                "GT" => Operation.Gt,
                "GE" => Operation.Ge,
                "LT" => Operation.Lt,
                "LE" => Operation.Le,
                "OR" => Operation.Or,
                "AND" => Operation.And,
                _ => throw new ArgumentException(),
            };
        }

        public static string AsString(this Operation op)
        {
            return op switch
            {
                Operation.Eq => "EQ",
                Operation.Ne => "NE",
                Operation.Gt => "GT",
                Operation.Ge => "GE",
                Operation.Lt => "LT",
                Operation.Le => "LE",
                Operation.And => "AND",
                Operation.Or => "OR",
                _ => throw new ArgumentException(),
            };
        }

        public static Clause ToClause(this string str)
        {
            var s = str.ToLower();
            return s switch
            {
                "unspecified" => Clause.Unspecified,
                "0" => Clause.Unspecified,
                "" => Clause.Unspecified,
                "same" => Clause.Same,
                "1" => Clause.Same,
                "distinct" => Clause.Distinct,
                "2" => Clause.Distinct,
                _ => throw new ArgumentException(),
            };
        }

        public static string AsString(this Clause clause)
        {
            return clause switch
            {
                Clause.Same => "SAME",
                Clause.Distinct => "DISTINCT",
                Clause.Unspecified => "",
                _ => throw new ArgumentException(),
            };
        }


        #region query

        public static readonly Parser<string> QuoteNameParser =
            from _1 in Parse.Chars('"', '\'')
            from n1 in NameParser
            from n2 in SpaceNameParser.Many()
            from _2 in Parse.Chars('"', '\'')
            select n1 + string.Join("", n2);

        public static readonly Parser<string> SpaceNameParser =
            from _ in Parse.Char(' ')
            from n in NameParser.AtLeastOnce()
            select " " + string.Join("", n);

        public static readonly Parser<string> NameParser =
            from first in Parse.Letter.Once()
            from body in Parse.LetterOrDigit.Or(Parse.Chars('-')).Many()
            select new string(first.Concat(body).ToArray());

        public static readonly Parser<string> BlankParser =
           from s in Parse.WhiteSpace.Many().Text().Or(Parse.LineEnd).Or(Parse.String("\t").Text())
           select s;

        public static readonly Parser<char> Digit1Parser =                       // '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' 
            from leading in BlankParser.Many()
            from digit1 in Parse.Digit.Except(Parse.Char('0'))
            select digit1;

        public static readonly Parser<uint> Number1Parser =                      // Digit1[Digit];
            from leading in BlankParser.Many()
            from n1 in Digit1Parser // cannot start with '0'
            from n in Parse.Digit.Many()
            select uint.Parse(new string(new char[] { n1 }.Concat(n).ToArray()));

        public static readonly Parser<string> NumberParser =
            from leading in BlankParser.Many()
            from n in Parse.Digit.AtLeastOnce()
            select new string(n.ToArray());

        public static readonly Parser<string> IdentParser =
            from _ in BlankParser.Many()
            from n in NameParser.Or(QuoteNameParser)
            select n;

        public static readonly Parser<string> AtIdentParser =
            from leading in BlankParser.Many()
            from at in Parse.Char('@')
            from id in IdentParser
            select new string(at.ToString().Concat(id).ToArray());

        public static readonly Parser<string> StringParser =
            from leading in BlankParser.Many()
            from s in Parse.AtLeastOnce(Parse.CharExcept(' '))
            select new string(s.ToArray());

        public static readonly Parser<string> ValueParser =
            from leading in BlankParser.Many()
            from v in IdentParser.Or(NumberParser).Or(StringParser)
            select v;

        public static readonly Parser<string> OpParser =
            from leading in BlankParser.Many()
            from op in Parse.String("EQ")
                .Or(Parse.String("NE"))
                .Or(Parse.String("GE"))
                .Or(Parse.String("GT"))
                .Or(Parse.String("LE"))
                .Or(Parse.String("LT"))
            select new string(op.ToArray());

        public static readonly Parser<string> AttributeFilterParser =
            from leading in BlankParser.Many()
            from ident in IdentParser
            from space1 in BlankParser.AtLeastOnce()
            from op in OpParser
            from space2 in BlankParser.AtLeastOnce()
            from value in ValueParser
            select string.Join(ExprSpliter, new[] { ident, op, value });

        public static readonly Parser<SimpleExpr> SimpleExprParser =
            from leading in BlankParser.Many()
            from key in IdentParser
            from space1 in BlankParser.AtLeastOnce()
            from op in OpParser
            from space2 in BlankParser.AtLeastOnce()
            from value in ValueParser
            select new SimpleExpr(key, op, value);

        //Expr::=
        //    '@' Ident(* filter reference *)
        //  | Ident, Op, Value(* attribute filter *)
        //;
        public static readonly Parser<FilterOrExpr> FilterOrExprParser =
            from leading in BlankParser.Many()
            from expr in AtIdentParser.Or(AttributeFilterParser)
            select new FilterOrExpr(expr);

        public static readonly Parser<FilterOrExpr> AndExprParser =
           from leading in BlankParser.Many()
           from and in Parse.String("AND")
           from space1 in BlankParser.AtLeastOnce()
           from f in FilterOrExprParser
           select f;

        //AndChain::=
        //    Expr, ['AND', Expr]
        //;
        public static readonly Parser<AndChain> AndChainParser =
            from leading in BlankParser.Many()
            from expr1 in FilterOrExprParser
            from space1 in BlankParser.AtLeastOnce()
            from exprs in AndExprParser.Many()
            select new AndChain(new FilterOrExpr[] { expr1 }.Concat(exprs).ToArray());

        public static readonly Parser<AndChain> OrExprParser =
            from leading in BlankParser.Many()
            from or in Parse.String("OR")
            from space1 in BlankParser.AtLeastOnce()
            from a in AndChainParser
            select a;

        //OrChain::=
        //    AndChain, ['OR', AndChain]
        //;
        public static readonly Parser<OrChain> OrChainParser =
            from leading in BlankParser.Many()
            from and1 in AndChainParser
            from space1 in BlankParser.AtLeastOnce()
            from ands in OrExprParser.Many()
            select new OrChain(new AndChain[] { and1 }.Concat(ands).ToArray());

        public static readonly Parser<string> AsIdentParser =
            from leading in BlankParser.Many()
            from a in Parse.String("AS")
            from space1 in BlankParser.AtLeastOnce()
            from id in IdentParser
            select id;

        public static readonly Parser<string> InIdentParser =
            from leading in BlankParser.Many()
            from a in Parse.String("IN")
            from space1 in BlankParser.AtLeastOnce()
            from id in IdentParser
            select id;

        //FilterStmt::=
        //    'FILTER', AndChain, ['OR', AndChain],
        //    'AS', Ident (* obligatory filter name *)
        //;
        public static readonly Parser<FilterStmt> FilterStmtParser =
            from leading in BlankParser.Many()
            from filter in Parse.String("FILTER")
            from space1 in BlankParser.AtLeastOnce()
            from or in OrChainParser // should only have one OrChain for each filter
            from space2 in BlankParser.AtLeastOnce()
            from a in AsIdentParser
            select new FilterStmt(or, a);

        public static readonly Parser<string> ClauseStringParser =
            from leading in BlankParser.Many()
            from c in Parse.String("SAME").Or(Parse.String("DISTINCT"))
            select new string(c.ToArray());

        public static readonly Parser<string[]> InClauseIdentParser =
            from leading in BlankParser.Many()
            from a in Parse.String("IN")
            from space1 in BlankParser.AtLeastOnce()
            from clause in ClauseStringParser.Optional()
            from space2 in BlankParser.AtLeastOnce()
            from bucket in IdentParser
            select clause.IsEmpty ? new string[] { bucket } : new string[] { clause.Get(), bucket };

        //SelectStmt::=
        //    'SELECT', Number1,       (* number of nodes to select without container backup factor*)
        //    ('IN', Clause?, Ident)?, (* bucket name *)
        //    FROM, (Ident | '*'),     (* filter reference or whole netmap*)
        //    ('AS', Ident)?           (* optional selector name*)
        //;
        public static readonly Parser<SelectorStmt> SelectorStmtParser =
            from leading in BlankParser.Many()
            from selectStr in Parse.String("SELECT")
            from space1 in BlankParser.AtLeastOnce()
            from n1 in Number1Parser
            from space2 in BlankParser.AtLeastOnce()
            from ss in InClauseIdentParser.Optional()
            from space3 in BlankParser.AtLeastOnce()
            from fromStr in Parse.String("FROM")
            from space4 in BlankParser.AtLeastOnce()
            from filter in IdentParser.Or(Parse.String("*")).Text()
            from name in AsIdentParser.Optional()
            select new SelectorStmt(n1, ss.GetOrElse(Array.Empty<string>()), filter, name.GetOrElse(""));

        public static readonly Parser<uint> CbfParser =
            from leading in BlankParser.Many()
            from cbf in Parse.String("CBF")
            from space1 in BlankParser.AtLeastOnce()
            from n in Number1Parser
            select n;

        public static readonly Parser<ReplicaStmt> ReplicaStmtParser =
            from leading in BlankParser.Many()
            from rep in Parse.String("REP")
            from space1 in BlankParser.AtLeastOnce()
            from n1 in Number1Parser
            from space2 in BlankParser.Many()
            from selector in InIdentParser.Optional()
            select new ReplicaStmt(n1, selector.GetOrElse(""));

        public static readonly Parser<Query> QueryParser =
            from leading in BlankParser.Many()
            from replicas in ReplicaStmtParser.AtLeastOnce()
            from space1 in BlankParser.Many()
            from cbf in CbfParser.Optional()
            from space2 in BlankParser.Many()
            from selectors in SelectorStmtParser.Many()
            from space3 in BlankParser.Many()
            from filters in FilterStmtParser.Many()
            select new Query(replicas.ToArray(), cbf.GetOrElse<uint>(0), selectors.ToArray(), filters.ToArray());
        #endregion


        public static PlacementPolicy ParsePlacementPolicy(string s)
        {
            var q = Query.Parse(s);

            var seenFilters = new Dictionary<string, bool>();
            var fs = new List<Filter>();
            foreach (var qf in q.Filters)
            {
                var f = FilterFromOrChain(qf.Value, seenFilters);
                f.Name = qf.Name;
                fs.Add(f);
                seenFilters[qf.Name] = true;
            }

            var seenSelectors = new Dictionary<string, bool>();
            var ss = new List<Selector>();
            foreach (var qs in q.Selectors)
            {
                if (qs.Filter != "*" && (!seenFilters.ContainsKey(qs.Filter) || !seenFilters[qs.Filter]))
                    throw new ParseException("unknown filter " + qs.Filter);

                var sel = new Selector();
                switch (qs.Bucket.Length)
                {
                    case 1: // only bucket
                        sel.Attribute = qs.Bucket[0];
                        break;
                    case 2: // clause + bucket
                        sel.Clause = qs.Bucket[0].ToClause();
                        sel.Attribute = qs.Bucket[1];
                        break;
                }
                sel.Name = qs.Name;
                seenSelectors[qs.Name] = true;
                sel.Filter = qs.Filter;
                if (qs.Count == 0)
                    throw new ParseException("policy: expected positive integer");
                sel.Count = qs.Count;

                ss.Add(sel);
            }

            var rs = new List<Replica>();
            foreach (var qr in q.Replicas)
            {
                var r = new Replica();

                if (qr.Selector != "")
                {
                    if (!seenSelectors.ContainsKey(qr.Selector) || !seenSelectors[qr.Selector])
                        throw new ParseException("policy: selector not found");
                    r.Selector = qr.Selector;
                }
                if (qr.Count == 0)
                    throw new ParseException("policy: expected positive integer");
                r.Count = qr.Count;
                rs.Add(r);
            }

            return new PlacementPolicy(q.CBF, rs, ss, fs);
        }


        public static Filter FilterFromOrChain(OrChain expr, Dictionary<string, bool> seen)
        {
            var fs = new List<Filter>();
            foreach (var ac in expr.Clauses)
            {
                var f = FilterFromAndChain(ac, seen);
                fs.Add(f);
            }
            if (fs.Count == 1)
                return fs[0];

            return new Filter("", "", "", Operation.Or, fs.ToArray());
        }

        public static Filter FilterFromAndChain(AndChain expr, Dictionary<string, bool> seen)
        {
            var fs = new List<Filter>();
            foreach (var fe in expr.Clauses)
            {
                Filter f;
                if (fe.Expr != null)
                    f = FilterFromSimpleExpr(fe.Expr);
                else
                    f = new Filter() { Name = fe.Reference };
                fs.Add(f);
            }
            if (fs.Count == 1)
                return fs[0];
            return new Filter("", "", "", Operation.And, fs.ToArray());
        }

        public static Filter FilterFromSimpleExpr(SimpleExpr se)
        {
            return new Filter("", se.Key, se.Value, se.Op.ToOperation(), null);
        }


        // code below is deprecated

        //public static Parser<uint> ReplFactorParser =
        //    from leading in BlankParser.Many()
        //    from RF in Parse.String("RF")
        //    from space in BlankParser.AtLeastOnce()
        //    from num in Parse.Number
        //    select uint.Parse(num);

        //public static Parser<Operation> OperationParser =
        //    from space1 in BlankParser.Many()
        //    from op in Parse.AtLeastOnce(Parse.CharExcept(' '))
        //    select new string(op.ToArray()).ToOperation();

        //public static Parser<Clause> ClauseParser =
        //    from space1 in BlankParser.Many()
        //    from clause in Parse.LetterOrDigit.AtLeastOnce()
        //    select new string(clause.ToArray()).ToClause();

        //public static Parser<Filter> FilterParser =
        //    from space1 in BlankParser.Many()
        //    from name in Parse.LetterOrDigit.Many() // name, can be letters or ""
        //    from space in BlankParser.AtLeastOnce()
        //    from key in Parse.Letter.Many() // key, can only be letters or ""
        //    from space2 in BlankParser.AtLeastOnce()
        //    from value in Parse.AnyChar.Many() // value, can be any or ""
        //    from space3 in BlankParser.AtLeastOnce()
        //    from op in OperationParser // op
        //    from space4 in BlankParser.AtLeastOnce()
        //    from filters in FilterParser.XMany() // recursive call
        //    select new Filter(new string(name.ToArray()), new string(key.ToArray()), new string(value.ToArray()), op, filters.ToArray());

        //public static Parser<Filter[]> FilterGroupParser =
        //    from leading in BlankParser.Many()
        //    from filter_str in Parse.String("FILTER")
        //    from filters in FilterParser.XAtLeastOnce()
        //    select filters.ToArray();

        //public static Parser<Selector> SelectorParser =
        //    from space1 in BlankParser.Many()
        //    from name in Parse.Letter.AtLeastOnce().Text() // name
        //    from space2 in BlankParser.AtLeastOnce()
        //    from attr in Parse.Letter.AtLeastOnce().Text() // attribute
        //    from space3 in BlankParser.AtLeastOnce()
        //    from clause in ClauseParser // clause
        //    from space4 in BlankParser.AtLeastOnce()
        //    from count in Parse.Number // count
        //    from space5 in BlankParser.AtLeastOnce()
        //    from filter in Parse.Letter.AtLeastOnce().Text() // filter name
        //    select new Selector(uint.Parse(count), clause,  attr, filter, name);

        //public static Parser<Selector[]> SelectorGroupParser =
        //    from space1 in BlankParser.Many()
        //    from selectors in SelectorParser.XAtLeastOnce()
        //    select selectors.ToArray();

        //public static Parser<Replica> ReplicaParser =
        //    from space1 in BlankParser.Many()
        //    from count in Parse.Number // count
        //    from space2 in BlankParser.AtLeastOnce()
        //    from selector in Parse.Letter.AtLeastOnce().Text() // selector name
        //    select new Replica(uint.Parse(count), selector);

        //public static Parser<Replica[]> ReplicaGroupParser =
        //    from space1 in BlankParser.Many()
        //    from replicas in ReplicaParser.XAtLeastOnce()
        //    select replicas.ToArray();

        //public static Parser<PlacementPolicy> PlacementPolicyParser =
        //    from space1 in BlankParser.Many()
        //    from bf in Parse.Number
        //    from replicas in ReplicaGroupParser
        //    from selectors in SelectorGroupParser
        //    from filters in FilterGroupParser
        //    select new PlacementPolicy(uint.Parse(bf), replicas, selectors, filters);
    }
}
