using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BattleAgent))]
public class BattleAgentVisual : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Animator animator;
    [SerializeField] private Image healthBar;
    
    // Accessors
    public Image HealthBar => healthBar;
    
    // References
    private BattleAgent battleAgent;
    
    // Animator Variables
    private static readonly int DamagedAnimParam = Animator.StringToHash("damaged");
    private static readonly int CastingSpellAnimParam = Animator.StringToHash("castingSpell");

    private void Awake()
    {
        battleAgent = GetComponent<BattleAgent>();
    }
    
    public void PlayDamagedAnimation()
    {
        animator.SetTrigger(DamagedAnimParam);
    }
    
    public void SetCastingSpellAnimation(bool isCasting)
    {
        animator.SetBool(CastingSpellAnimParam, isCasting);
    }
    
    public void UpdateHealthBar(float t)
    {
        healthBar.fillAmount = t;
    }
}
