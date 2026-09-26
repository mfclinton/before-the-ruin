using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    public Dialogue[] dialogues;
    public bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        
        if (other.CompareTag("Player"))
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager?.StartDialogue(dialogues);
            isTriggered = true;
            
            GameMemory.Instance.usedConversations.Add(name);
        }
    }
}
