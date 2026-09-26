using System;
using System.Collections;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(MinigameCharacterController2D))]
public class MinigameCharacterAnimatorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    
    [SerializeField] private StudioEventEmitter damageSound;
    [SerializeField] private StudioEventEmitter healSound;

    // Animation Params
    private static readonly int IsJumping = Animator.StringToHash("jumping");
    
    // Coroutine Variables
    private Coroutine healthModifiedCoroutine;
    
    // References
    private MinigameCharacterController2D characterController;
    
    private void Awake()
    {
        characterController = GetComponent<MinigameCharacterController2D>();
    }

    private void OnEnable()
    {
        characterController.OnHealthModified += OnHealthModified;
    }
    
    private void OnDisable()
    {
        characterController.OnHealthModified -= OnHealthModified;
        
        // Reset Sprite Color
        spriteRenderer.color = Color.white;
    }

    private void Update() => UpdateAnimator();
    
    private void UpdateAnimator()
    {
        bool isJumping = !characterController.IsGrounded;
        animator.SetBool(IsJumping, isJumping);
    }
    
    private void OnHealthModified(int amount)
    {
        if (amount == 0)
            return;
        
        bool isHealing = amount > 0;
        StartHealthModifiedCoroutine(isHealing);
        
        if (isHealing)
            healSound?.Play();
        else
            damageSound?.Play();
    }
    
    private Coroutine StartHealthModifiedCoroutine(bool isHealing)
    {
        if (healthModifiedCoroutine != null)
        {
            spriteRenderer.color = Color.white;
            StopCoroutine(healthModifiedCoroutine);
        }
        
        healthModifiedCoroutine = StartCoroutine(HealthModifiedCoroutine(isHealing));
        return healthModifiedCoroutine;
    }
    
    private IEnumerator HealthModifiedCoroutine(bool isHealing)
    {
        Color color = isHealing ? Color.green : Color.red;
        spriteRenderer.color = color;
        
        yield return new WaitForSeconds(0.5f);
        
        spriteRenderer.color = Color.white;
    }
}
