using UnityEngine;
using FMODUnity;

public class DialogueTriggerInteractable : MonoBehaviour, IInteractable
{
    public Dialogue[] dialogues;
    
    [Header("Interactable Settings")]
    [SerializeField] private string interactableMessage = "Press {0}";
    [SerializeField] private StudioEventEmitter dialogueSound;
    
    public bool isTriggered = false;
    
    public void Interact()
    {
        if (CanInteract())
        {
            if (dialogueSound != null)
                dialogueSound.Play();
            
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager?.StartDialogue(dialogues);
            isTriggered = true;
            
            GameMemory.Instance.usedConversations.Add(name);
        }
    }

    public bool CanInteract()
    {
        return !isTriggered;
    }

    public void OnInteractEnter()
    {
        
    }

    public void OnInteractExit()
    {
        
    }

    public string GetInteractableMessage(string key)
    {
        return string.Format(interactableMessage, key);
    }
}