using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameMemory : MonoBehaviour
{
    // Data
    public bool Skull_King_Defeated;
    public bool Centaur_Defeated;
    public bool Old_Man_Defeated;
    
    public List<String> usedConversations = new();
    
    // Singleton
    public static GameMemory Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handle Skull King
        GameObject[] skullKings = GameObject.FindGameObjectsWithTag("Skull King");
        foreach (GameObject skullKing in skullKings)
        {
            skullKing.SetActive(!Skull_King_Defeated);
        }
        
        // Handle Centaur
        GameObject[] centaurs = GameObject.FindGameObjectsWithTag("Centaur");
        foreach (GameObject centaur in centaurs)
        {
            centaur.SetActive(!Centaur_Defeated);
        }
        
        // Handle Old
        GameObject[] oldMen = GameObject.FindGameObjectsWithTag("Old Man");
        foreach (GameObject oldMan in oldMen)
        {
            oldMan.SetActive(!Old_Man_Defeated);
        }
        
        // Find all DialogueTriggerZones
        var dialogueTriggerZones = FindObjectsByType<DialogueTriggerZone>(FindObjectsSortMode.None);
        foreach (var dialogueTriggerZone in dialogueTriggerZones)
        {
            dialogueTriggerZone.isTriggered = usedConversations.Contains(dialogueTriggerZone.name);
        }
        
        // Find all DialogueTriggerInteractables
        var dialogueTriggerInteractables = FindObjectsByType<DialogueTriggerInteractable>(FindObjectsSortMode.None);
        foreach (var dialogueTriggerInteractable in dialogueTriggerInteractables)
        {
            dialogueTriggerInteractable.isTriggered = usedConversations.Contains(dialogueTriggerInteractable.name);
        }
    }
}
