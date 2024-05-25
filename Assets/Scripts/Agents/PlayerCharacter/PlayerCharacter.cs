using System;
using StateManagement.StateClasses;
using Unity.VisualScripting;
using UnityEngine;

namespace Agents.PlayerCharacter {
    public class PlayerCharacter : MonoBehaviour {
        private PlayerState playerState;

        private void Awake() {
            playerState = GetComponent<PlayerState>();
        }

        // Called whenever the player's collider intersects with another
        public void OnTriggerEnter(Collider other) {
            Item item = other.GetComponent<Item>();
            // if an item component was found
            if (item) {
                // add one of that item to the players inventory
                playerState.playerInventory.AddItemToInventory(item.item, 1);
                Destroy(other.GameObject());
            }
        }

        private void OnApplicationQuit() {
            playerState.playerInventory.inventory.Clear();
        }
    }
}