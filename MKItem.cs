using System;
using System.Linq;
using System.Collections.Generic;
using Minikit.Inventory.Internal;
using UnityEngine;

namespace Minikit.Inventory
{
    /// <summary> Represents a single object that can be held within a bag </summary>
    public class MKItem
    {
        public List<MKTag> tags = new();

        protected List<MKShard> dynamicShards = new();
        public MKItemDefinitionScriptableObject itemDefinition { get; private set; }
        
        public bool stackable => GetStackableShard() != null;
        public int stackCount => GetStackCountShard()?.current ?? 1;
        public int stackMax => GetStackableShard()?.max ?? 1;

        
        public MKItem(MKItemDefinitionScriptableObject _itemDefinition, List<MKShard> _additionalDynamicShards = null)
        {
            itemDefinition = _itemDefinition;

            tags = _itemDefinition.GetTags();

            dynamicShards.AddRange(itemDefinition.GetAllDynamicShards());
            if (_additionalDynamicShards != null)
            {
                dynamicShards.AddRange(_additionalDynamicShards);
            }

            CreateCompanionShards();
        }
        

        public void SetStackCount(int _count)
        {
            MKShard_StackCount countShard = GetStackCountShard();
            if (countShard == null)
            {
                return;
            }

            countShard.current = Mathf.Clamp(_count, 0, stackMax);
        }

        public int AddToStack(int _amount)
        {
            if (_amount <= 0)
            {
                return 0;
            }

            MKShard_StackCount countShard = GetStackCountShard();
            if (countShard == null)
            {
                return _amount;
            }

            int added = Mathf.Min(stackMax - countShard.current, _amount);
            countShard.current += added;

            return _amount - added;
        }

        public virtual bool CanStackWith(MKItem _other)
        {
            if (_other == null
                || ReferenceEquals(_other, this))
            {
                return false;
            }

            if (_other.itemDefinition != itemDefinition)
            {
                return false;
            }

            if (!stackable
                || !_other.stackable)
            {
                return false;
            }

            if (HasNonStackDynamicShards()
                || _other.HasNonStackDynamicShards())
            {
                return false;
            }

            if (tags.Count != _other.tags.Count)
            {
                return false;
            }

            foreach (MKTag tag in tags)
            {
                if (!_other.tags.Contains(tag))
                {
                    return false;
                }
            }

            return true;
        }
        public MKShard GetFirstShard(MKTagQuery _tagQuery = null)
        {
            return GetAllShards(_tagQuery, true).FirstOrDefault();
        }

        public T GetFirstShard<T>(MKTagQuery _tagQuery = null) where T : MKShard
        {
            return GetAllShards<T>(_tagQuery, true).FirstOrDefault();
        }

        public List<MKShard> GetAllShards(MKTagQuery _tagQuery = null, bool _returnFirst = false)
        {
            return GetAllShards<MKShard>(_tagQuery);
        }

        public List<T> GetAllShards<T>(MKTagQuery _tagQuery = null, bool _returnFirst = false) where T : MKShard
        {
            List<T> foundShards = new();
            foreach (MKShard dynamicShard in dynamicShards)
            {
                if (dynamicShard.GetType().IsAssignableFrom(typeof(T)))
                {
                    if (_tagQuery?.Test(dynamicShard.tags) ?? true) // If no query is supplied, pass every shard
                    {
                        foundShards.Add(dynamicShard as T);

                        if (_returnFirst)
                        {
                            return foundShards;
                        }
                    }
                }
            }

            // Search for static shards on the item definition directly
            foundShards.AddRange(itemDefinition.GetAllStaticShards<T>(_tagQuery, _returnFirst));

            return foundShards;
        }

        private MKShard_Stackable GetStackableShard()
        {
            return GetFirstShard<MKShard_Stackable>();
        }

        private MKShard_StackCount GetStackCountShard()
        {
            return GetFirstShard<MKShard_StackCount>();
        }

        private bool HasNonStackDynamicShards()
        {
            foreach (MKShard shard in dynamicShards)
            {
                if (shard is not MKShard_StackCount)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary> Auto-creates the dynamic companion for each static shard on the definition that declares one </summary>
        private void CreateCompanionShards()
        {
            foreach (MKShard staticShard in itemDefinition.GetAllStaticShards())
            {
                if (staticShard.GetCompanionShard() is Type companionShardType)
                {
                    foreach (MKShard shard in dynamicShards)
                    {
                        if (shard.GetType() == companionShardType)
                        {
                            // Skip creating the dynamic companion shard if we already have one
                            continue;
                        }
                    }

                    if (Activator.CreateInstance(companionShardType) is MKShard companionShard)
                    {
                        dynamicShards.Add(companionShard);
                    }
                }
            }
        }
    }
} // Minikit.Inventory namespace
