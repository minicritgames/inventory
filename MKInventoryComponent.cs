using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Minikit.Inventory
{
    /// <summary> Represents a collection of bags that can be held on a GameObject </summary>
    public class MKInventoryComponent : MonoBehaviour
    {
        [SerializeField] protected List<MKSlot> slots = new();


        public void AddSlot(MKSlot _slot)
        {
            if (!slots.Contains(_slot))
            {
                slots.Add(_slot);
            }
        }

        public void AddSlots(List<MKSlot> _slots)
        {
            foreach (MKSlot slot in _slots)
            {
                AddSlot(slot);
            }
        }

        public void RemoveSlot(MKSlot _slot)
        {
            if (slots.Contains(_slot))
            {
                slots.Remove(_slot);
            }
        }

        public void RemoveSlots(List<MKSlot> _slots)
        {
            foreach (MKSlot slot in _slots)
            {
                RemoveSlot(slot);
            }
        }

        public virtual bool LootItem(MKItem _item)
        {
            foreach (MKSlot slot in slots)
            {
                if (slot.CanHoldItem(_item))
                {
                    slot.SetItem(_item);
                    OnItemAdded(_item);

                    return true;
                }
            }

            return false;
        }

        public bool RemoveItem(MKItem _item)
        {
            foreach (MKSlot slot in slots)
            {
                if (slot.item == _item)
                {
                    slot.SetItem(null);
                    OnItemRemoved(_item);

                    return true;
                }
            }

            return false;
        }

        public MKSlot GetFirstSlot(MKTagQuery _slotTagQuery = null)
        {
            return GetFirstSlot<MKSlot>(_slotTagQuery);
        }

        public T GetFirstSlot<T>(MKTagQuery _slotTagQuery = null) where T : MKSlot
        {
            foreach (MKSlot itSlot in IterateSlots())
            {
                if (_slotTagQuery != null
                    && !_slotTagQuery.Test(itSlot.slotTags))
                {
                    continue;
                }

                if (itSlot is T slot)
                {
                    return slot;
                }
            }

            return null;
        }
        
        public List<MKSlot> GetSlots(MKTagQuery _slotTagQuery = null)
        {
            return GetSlots<MKSlot>(_slotTagQuery);
        }

        public List<T> GetSlots<T>(MKTagQuery _slotTagQuery = null) where T : MKSlot
        {
            List<T> foundSlots = new();
            foreach (MKSlot itSlot in IterateSlots())
            {
                if (_slotTagQuery != null
                    && !_slotTagQuery.Test(itSlot.slotTags))
                {
                    continue;
                }

                if (itSlot is T slot)
                {
                    foundSlots.Add(slot);
                }
            }

            return foundSlots;
        }

        public MKItem GetFirstItem(MKTagQuery _slotTagQuery = null, MKTagQuery _itemTagQuery = null)
        {
            return GetFirstItem<MKItem>(_slotTagQuery, _itemTagQuery);
        }
        
        public T GetFirstItem<T>(MKTagQuery _slotTagQuery = null, MKTagQuery _itemTagQuery = null) where T : MKItem
        {
            foreach (MKSlot slot in IterateSlots())
            {
                if (_slotTagQuery != null
                    && !_slotTagQuery.Test(slot.slotTags))
                {
                    continue;
                }

                if (slot.item is T item)
                {
                    if (_itemTagQuery != null
                        && !_itemTagQuery.Test(item.tags))
                    {
                        continue;
                    }

                    return item;
                }
            }

            return null;
        }
        
        public List<MKItem> GetItems(MKTagQuery _slotTagQuery = null, MKTagQuery _itemTagQuery = null)
        {
            return GetItems<MKItem>(_slotTagQuery, _itemTagQuery);
        }

        public List<T> GetItems<T>(MKTagQuery _slotTagQuery = null, MKTagQuery _itemTagQuery = null) where T : MKItem
        {
            List<T> foundItems = new();
            foreach (MKSlot slot in IterateSlots())
            {
                if (_slotTagQuery != null
                    && !_slotTagQuery.Test(slot.slotTags))
                {
                    continue;
                }

                if (slot.item is T item)
                {
                    if (_itemTagQuery != null
                        && !_itemTagQuery.Test(item.tags))
                    {
                        continue;
                    }

                    foundItems.Add(item);
                }
            }

            return foundItems;
        }

        public IEnumerable<MKSlot> IterateSlots()
        {
            return slots;
        }

        protected virtual void OnItemAdded(MKItem _item)
        {

        }

        protected virtual void OnItemRemoved(MKItem _item)
        {

        }
    }
}// Minikit.Inventory namespace
