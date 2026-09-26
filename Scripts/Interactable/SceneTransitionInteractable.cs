using System;
using EasyTransition;
using UnityEngine;
using FMODUnity;

public class SceneTransitionInteractable : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private string sceneName;
    [SerializeField] private DoorEnum doorEnum;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private StudioEventEmitter transitionSound;
    [SerializeField] private StudioEventEmitter enterSound;
    
    [Header("Transition Visual Settings")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float transitionDuration = 1f;
    
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    
    [Header("Interactable Settings")]
    [SerializeField] private string interactableMessage = "Press {0}";
    
    // Properties
    public DoorEnum DoorEnum => doorEnum;
    public Transform SpawnPoint => spawnPoint;
    
    // Constants
    private static readonly int IsInteractableAnimParam = Animator.StringToHash("isInteractable");

    private void Awake()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    public void Interact()
    {
        if (transitionSound != null)
            transitionSound.Play();
        SceneTransitionManager.Instance.Transition(sceneName, doorEnum, transitionSettings, transitionDuration);
    }

    public bool CanInteract()
    {
        return true;
    }
    
    public void OnInteractEnter()
    {
        UpdateAnimator(true);
        
        if (enterSound != null)
            enterSound.Play();
    }
    
    public void OnInteractExit()
    {
        UpdateAnimator(false);
    }
    
    private void UpdateAnimator(bool isInteractable)
    {
        if (animator == null)
            return;
        
        isInteractable = isInteractable && CanInteract();
        animator.SetBool(IsInteractableAnimParam, isInteractable);
    }
    
    public string GetInteractableMessage(string key)
    {
        return string.Format(interactableMessage, key);
    }
}