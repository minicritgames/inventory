using System.Collections.Generic;
using UnityEngine;

namespace Minikit.Inventory
{
    public static class MKItemExtensions
    {
        public static T GetFirstShardWithTag<T>(this MKItem _item, MKTag _tag) where T : MKShard
        {
            return _item?.GetFirstShard<T>(new MKTagQuery(MKTagQueryCondition.All, new() { _tag }));
        }
        
        public static T GetFirstShardWithAllTags<T>(this MKItem _item, List<MKTag> _tags) where T : MKShard
        {
            return _item?.GetFirstShard<T>(new MKTagQuery(MKTagQueryCondition.All, _tags));
        }

        public static List<T> GetAllShardsWithTag<T>(this MKItem _item, MKTag _tag) where T : MKShard
        {
            return _item?.GetAllShards<T>(new MKTagQuery(MKTagQueryCondition.All, new() { _tag }));
        }
        
        public static List<T> GetAllShardsWithAllTags<T>(this MKItem _item, List<MKTag> _tags) where T : MKShard
        {
            return _item?.GetAllShards<T>(new MKTagQuery(MKTagQueryCondition.All, _tags));
        }
    }
} // Minikit.Inventory namespace
