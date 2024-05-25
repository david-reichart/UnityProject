using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using StateManagement.StateClasses;
using UnityEngine;

namespace StateManagement {
    public class StateSavingHandler : MonoBehaviour {

        /// <summary>
        /// Attach to UI to allow for saving of game state.
        /// </summary>
        public void SaveAction() {
            StartCoroutine(SaveGame());
        }

        /// <summary>
        /// Attach to UI to allow for loading of game state.
        /// </summary>
        public void LoadAction() {
            LoadGame();
        }

        /// <summary>
        /// Loop through all objects in the scene that inherit the State class and save their states in JSON format.
        /// </summary>
        private IEnumerator SaveGame() {
            // All objects with states that are children of the main state class will be located and updated
            foreach (State state in GameObject.FindObjectsOfType<State>()) {
                // Save only permitted states
                if (state.ShouldSave()) {
                    yield return new WaitForEndOfFrame();
                    // Get the state of the current state object in JSON format
                    string stateJson = state.SaveState();

                    // Write the current object's JSON state to disk asynchronously to avoid interrupting gameplay
                    string filePath = Application.persistentDataPath + "/" + state.GetUniqueID() + ".json";
                    WriteFileAsync(filePath, stateJson);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        /// <summary>
        /// Asynchronous file writing operation for writing each state object's JSON data to its own file.
        /// </summary>
        /// <param name="filePath">The file path for the current state object.</param>
        /// <param name="stateJson">The string containing this state object's JSON formatted state data.</param>
        public async Task WriteFileAsync(string filePath, string stateJson) {
             using (StreamWriter outputFile = new StreamWriter(filePath)) {
                await outputFile.WriteAsync(stateJson); 
             }
        }

        /// <summary>
        /// Reads all saved state files on disk found using the GetUniqueID method on each state and loads each state.
        /// </summary>
        private void LoadGame() {
            // All objects with states that are children of the main state class will be located and updated
            foreach (State state in GameObject.FindObjectsOfType<State>()) {
                // Load only permitted states
                if (state.ShouldLoad()) {
                    // Based on the state's unique ID, we get its expected file path
                    string stateFileLocation = Application.persistentDataPath + "/" + state.GetUniqueID() + ".json";
                    Debug.Log(stateFileLocation);
                    
                    // Check to see if there is a file in the expected path and load it if we find one
                    if (File.Exists(stateFileLocation)) {
                        string stateJson = File.ReadAllText(stateFileLocation);
                        state.LoadState(stateJson);
                    }
                }
            }
        }
    }
}