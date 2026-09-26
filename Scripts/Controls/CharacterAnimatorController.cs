using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    
    [SerializeField] private bool invertSpriteX;

    // Animation Params
    private static readonly int IsMovingAnimParam = Animator.StringToHash("isMoving");
    
    // References
    private CharacterController2D characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
    }

    private void Update() => UpdateAnimator();

    private void UpdateAnimator()
    {
        Vector2 movement = characterController.MoveInput;
        
        // Flips Sprite
        if (movement.x > 0) spriteRenderer.flipX = invertSpriteX;
        else if (movement.x < 0) spriteRenderer.flipX = !invertSpriteX;
        
        // Updates Animator
        bool isMoving = movement != Vector2.zero;
        animator.SetBool(IsMovingAnimParam, isMoving);
    }
}