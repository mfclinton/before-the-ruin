using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class InteractionDetector : MonoBehaviour
{
    // Internal Variables
    public List<IInteractable> interactablesInRange { get; private set; }
    
    // State
    public bool IsInteractDetectorEnabled { get; private set; }
    public bool CanInteract => IsInteractDetectorEnabled && interactablesInRange.Count > 0;

    private void Awake()
    {
        // Get References
        interactablesInRange = new List<IInteractable>();
        
        // Set Initial State
        IsInteractDetectorEnabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Add Interactable
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable.CanInteract())
        {
            interactablesInRange.Add(interactable);
            interactable.OnInteractEnter();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        // Remove Interactable
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesInRange.Remove(interactable);
            interactable.OnInteractExit();
        }
    }
    
    public void SetInteractDetectorEnabled(bool enabled)
    {
        IsInteractDetectorEnabled = enabled;
    }
}
