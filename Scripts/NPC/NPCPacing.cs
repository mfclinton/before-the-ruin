using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class NPCPacing : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    
    [SerializeField] private float speed = 2f;
    [SerializeField] private AnimationCurve moveCurve;
    
    // Internal Variables
    public bool IsPacingEnabled { get; private set; }
    private Vector3 currentTarget;
    private bool isMovingToB;
    
    // Animation Params
    private static readonly int IsMovingAnimParam = Animator.StringToHash("isMoving");
    
    // References
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        // Get References
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Set initial target
        currentTarget = pointB.position;
        isMovingToB = true;
        
        // Set initial state
        SetPacingEnabled(true);
    }

    void FixedUpdate()
    {
        if (!IsPacingEnabled)
            return;

        // Calculate movement
        float step = speed * moveCurve.Evaluate(Time.time) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);

        // Check if reached target
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            isMovingToB = !isMovingToB;
            currentTarget = isMovingToB ? pointB.position : pointA.position;
        }

        spriteRenderer.flipX = isMovingToB;
    }

    public void SetPacingEnabled(bool enabled)
    {
        IsPacingEnabled = enabled;
        animator.SetBool(IsMovingAnimParam, enabled);
    }
}
