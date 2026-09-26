using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter dialogueSoundOpen;
    [SerializeField] private StudioEventEmitter dialogueSoundClose;
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image characterImage;
    public CanvasGroup dialogueCanvasGroup;

    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();

    private void Awake()
    {
        dialogueCanvasGroup.alpha = 0;
    }

    public void StartDialogue(Dialogue[] dialogues)
    {
        dialogueQueue.Clear();
        
        foreach (var dialogue in dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }

        dialogueCanvasGroup.alpha = 1;
        DisplayNextDialogue();
        
        if (dialogueSoundOpen != null)
            dialogueSoundOpen.Play();
    }

    public void DisplayNextDialogue()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        var dialogue = dialogueQueue.Dequeue();
        nameText.text = dialogue.character.characterName;
        dialogueText.text = dialogue.dialogueText;
        characterImage.sprite = dialogue.character.characterImage;
    }

    private void EndDialogue()
    {
        dialogueCanvasGroup.alpha = 0;
        Debug.Log("End of conversation");
        
        if (dialogueSoundClose != null)
            dialogueSoundClose.Play();
    }

    public void OnNextButtonPressed()
    {
        DisplayNextDialogue();
    }
}