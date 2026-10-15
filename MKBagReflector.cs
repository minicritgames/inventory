using System;
using System.Collections.Generic;

namespace Minikit.Inventory.Internal
{
    public static class MKBagReflector
    {
        private static List<Type> nativelyDefinedBagTypes;


        static MKBagReflector()
        {
            nativelyDefinedBagTypes = MKReflection.GetConcreteSubclassesOf<MKBag>();
        }

        
        public static List<Type> GetNativelyDefinedBagTypes()
        {
            return nativelyDefinedBagTypes;
        }
    }
} // Minikit.Inventory.Internal namespace
