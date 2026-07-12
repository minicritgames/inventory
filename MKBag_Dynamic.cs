using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minikit.Inventory
{
    /// <summary> A bag that grows slots on demand as items are looted, up to an optional capacity </summary>
    [Serializable]
    public class MKBag_Dynamic : MKBag
    {
        [SerializeField] private List<MKTag> slotTags = new();
        [SerializeField] private MKTagQuery itemTagQuery = new(MKTagQueryCondition.Any, null);
        [SerializeField] private int capacity = 0; // 0 = unbounded
        [SerializeField] private bool stacking = true;

        [NonSerialized] private List<MKSlot> slots = new();


        public MKBag_Dynamic()
        {
        }

        public MKBag_Dynamic(List<MKTag> _slotTags, MKTagQuery _itemTagQuery, int _capacity, bool _stacking)
        {
            slotTags = _slotTags;
            itemTagQuery = _itemTagQuery;
            capacity = _capacity;
            stacking = _stacking;
        }


        public override IEnumerable<MKSlot> IterateSlots()
        {
            slots ??= new();

            return slots;
        }

        public bool Accepts(MKItem _item)
        {
            return itemTagQuery == null
                || itemTagQuery.Test(_item.tags);
        }

        public override MKLootResult TryLoot(MKItem _item)
        {
            if (!Accepts(_item))
            {
                return MKLootResult.Failed;
            }

            slots ??= new();

            int remaining = _item.stackCount;
            bool mergedAny = false;
            if (stacking)
            {
                foreach (MKSlot slot in slots)
                {
                    if (slot.item != null
                        && !ReferenceEquals(slot.item, _item)
                        && slot.item.CanStackWith(_item))
                    {
                        remaining = slot.item.AddToStack(remaining);
                        mergedAny = true;

                        if (remaining <= 0)
                        {
                            RaiseChanged();

                            return MKLootResult.Merged;
                        }
                    }
                }
            }

            if (capacity > 0
                && slots.Count >= capacity)
            {
                // Full. Any amount we merged into existing stacks is kept; the leftover is dropped.
                if (mergedAny)
                {
                    RaiseChanged();

                    return MKLootResult.Merged;
                }

                return MKLootResult.Failed;
            }

            if (remaining != _item.stackCount)
            {
                _item.SetStackCount(remaining);
            }

            MKSlot newSlot = new()
            {
                slotTags = new List<MKTag>(slotTags),
                itemTagQuery = itemTagQuery
            };
            slots.Add(newSlot);
            newSlot.SetItem(_item);
            RaiseChanged();

            return MKLootResult.Placed;
        }

        public override bool Remove(MKItem _item)
        {
            slots ??= new();

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item == _item)
                {
                    slots[i].SetItem(null);
                    slots.RemoveAt(i);
                    RaiseChanged();

                    return true;
                }
            }

            return false;
        }
    }
} // Minikit.Inventory namespace
