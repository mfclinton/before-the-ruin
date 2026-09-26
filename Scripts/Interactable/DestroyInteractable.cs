using UnityEngine;
using FMODUnity;

public class DestroyInteractable : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private GameObject toDestroy;
    [SerializeField] private StudioEventEmitter destroySound;
    
    [Header("Interactable Settings")]
    [SerializeField] private string interactableMessage = "Press {0}";
    
    public void Interact()
    {
        if (destroySound != null)
            destroySound.Play();
        Destroy(toDestroy);
    }

    public bool CanInteract()
    {
        return true;
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