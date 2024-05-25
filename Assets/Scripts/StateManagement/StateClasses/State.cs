using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateManagement.StateClasses {
    /// <summary>
    /// This class defines functionality structure for game state instances.
    /// States should be children of this class. Children inherit MonoBehavior.
    /// </summary>
    public class State : MonoBehaviour {
        /// <summary>
        /// This method converts the current given state into a JSON formatted string which can be saved to disk.
        /// </summary>
        public virtual string SaveState() {
            return null;
        }
        
        /// <summary>
        /// This method loads state data from an on disk JSON file.
        /// </summary>
        /// <param name="loadedJson">The string containing JSON formatted data to be loaded.</param>
        public virtual void LoadState(string loadedJson) {
        }
        
        /// <summary>
        /// This method is used to create cases to prevent the player from being able to save the given state given some conditions.
        /// </summary>
        /// <returns>A boolean true/false value for if saving should be allowed.</returns>
        public virtual bool ShouldSave() {
            return true;
        }

        /// <summary>
        /// This method is used to create cases to prevent the player from being able to load a saved state given some conditions.
        /// </summary>
        /// <returns>A boolean true/false value for if loading should be allowed.</returns>
        public virtual bool ShouldLoad() {
            return true;
        }

        public virtual string GetUniqueID() {
            string uniqueID = "Game_" + gameObject.name + "_" + (this.GetType());
            return uniqueID;
        }
    } 
}