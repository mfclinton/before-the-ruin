using System;
using UnityEngine;
using FMODUnity;

public class BattleAgent : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private StudioEventEmitter healSound;
    [SerializeField] private StudioEventEmitter damageSound;
    [SerializeField] private StudioEventEmitter deathSound;
    
    // Accessors
    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDead => CurrentHealth <= 0;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void ModifyHealth(int amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        
        if (amount > 0)
        {
            if (healSound != null)
                healSound.Play();
        }
        else
        {
            if (damageSound != null && !IsDead)
                damageSound.Play();
            else if (deathSound != null && IsDead)
                deathSound.Play();
        }
    }
}
