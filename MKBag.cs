using System;
using System.Collections.Generic;

namespace Minikit.Inventory
{
    public enum MKLootResult
    {
        Failed, // The bag could not hold the item
        Placed, // The item was placed into a new/empty slot
        Merged // The item was merged into an existing stack (the passed instance is discarded)
    }

    /// <summary> A storage region within an inventory with their own slots and placement rules </summary>
    [Serializable]
    public abstract class MKBag
    {
        [NonSerialized] public Action OnChanged;


        public abstract IEnumerable<MKSlot> IterateSlots();

        public abstract MKLootResult TryLoot(MKItem _item);

        public abstract bool Remove(MKItem _item);

        protected void RaiseChanged()
        {
            OnChanged?.Invoke();
        }
    }
} // Minikit.Inventory namespace
