using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minikit.Inventory
{
    /// <summary> A bag with a fixed, authored set of slots </summary>
    [Serializable]
    public class MKBag_Fixed : MKBag
    {
        [SerializeField] private List<MKSlot> slots = new();


        public MKBag_Fixed()
        {
        }

        public MKBag_Fixed(List<MKSlot> _slots)
        {
            slots = _slots;
        }


        public override IEnumerable<MKSlot> IterateSlots()
        {
            return slots;
        }

        public override MKLootResult TryLoot(MKItem _item)
        {
            foreach (MKSlot slot in slots)
            {
                if (!slot.slotTags.Contains(MKTag.Get(MKInventoryTags.slotNoLoot))
                    && slot.CanHoldItem(_item))
                {
                    slot.SetItem(_item);
                    RaiseChanged();

                    return MKLootResult.Placed;
                }
            }

            return MKLootResult.Failed;
        }

        public override bool Remove(MKItem _item)
        {
            foreach (MKSlot slot in slots)
            {
                if (slot.item == _item)
                {
                    slot.SetItem(null);
                    RaiseChanged();

                    return true;
                }
            }

            return false;
        }
    }
} // Minikit.Inventory namespace
