using System;
using System.Collections;
using System.Collections.Generic;
using Agents.PlayerCharacter;
using ItemSystem.Inventory;
using StateManagement.StateClasses;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIManager : MonoBehaviour {
    [SerializeField] private InventoryScriptableObject playerInventory;
    [SerializeField] private int inventorySlotCount;
    [SerializeField] private int currentItemCount;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualElement uiInventoryContainer;
    private PlayerState playerState;

    private void Start() {
        this.currentItemCount = playerInventory.inventory.Count; // how many items the player currently has
        this.inventorySlotCount = playerInventory.inventorySize; // max number of items allowed
        
        this.uiDocument = GetComponent<UIDocument>();
        this.uiInventoryContainer = uiDocument.rootVisualElement.Q<VisualElement>("UIBackground");

        this.playerState = FindObjectOfType<PlayerCharacter>().GetComponent<PlayerState>();
        
        InitializeUI();
    }

    private void Update() {
        if (playerInventory.inventory.Count > 0) {
            DisplayItem();
        }
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
        VisualElement quantityLabel = CreateSlotQuantityLabel();
        quantityLabel.name = "ItemSlotLabel" + rowNumber + "-" + slotNumber;
        newInventorySlot.Add(quantityLabel);
        return newInventorySlot;
    }

    private Label CreateSlotQuantityLabel() {
        Label label = new Label("0");
        label.AddToClassList("inventorySlotQuantityText");
        return label;
    }

    private void DisplayItem() {
        VisualElement slot = uiInventoryContainer.Q<VisualElement>("ItemSlot1-0");
        slot.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0)); // make background invis
        slot.style.backgroundImage = new StyleBackground(playerInventory.inventory[0].GetSprite());
        Label slotQuantity = slot.Q<Label>("ItemSlotLabel1-0");
        slotQuantity.text = playerInventory.inventory[0].itemQuantity.ToString();
    }
}