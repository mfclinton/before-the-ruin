public interface IInteractable
{
    public void Interact();
    public bool CanInteract();
    public void OnInteractEnter();
    public void OnInteractExit();
    public string GetInteractableMessage(string key);
}