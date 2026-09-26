using System;
using System.Collections;
using EasyTransition;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Battle Agent Visuals References")]
    [SerializeField] private BattleAgentVisual playerVisual;
    [SerializeField] private BattleAgentVisual enemyVisual;
    
    [Header("UI References")]
    [SerializeField] private CanvasGroup playerActionSelectionUI;
    
    [Header("MiniGame References")]
    [SerializeField] private Transform minigameParent;
    
    [Header("Time Settings")]
    [SerializeField] private float playerActionSelectionUIFadeTime = 1.0f;
    [SerializeField] private float minigameDuration = 20.0f;
    [SerializeField] private float damageProcessingTime = 1.0f;
    
    [Header("Transition Settings")]
    [SerializeField] private TransitionSettings TransitionSettings;
    [SerializeField] private float transitionDuration = 1.0f;

    // Internal State
    private BattleTurnPhase currentPhase;
    
    // Internal Variables
    private Coroutine battleStateMachine;
    private Coroutine playerActionSelectionUIFadeCoroutine;
    
    private MinigameCharacterController2D minigameCharacterController;

    #region Setup

    private void Awake()
    {
        // Get References
        minigameCharacterController = minigameParent.GetComponentInChildren<MinigameCharacterController2D>();
        
        // Set Initial State
        currentPhase = BattleTurnPhase.PLAYER_TURN_START;
        playerActionSelectionUI.alpha = 0.0f;   
    }

    private void Start()
    {
        battleStateMachine = StartCoroutine(BattleStateMachine());
    }

    private IEnumerator BattleStateMachine()
    {
        while (currentPhase != BattleTurnPhase.BATTLE_OVER_LOSE && currentPhase != BattleTurnPhase.BATTLE_OVER_WIN)
        {
            switch (currentPhase)
            {
                case BattleTurnPhase.PLAYER_TURN_START:
                    yield return StartCoroutine(HandlePlayerTurnStart());
                    break;
                case BattleTurnPhase.PLAYER_ACTION_SELECTION:
                    yield return StartCoroutine(HandlePlayerActionSelection());
                    break;
                case BattleTurnPhase.PLAYER_ACTION_ATTACKING:
                    yield return StartCoroutine(HandlePlayerAttacking());
                    break;
                case BattleTurnPhase.PLAYER_ACTION_FLEEING:
                    yield return StartCoroutine(HandlePlayerFleeing());
                    break;
                case BattleTurnPhase.ENEMY_TURN:
                    yield return StartCoroutine(HandleEnemyTurn());
                    break;
                case BattleTurnPhase.PROCESSING_PLAYER_DAMAGE:
                    yield return StartCoroutine(HandleDamageProcessing());
                    break;
                case BattleTurnPhase.PROCESSING_ENEMY_DAMAGE:
                    yield return StartCoroutine(HandleDamageProcessing());
                    break;
            }
        }
        
        yield return StartCoroutine(HandleBattleOver());
    }

    #endregion

    #region Battle State Handlers

    private IEnumerator HandlePlayerTurnStart()
    {
        Debug.Log("Player Turn Started!");
        
        // Fade in UI
        Coroutine fadeUICoroutine = FadeUICoroutine(playerActionSelectionUI, playerActionSelectionUIFadeTime, false);
        yield return fadeUICoroutine;
        
        currentPhase = BattleTurnPhase.PLAYER_ACTION_SELECTION;
    }

    private IEnumerator HandlePlayerActionSelection()
    {
        Debug.Log("Player is selecting an action.");
        yield return new WaitForSeconds(1.0f); // TODO: Simulate player action selection
    }

    private IEnumerator HandlePlayerAttacking()
    {
        Debug.Log("Player is attacking!");
        
        // Fade out UI
        Coroutine fadeUICoroutine = FadeUICoroutine(playerActionSelectionUI, playerActionSelectionUIFadeTime, true);
        
        playerVisual.SetCastingSpellAnimation(true);
        minigameParent.gameObject.SetActive(true);
        minigameParent.GetComponentInChildren<MinigameEnemyController2D>().SetEnemyState(false);
        yield return new WaitForSeconds(minigameDuration);
        playerVisual.SetCastingSpellAnimation(false);
        minigameParent.gameObject.SetActive(false);
        
        currentPhase = BattleTurnPhase.PROCESSING_ENEMY_DAMAGE;
    }

    private IEnumerator HandlePlayerFleeing()
    {
        Debug.Log("Player is fleeing!");
        
        // Fade out UI
        Coroutine fadeUICoroutine = FadeUICoroutine(playerActionSelectionUI, playerActionSelectionUIFadeTime, true);
        yield return fadeUICoroutine;
        currentPhase = BattleTurnPhase.BATTLE_OVER_LOSE; // TODO:
    }

    private IEnumerator HandleEnemyTurn()
    {
        Debug.Log("Player is attacking!");
        
        // Fade out UI
        Coroutine fadeUICoroutine = FadeUICoroutine(playerActionSelectionUI, playerActionSelectionUIFadeTime, true);
        
        playerVisual.SetCastingSpellAnimation(true);
        minigameParent.gameObject.SetActive(true);
        minigameParent.GetComponentInChildren<MinigameEnemyController2D>().SetEnemyState(true);
        yield return new WaitForSeconds(minigameDuration);
        playerVisual.SetCastingSpellAnimation(false);
        minigameParent.gameObject.SetActive(false);
        
        currentPhase = BattleTurnPhase.PROCESSING_PLAYER_DAMAGE;
    }

    private IEnumerator HandleDamageProcessing()
    {
        Debug.Log("Processing damage...");
        
        BattleAgent playerAgent = playerVisual.GetComponent<BattleAgent>();
        BattleAgent enemyAgent = enemyVisual.GetComponent<BattleAgent>();
        
        // Process Player Health Change
        playerAgent.ModifyHealth(minigameCharacterController.NetHealth);
        
        if (minigameCharacterController.NetHealth < 0)
            playerVisual.PlayDamagedAnimation();
        
        float playerHealth = (float) playerAgent.CurrentHealth / playerAgent.MaxHealth;
        Coroutine playerHealthCoroutine = lerpHealthBarCoroutine(playerVisual, playerHealth, damageProcessingTime);
        
        // Process Enemy Health Change
        enemyAgent.ModifyHealth(minigameCharacterController.EnemyNetHealth);

        if (minigameCharacterController.EnemyNetHealth < 0)
            enemyVisual.PlayDamagedAnimation();
        
        float enemyHealth = (float) enemyAgent.CurrentHealth / enemyAgent.MaxHealth;
        Coroutine enemyHealthCoroutine = lerpHealthBarCoroutine(enemyVisual, enemyHealth, damageProcessingTime);
        
        // Wait for health bars
        yield return playerHealthCoroutine;
        yield return enemyHealthCoroutine;
        
        if (playerAgent.IsDead)
        {
            Debug.Log("Player is dead!");
            currentPhase = BattleTurnPhase.BATTLE_OVER_LOSE;
            yield break;
        }
        else if (enemyAgent.IsDead)
        {
            Debug.Log("Enemy is dead!");
            if (enemyVisual.CompareTag("Skull King"))
                GameMemory.Instance.Skull_King_Defeated = true;
            else if (enemyVisual.CompareTag("Centaur"))
                GameMemory.Instance.Centaur_Defeated = true;

            currentPhase = BattleTurnPhase.BATTLE_OVER_WIN;
            yield break;
        }
        else
        {
            if (currentPhase == BattleTurnPhase.PROCESSING_PLAYER_DAMAGE)
                currentPhase = BattleTurnPhase.PLAYER_TURN_START;
            else
                currentPhase = BattleTurnPhase.ENEMY_TURN;
        }
    }

    private IEnumerator HandleBattleOver()
    {
        Debug.Log("Battle Over!");

        string sceneName = "Scenes/Overworld_Scene";
        DoorEnum doorEnum = DoorEnum.NONE;
        if (currentPhase == BattleTurnPhase.BATTLE_OVER_WIN)
        {
            sceneName = "";
            if (enemyVisual.CompareTag("Skull King"))
                sceneName = "Scenes/Tower_Floor1_Scene";
            else if (enemyVisual.CompareTag("Centaur"))
                sceneName = "Scenes/Tower_Floor2_Scene";
            
            doorEnum = DoorEnum.DOOR_3;
        }
        
        SceneTransitionManager.Instance.Transition(sceneName, doorEnum, TransitionSettings, transitionDuration);
        StopCoroutine(battleStateMachine);
        yield break;
    }

    #endregion
    
    #region Helper Methods
    
    private Coroutine FadeUICoroutine(CanvasGroup canvasGroup, float duration, bool fadeOut = false)
    {
        if (playerActionSelectionUIFadeCoroutine != null)
            StopCoroutine(playerActionSelectionUIFadeCoroutine);
        
        playerActionSelectionUIFadeCoroutine = StartCoroutine(FadeUI(canvasGroup, duration, fadeOut));
        return playerActionSelectionUIFadeCoroutine;
    }
    
    private IEnumerator FadeUI(CanvasGroup canvasGroup, float duration, bool fadeOut = false)
    {
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = fadeOut ? 0.0f : 1.0f;
        
        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            
            float t = timeElapsed / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            
            yield return null;
        }
        
        canvasGroup.alpha = targetAlpha;
        playerActionSelectionUIFadeCoroutine = null;
    }
    
    private Coroutine lerpHealthBarCoroutine(BattleAgentVisual visual, float targetHealth, float duration)
    {
        return StartCoroutine(LerpHealthBar(visual, targetHealth, duration));
    }
    
    private IEnumerator LerpHealthBar(BattleAgentVisual visual, float targetHealth, float duration)
    {
        float startHealth = visual.HealthBar.fillAmount;
        
        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            
            float t = timeElapsed / duration;
            visual.UpdateHealthBar(Mathf.Lerp(startHealth, targetHealth, t));
            
            yield return null;
        }
        
        visual.UpdateHealthBar(targetHealth);
    }
    
    public void TriggerPlayerAttack()
    {
        if (currentPhase != BattleTurnPhase.PLAYER_ACTION_SELECTION)
            return;
        
        currentPhase = BattleTurnPhase.PLAYER_ACTION_ATTACKING;
    }
    
    public void TriggerPlayerFlee()
    {
        if (currentPhase != BattleTurnPhase.PLAYER_ACTION_SELECTION)
            return;
        
        currentPhase = BattleTurnPhase.PLAYER_ACTION_FLEEING;
    }
    
    #endregion
}
