using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Minikit.Inventory
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ItemDefinition", menuName = "Minikit/Inventory/ItemDefinition", order = 1)]
    public class MKItemDefinitionScriptableObject : ScriptableObject
    {
        // WARNING!!! If these properties are renamed, serialized data within is lost. Most, if not all, data for an ItemDefinition is within shards
        // [SerializeField] doesn't allow children of the declared type, we use SerializeReference here

        /// <summary> Tags that only exist on the item definition </summary>
        [SerializeField] private List<MKTag> tags = new();

        /// <summary> Static Shards contain data that never changes. Static shards cannot be added or removed at runtime, and don't network </summary>
        [SerializeReference] private List<MKShard> staticShards;

        /// <summary> Dynamic Shards contain data that may change. Dynamic shards CAN be added and removed at runtime, and DO network </summary>
        [SerializeReference] private List<MKShard> dynamicShards;


        public List<MKTag> GetTags()
        {
            return tags;
        }

        public MKShard GetFirstStaticShard(MKTagQuery _tagQuery = null)
        {
            return GetAllStaticShards(_tagQuery, true).FirstOrDefault();
        }

        public T GetFirstStaticShard<T>(MKTagQuery _tagQuery = null) where T : MKShard
        {
            return GetAllStaticShards<T>(_tagQuery, true).FirstOrDefault();
        }

        public List<MKShard> GetAllStaticShards(MKTagQuery _tagQuery = null, bool _returnFirst = false)
        {
            return GetAllStaticShards<MKShard>(_tagQuery);
        }

        public List<T> GetAllStaticShards<T>(MKTagQuery _tagQuery = null, bool _returnFirst = false) where T : MKShard
        {
            List<T> foundShards = new();
            foreach (MKShard shard in staticShards)
            {
                if (shard.GetType().IsAssignableFrom(typeof(T)))
                {
                    if (_tagQuery?.Test(shard.tags) ?? true) // If no query is supplied, pass every shard
                    {
                        foundShards.Add(shard as T);

                        if (_returnFirst)
                        {
                            return foundShards;
                        }
                    }
                }
            }
            return foundShards;
        }

        public MKShard GetFirstDynamicShard(MKTagQuery _tagQuery = null)
        {
            return GetAllDynamicShards(_tagQuery, true).FirstOrDefault();
        }

        public T GetFirstDynamicShard<T>(MKTagQuery _tagQuery = null) where T : MKShard
        {
            return GetAllDynamicShards<T>(_tagQuery, true).FirstOrDefault();
        }

        public List<MKShard> GetAllDynamicShards(MKTagQuery _tagQuery = null, bool _returnFirst = false)
        {
            return GetAllDynamicShards<MKShard>(_tagQuery);
        }

        public List<T> GetAllDynamicShards<T>(MKTagQuery _tagQuery = null, bool _returnFirst = false) where T : MKShard
        {
            List<T> foundShards = new();
            foreach (MKShard shard in dynamicShards)
            {
                if (shard.GetType().IsAssignableFrom(typeof(T)))
                {
                    if (_tagQuery?.Test(shard.tags) ?? true) // If no query is supplied, pass every shard
                    {
                        foundShards.Add(shard as T);

                        if (_returnFirst)
                        {
                            return foundShards;
                        }
                    }
                }
            }
            return foundShards;
        }

#if UNITY_EDITOR
        public void AddStaticShard(MKShard _shard)
        {
            staticShards.Add(_shard);
        }

        public void AddDynamicShard(MKShard _shard)
        {
            dynamicShards.Add(_shard);
        }
#endif // UNITY_EDITOR
    }
} // Minikit.Inventory namespace
