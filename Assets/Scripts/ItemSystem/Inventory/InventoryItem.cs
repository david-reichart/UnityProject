using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem.Inventory {
    [Serializable]
    public class InventoryItem {
        public ItemScriptableObject itemInSlot;
        public int itemQuantity;

        public InventoryItem(ItemScriptableObject item, int quantity) {
            this.itemInSlot = item;
            this.itemQuantity = quantity;
        }

        public void AddQuantity(int quantityToAdd) {
            this.itemQuantity += quantityToAdd;
        }

        public void RemoveQuantity(int quantityToRemove) {
            this.itemQuantity -= quantityToRemove;
        }
    }
}