using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public Character character;  // Reference to Character ScriptableObject
    [TextArea(3, 10)]
    public string dialogueText;   // Dialogue text
}