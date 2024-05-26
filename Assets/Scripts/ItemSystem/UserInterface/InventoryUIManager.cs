using System;
using System.Collections;
using System.Collections.Generic;
using ItemSystem.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIManager : MonoBehaviour {
    [SerializeField] private InventoryScriptableObject playerInventory;
    [SerializeField] private int inventorySlotCount;
    [SerializeField] private int currentItemCount;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualElement uiInventoryContainer;

    private void Start() {
        this.currentItemCount = playerInventory.inventory.Count; // how many items the player currently has
        this.inventorySlotCount = playerInventory.inventorySize; // max number of items allowed

        this.uiDocument = GetComponent<UIDocument>();
        this.uiInventoryContainer = uiDocument.rootVisualElement.Q<VisualElement>("UIBackground");
        
        InitializeUI();
    }

    // Should be called once at instantiation of the player's inventory UI.
    private void InitializeUI() {
        // Initialize the required number of rows to display all potential items
        int rowCount = this.inventorySlotCount / 10;
        for (int i = 0; i < rowCount; i++) {
            VisualElement newRow = CreateInventoryRow(i);
            uiInventoryContainer.Add(newRow);
        }
    }

    // Checks to see if the player's inventory size increased. Adds new UI elements to reflect the larger inventory.
    private void CheckForUIUpdate() {
        int rowCount = this.inventorySlotCount / 10; // number of rows this UI should display

        for (int i = 0; i < rowCount; i++) {
            // does this row already exist in the UI?
            if (uiInventoryContainer.Q<VisualElement>("InventoryRow" + i) == null) {
                VisualElement newRow = CreateInventoryRow(i);
                uiInventoryContainer.Add(newRow);
            }
        }
    }

    // Creates and returns a new VisualElement representing a row of 10 inventory item UI containers.
    private VisualElement CreateInventoryRow(int rowNumber) {
        // instantiate a new uniquely named UI row
        VisualElement newRow = new VisualElement();
        newRow.AddToClassList("inventoryRow");
        newRow.name = "InventoryRow" + rowNumber;

        // add 10 uniquely named inventory slot visual elements to the new UI row
        for (int slotNumber = 0; slotNumber < 10; slotNumber++) {
            newRow.Add(CreateInventorySlot(slotNumber, rowNumber));
        }

        return newRow;
    }

    // Creates and returns a new VisualElement representing an inventory slot.
    private VisualElement CreateInventorySlot(int slotNumber, int rowNumber) {
        VisualElement newInventorySlot = new VisualElement();
        newInventorySlot.AddToClassList("inventorySlot");
        newInventorySlot.name = "ItemSlot" + rowNumber + "-" + slotNumber;
        return newInventorySlot;
    }
}