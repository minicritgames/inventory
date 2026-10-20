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

        public virtual bool TryPlace(MKItem _item, MKSlot _slot)
        {
            foreach (MKSlot slot in IterateSlots())
            {
                if (slot == _slot
                    && slot.CanHoldItem(_item))
                {
                    slot.SetItem(_item);
                    RaiseChanged();

                    return true;
                }
            }

            return false;
        }

        protected void RaiseChanged()
        {
            OnChanged?.Invoke();
        }
    }
} // Minikit.Inventory namespace
