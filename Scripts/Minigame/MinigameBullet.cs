using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MinigameBullet : MonoBehaviour
{
    // Settings
    public int HealthModifier;
    public int EnemyHealthModifier;
    
    // Config
    public Vector2 Movement { get; set; }
    
    // Constants
    private const string BulletWallTag = "BulletWall";

    private void FixedUpdate()
    {
        transform.position += (Vector3) Movement * Time.fixedDeltaTime;
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Hit Player
        MinigameCharacterController2D player = other.GetComponent<MinigameCharacterController2D>();
        if (player != null)
        {
            player.ModifyHealth(HealthModifier, EnemyHealthModifier);
            Destroy(gameObject);
        }
        
        print(other.tag);
        
        // Hit BulletWall
        if (other.CompareTag(BulletWallTag))
            Destroy(gameObject);
    }
}
