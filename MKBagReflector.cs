using System;
using System.Collections.Generic;
using System.Reflection;

namespace Minikit.Inventory.Internal
{
    public static class MKBagReflector
    {
        private static List<Type> nativelyDefinedBagTypes = new();


        static MKBagReflector()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(MKBag))
                        && !type.IsAbstract) // Ignore abstract classes since we don't want to register them
                    {
                        nativelyDefinedBagTypes.Add(type);

                        //Debug.Log($"Registered {nameof(MKBag)}: {type.Name}");
                    }
                }
            }
        }

        
        public static List<Type> GetNativelyDefinedBagTypes()
        {
            return nativelyDefinedBagTypes;
        }
    }
} // Minikit.Inventory.Internal namespace
