using System;
using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

namespace ItemSystem {
    [CreateAssetMenu(fileName = "ConsumableScriptableObject", menuName = "Item System Scriptable Objects/Items/Consumable")]
    public class ConsumableScriptableObject : ItemScriptableObject {
        private void Awake() {
            this.itemType = ItemType.Consumable;
        }
    }
}

