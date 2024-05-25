using System;
using System.Collections;
using ItemSystem.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StateManagement.StateClasses {
    /// <summary>
    /// This class stores the general state of the player character.
    /// </summary>
    public class PlayerState : State {
        [Serializable]
        public struct playerValues {
            public Vector3 playerPosition;
            public Vector3 playerRotation;
        }
        public playerValues currentPlayerValues = new playerValues();
        public InventoryScriptableObject playerInventory;
        
        public override string SaveState() {
            currentPlayerValues.playerPosition = this.transform.position;
            currentPlayerValues.playerRotation = this.transform.eulerAngles;

            string stateJson = JsonUtility.ToJson(currentPlayerValues);
            return stateJson;
        }

        public override void LoadState(string loadedJson) {
            currentPlayerValues = JsonUtility.FromJson<playerValues>(loadedJson);
            // player position
            this.transform.position = currentPlayerValues.playerPosition;
            this.transform.eulerAngles = currentPlayerValues.playerRotation;
        }

        public override bool ShouldSave() {
            return true;
        }

        public override bool ShouldLoad() {
            return true;
        }
    }
}