using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Minikit.Inventory
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MKShardAttribute : Attribute
    {
        public string shardType;
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MKShardHiddenAttribute : Attribute
    {
    }
} // Minikit.Inventory namespace

namespace Minikit.Inventory.Internal
{
    public static class MKShardReflector
    {
        private static Dictionary<string, Type> nativelyDefinedShardTypesByName = new();


        static MKShardReflector()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(MKShard))
                        && !type.IsAbstract // Ignore abstract classes since we don't want to register them
                        && type.GetCustomAttribute<MKShardAttribute>() is MKShardAttribute shardAttribute // Make sure it has a shard attribute
                        && !string.IsNullOrEmpty(shardAttribute.shardType) // and the shard attribute has a valid type
                        && type.GetCustomAttribute<MKShardHiddenAttribute>() == null) // Ignore shards that requested to be hidden
                    {
                        nativelyDefinedShardTypesByName.Add(shardAttribute.shardType, type);

                        //Debug.Log($"Registered {nameof(MKShard)}: {type.Name} with key {shardAttribute.shardType}");
                    }
                }
            }
        }


        public static Type GetRegisteredShardType(string _key)
        {
            if (nativelyDefinedShardTypesByName.ContainsKey(_key))
            {
                return nativelyDefinedShardTypesByName[_key];
            }

            Debug.LogError($"Failed to get registered {nameof(MKShard)} type from key {_key}");
            return null;
        }

        public static string GetRegisteredShardTypeName(Type _type)
        {
            foreach (KeyValuePair<string, Type> pair in nativelyDefinedShardTypesByName)
            {
                if (pair.Value == _type)
                {
                    return pair.Key;
                }
            }

            return null;
        }

        public static List<string> GetNativelyDefinedShardTypes()
        {
            return nativelyDefinedShardTypesByName.Keys.ToList();
        }
    }
} // Minikit.Inventory.Internal namespace
