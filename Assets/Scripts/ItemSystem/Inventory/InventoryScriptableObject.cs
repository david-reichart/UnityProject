using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using ItemSystem.Inventory;

namespace ItemSystem.Inventory {
    [CreateAssetMenu(fileName = "InventoryScriptableObject", menuName = "Item System Scriptable Objects/Inventory")]
    public class InventoryScriptableObject : ScriptableObject {
        [SerializeField] public List<InventoryItem> inventory = new List<InventoryItem>();

        public void AddItemToInventory(ItemScriptableObject itemToAdd, int amountToAdd) {
            bool hasItem = false;
            
            // Check to see if the itemToAdd is already in the inventory
            for (int i = 0; i < inventory.Count; i++) {
                if (inventory[i].itemInSlot == itemToAdd) {
                    inventory[i].AddQuantity(amountToAdd);
                    hasItem = true;
                    break;
                }
            }

            // Adding an item not previously found in the inventory
            if (!hasItem) {
                InventoryItem newItem = new InventoryItem(itemToAdd, amountToAdd);
                inventory.Add(newItem);
            }
        }
    }
}

