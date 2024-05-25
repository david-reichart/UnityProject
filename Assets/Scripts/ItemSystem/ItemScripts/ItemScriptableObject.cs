using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem {
    public enum ItemType {
        Consumable
    }
    public abstract class ItemScriptableObject : ScriptableObject {
        public GameObject itemPrefab;
        public Sprite itemSprite;
        public ItemType itemType;
        public string itemName;
        [TextArea(10, 10)] public string itemDescription;
    }
}