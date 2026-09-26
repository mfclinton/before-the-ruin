using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MinigameCharacterController2D : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private StudioEventEmitter jumpSound;
    [SerializeField] private StudioEventEmitter gainDamageSound;
    
    // Internal Variables
    public bool IsGrounded => rb.linearVelocity.y == 0;
    public int NetHealth { get; private set; }
    public int EnemyNetHealth { get; private set; }
    
    // Events
    public Action<int> OnHealthModified;
    
    // References
    private Rigidbody2D rb;
    private InputSystem_Actions inputActions;
    
    private void Awake()
    {
        // Get References
        rb = GetComponent<Rigidbody2D>();
        inputActions = new InputSystem_Actions();
        
        // Set MiniGame Action Map
        inputActions.Minigame.Enable();
        
        // Setup Input Actions
        inputActions.Minigame.Jump.performed += Jump;
    }

    private void OnEnable()
    {
        inputActions.Minigame.Enable();
        NetHealth = 0;
        EnemyNetHealth = 0;
    }
    
    private void OnDisable()
    {
        inputActions.Minigame.Disable();
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!IsGrounded)
            return;
        
        rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        jumpSound?.Play();
    }
    
    public void ModifyHealth(int amount, int enemyAmount)
    {
        NetHealth += amount;
        EnemyNetHealth += enemyAmount;
        OnHealthModified?.Invoke(amount);
        
        if (enemyAmount < 0)
            gainDamageSound?.Play();
    }
}
