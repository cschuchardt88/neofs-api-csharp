using System;
using System.Collections.Generic;

namespace Neo.FileStorage.API.Object
{
    public static class ObjectTypeExtension
    {
        private static readonly Dictionary<ObjectType, string> ObjectTypeNames = new()
        {
            { ObjectType.Regular, "REGULAR" },
            { ObjectType.Tombstone, "TOMBSTONE" },
            { ObjectType.StorageGroup, "STORAGEGROUP" }
        };
        private static readonly Dictionary<string, ObjectType> ObjectTypeValues = new()
        {
            { "REGULAR", ObjectType.Regular },
            { "TOMBSTONE", ObjectType.Tombstone },
            { "STORAGEGROUP", ObjectType.StorageGroup }
        };

        public static string String(this ObjectType t)
        {
            if (ObjectTypeNames.TryGetValue(t, out var name))
                return name;
            throw new InvalidOperationException("Invalid object type");
        }

        public static ObjectType ToObjectType(this string t)
        {
            if (ObjectTypeValues.TryGetValue(t, out var type))
                return type;
            throw new InvalidOperationException("Invalid object type string");
        }
    }
}
