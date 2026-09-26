using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionVisualizer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string interactKey = "E";
    
    [Header("References")]
    [SerializeField] private InteractionDetector interactionDetector;
    
    [Header("UI References")]
    [SerializeField] private CanvasGroup interactionCanvasGroup;
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Awake()
    {
        // Get References
        if (interactionDetector == null)
            interactionDetector = FindAnyObjectByType<InteractionDetector>();
        
        // Set Initial State
        interactionCanvasGroup.alpha = 0;
    }
    
    private void Update()
    {
        UpdateInteractionUI();
    }

    private void UpdateInteractionUI()
    {
        bool canInteract = interactionDetector.CanInteract;
        
        interactionCanvasGroup.alpha = canInteract ? 1 : 0;
        interactionText.text = canInteract ? interactionDetector.interactablesInRange[0].GetInteractableMessage(interactKey) : "";
    }
}
