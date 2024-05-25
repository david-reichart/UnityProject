using UnityEngine;

namespace Agents.PlayerCharacter {
    /// <summary>
    /// This class allows the player GameObject to move. The player can only move on the x and z axis.
    /// </summary>
    public class PlayerCharacterMovementHandler : MonoBehaviour {
        private Rigidbody playerRigidbody;
        private PlayerCharacterInputActions playerInput;
        private float playerMovementSpeed = 3f;

        private void Awake() {
            playerInput = new PlayerCharacterInputActions();
            playerRigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable() {
            playerInput.Enable();
        }

        private void OnDisable() {
            playerInput.Disable();
        }

        private void Update() {
            OnMovement();
        }
        
        /// <summary>
        /// Executes methods to move the player character when movement input actions are performed.
        /// </summary>
        private void OnMovement() {
            TransformMovement();
        }

        /// <summary>
        /// Moves the player character using rigidbody transform in the input direction.
        /// </summary>
        /// <remarks>
        /// Transforms result in a snappier movement without momentum.
        /// Input keybindings are defined in PlayerCharacterInputActions.
        /// </remarks>
        private void TransformMovement() {
            // collecting input movement data
            Vector2 movementDirectionV2 = playerInput.Navigation.Movement.ReadValue<Vector2>();
            Vector3 movementDirection = new Vector3(
                movementDirectionV2.x,
                0f,
                movementDirectionV2.y);
            float distanceMoved = playerMovementSpeed * Time.deltaTime;
            
            // rotate towards movement direction
            transform.forward = Vector3.Slerp(
                transform.forward,
                movementDirection,
                Time.deltaTime * playerMovementSpeed);
            
            // move player
            playerRigidbody.transform.position += distanceMoved * movementDirection;
        }
        
        /* -----------------GHOST METHODS----------------- */
        
        /// <summary>
        /// Moves the player character using rigidbody velocity in the input direction.
        /// </summary>
        /// <remarks>
        /// The use of velocity introduces momentum. todo:momentum management if this method gets used
        /// Input keybindings are defined in PlayerCharacterInputActions.
        /// </remarks>
        private void VelocityMovement() {
            // collecting input movement data
            Vector2 movementDirectionV2 = playerInput.Navigation.Movement.ReadValue<Vector2>();
            Vector3 movementDirection = new Vector3(
                movementDirectionV2.x,
                0f,
                movementDirectionV2.y);
            // todo:moves VERY SLOW and needs higher movement speed number compared to transform movement
            float distanceMoved = (playerMovementSpeed * 5) * Time.deltaTime;
            
            // given NO movement input; stops forward momentum from velocity updates
            if (movementDirection == Vector3.zero) {
                playerRigidbody.velocity = Vector3.zero;
            }
            // given movement input
            playerRigidbody.velocity += distanceMoved * movementDirection;
            
            // rotate towards movement direction
            transform.forward = Vector3.Slerp(
                transform.forward,
                movementDirection,
                Time.deltaTime * playerMovementSpeed);
        }
    }
}