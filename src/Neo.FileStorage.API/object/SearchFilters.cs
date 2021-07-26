using System.Collections.Generic;
using System.Linq;
using Neo.FileStorage.API.Refs;
using static Neo.FileStorage.API.Object.SearchRequest.Types.Body.Types;

namespace Neo.FileStorage.API.Object
{
    public class SearchFilters
    {
        private readonly List<Filter> filters = new();

        public Filter[] Filters => filters.ToArray();

        public SearchFilters() { }

        public SearchFilters(IEnumerable<Filter> fs)
        {
            filters = fs.ToList();
        }

        public void AddFilter(string name, string value, MatchType op)
        {
            filters.Add(new Filter
            {
                Key = name,
                Value = value,
                MatchType = op,
            });
        }

        public void AddObjectVersionFilter(MatchType op, Version v)
        {
            AddFilter(Filter.FilterHeaderVersion, v.String(), op);
        }

        public void AddObjectContainerIDFilter(MatchType op, ContainerID cid)
        {
            AddFilter(Filter.FilterHeaderContainerID, cid.ToBase58String(), op);
        }

        public void AddObjectOwnerIDFilter(MatchType op, OwnerID oid)
        {
            AddFilter(Filter.FilterHeaderOwnerID, oid.ToAddress(), op);
        }

        public void AddRootFilter()
        {
            AddFilter(Filter.FilterPropertyRoot, "", MatchType.Unspecified);
        }

        public void AddPhyFilter()
        {
            AddFilter(Filter.FilterPropertyPhy, "", MatchType.Unspecified);
        }

        public void AddParentIDFilter(MatchType op, ObjectID oid)
        {
            AddFilter(Filter.FilterHeaderParent, oid.ToBase58String(), op);
        }

        public void AddObjectIDFilter(MatchType op, ObjectID oid)
        {
            AddFilter(Filter.FilterHeaderObjectID, oid.ToBase58String(), op);
        }

        public void AddSplitIDFilter(MatchType op, SplitID sid)
        {
            AddFilter(Filter.FilterHeaderSplitID, sid.ToString(), op);
        }

        public void AddTypeFilter(MatchType op, ObjectType typ)
        {
            AddFilter(Filter.FilterHeaderObjectType, typ.ToString(), op);
        }
    }
}
