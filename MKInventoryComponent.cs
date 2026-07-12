using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Minikit.Inventory
{
    /// <summary> Represents a collection of bags that can be held on a GameObject </summary>
    public class MKInventoryComponent : MonoBehaviour
    {
        /// <summary> Raised when any bag's contents change (placement, removal, or stack count) </summary>
        [HideInInspector] public UnityEvent OnContentsChanged = new();

        [SerializeReference] protected List<MKBag> bags = new();


        protected virtual void Awake()
        {
            foreach (MKBag bag in bags)
            {
                bag.OnChanged += BagChanged;
            }
        }
        
        
        public virtual bool LootItem(MKItem _item)
        {
            foreach (MKBag bag in bags)
            {
                switch (bag.TryLoot(_item))
                {
                    case MKLootResult.Placed:
                        OnItemAdded(_item);
                        return true;
                    case MKLootResult.Merged:
                        return true;
                }
            }

            return false;
        }

        public bool RemoveItem(MKItem _item)
        {
            foreach (MKBag bag in bags)
            {
                if (bag.Remove(_item))
                {
                    OnItemRemoved(_item);
                    return true;
                }
            }

            return false;
        }

        public int GetItemCount(MKItemDefinitionScriptableObject _definition)
        {
            int total = 0;
            foreach (MKSlot slot in IterateSlots())
            {
                if (slot.item != null
                    && slot.item.itemDefinition == _definition)
                {
                    total += slot.item.stackCount;
                }
            }

            return total;
        }

        public bool ConsumeItem(MKItemDefinitionScriptableObject _definition, int _count)
        {
            if (_count <= 0
                || GetItemCount(_definition) < _count)
            {
                return false;
            }

            List<MKItem> emptied = new();
            foreach (MKSlot slot in IterateSlots())
            {
                if (_count <= 0)
                {
                    break;
                }

                if (slot.item != null
                    && slot.item.itemDefinition == _definition)
                {
                    int available = slot.item.stackCount;
                    if (available > _count)
                    {
                        slot.item.SetStackCount(available - _count);
                        _count = 0;
                    }
                    else
                    {
                        _count -= available;
                        emptied.Add(slot.item);
                    }
                }
            }

            foreach (MKItem item in emptied)
            {
                RemoveItem(item);
            }

            OnContentsChanged.Invoke();

            return true;
        }

        public void AddBag(MKBag _bag)
        {
            if (!bags.Contains(_bag))
            {
                _bag.OnChanged += BagChanged;
                bags.Add(_bag);
            }
        }

        public IReadOnlyList<MKBag> GetBags()
        {
            return bags;
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
            foreach (MKBag bag in bags)
            {
                foreach (MKSlot slot in bag.IterateSlots())
                {
                    yield return slot;
                }
            }
        }

        protected virtual void OnItemAdded(MKItem _item)
        {

        }

        protected virtual void OnItemRemoved(MKItem _item)
        {

        }

        private void BagChanged()
        {
            OnContentsChanged.Invoke();
        }
    }
}// Minikit.Inventory namespace
