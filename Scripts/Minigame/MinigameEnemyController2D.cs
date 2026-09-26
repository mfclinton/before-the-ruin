using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using FMODUnity;

public class MinigameEnemyController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private BoxCollider2D movementBounds;
    [SerializeField] private Vector2 movementSpeedRange = new Vector2(1f, 3f);

    [Header("Shooting Settings")]
    [SerializeField] private Transform shootingParent;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject[] enemyBulletPrefabs;
    [SerializeField] private GameObject[] friendlyBulletPrefabs;
    [SerializeField] private Vector2 bulletSpawnTimeRange = new Vector2(.1f, 2f);
    [SerializeField] private Vector2 bulletSpeedRange = new Vector2(2f, 5f);
    [SerializeField] private StudioEventEmitter shootSound;
    
    // Internal Variables
    private Animator animator;
    private bool isEnemy;
    
    // Animation Params
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    
    // Internal Variables
    private Vector2 targetPosition;
    private float movementSpeed;
    private float nextBulletSpawnTime;
    
    private Bounds bounds;
    
    void Awake()
    {
        // Get Bounds
        bounds = movementBounds.bounds;
        
        // Get References
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SetSpeed();
        SetTarget();
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetSpeed();
            SetTarget();    
        }
        MoveCharacter();
        
        if (Time.time > nextBulletSpawnTime)
            Shoot();
    }
    
    private void SetSpeed()
    {
        movementSpeed = Random.Range(movementSpeedRange.x, movementSpeedRange.y);
    }

    private void SetTarget()
    {
        Vector2 randomBoundsPosition = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        targetPosition = randomBoundsPosition;
    }

    private void MoveCharacter()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.fixedDeltaTime);
    }
    
    private void Shoot()
    {
        GameObject[] bulletPrefabs = isEnemy ? enemyBulletPrefabs : friendlyBulletPrefabs;
        GameObject enemyBulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];
        
        GameObject bullet = Instantiate(enemyBulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.transform.SetParent(shootingParent);
        
        MinigameBullet minigameBullet = bullet.GetComponent<MinigameBullet>();
        
        float bulletSpeed = Random.Range(bulletSpeedRange.x, bulletSpeedRange.y);
        
        minigameBullet.Movement = Vector2.left * bulletSpeed;
        
        nextBulletSpawnTime = Time.time + Random.Range(bulletSpawnTimeRange.x, bulletSpawnTimeRange.y);
        
        // Play Shoot Animation
        animator.SetTrigger(AttackTrigger);
        
        shootSound?.Play();
    }

    public void SetEnemyState(bool isEnemy)
    {
        this.isEnemy = isEnemy;
        animator.SetBool("isEnemy", isEnemy);
    }
}
