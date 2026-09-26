using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("References")]
    [SerializeField] private InteractionDetector interactionDetector;

    [SerializeField] private StudioEventEmitter footstepSoundOutside;
    [SerializeField] private StudioEventEmitter footstepSoundInside;

    // Internal Variables
    public Vector2 MoveInput { get; private set; }
    
    public bool IsMovementEnabled { get; private set; }
    
    // References
    private Rigidbody2D rb;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        // Get References
        rb = GetComponent<Rigidbody2D>();
        inputActions = new InputSystem_Actions();
        
        // Set Initial State
        IsMovementEnabled = true;
        
        // Setup Input Actions
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInputCancel;

        inputActions.Player.Interact.performed += Interact;
    }

    #region Unity Callbacks

    private void OnEnable() => inputActions.Player.Enable();

    private void OnDisable() => inputActions.Player.Disable();

    private void FixedUpdate() => MoveCharacter();

    #endregion

    #region Input Callbacks

    private void OnMoveInput(InputAction.CallbackContext ctx)
    {
        if (!IsMovementEnabled)
            return;

        Vector2 movement = ctx.ReadValue<Vector2>();
        
        // Handle Constraints
        if ((rb.constraints & RigidbodyConstraints2D.FreezePositionY) != RigidbodyConstraints2D.None)
            movement.y = 0;
        
        if ((rb.constraints & RigidbodyConstraints2D.FreezePositionX) != RigidbodyConstraints2D.None)
            movement.x = 0;
        
        // Set Movement
        MoveInput = movement;

        bool isInside = (rb.constraints & RigidbodyConstraints2D.FreezePositionY) != RigidbodyConstraints2D.None;
        if (movement.magnitude > 0.1f)
        {
            if (isInside)
            {
                footstepSoundInside?.Play();
            }
            else
            {
                footstepSoundOutside?.Play();
            }
        }
        else
        {
            footstepSoundInside?.Stop();
            footstepSoundOutside?.Stop();
        }
    }

    private void OnMoveInputCancel(InputAction.CallbackContext ctx) => MoveInput = Vector2.zero;

    #endregion

    #region Movement

    private void MoveCharacter()
    {
        if (rb == null || !IsMovementEnabled)
            return;
        
        rb.linearVelocity = MoveInput * moveSpeed;
    }

    public void EnableMovement(bool enable) => IsMovementEnabled = enable;
    
    #endregion
    
    #region Interaction
    
    private void Interact(InputAction.CallbackContext callbackContext)
    {
        if (interactionDetector == null|| !interactionDetector.CanInteract)
            return;
        
        interactionDetector.interactablesInRange[0].Interact();
    }
    
    #endregion
}