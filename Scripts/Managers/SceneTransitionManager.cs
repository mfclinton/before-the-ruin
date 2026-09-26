using System;
using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SceneTransitionManager : MonoBehaviour
{
    // Internal Variables
    private DoorEnum currentDoorEnum;
    
    // Singleton
    public static SceneTransitionManager Instance { get; private set; }
    
    private void Awake()
    {
        // Singleton
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
        SetPlayerToTransitionSpawn();
    }

    private void SetPlayerToTransitionSpawn()
    {
        if (currentDoorEnum == DoorEnum.NONE)
            return;
        
        // Find Scene Transition Interactable
        SceneTransitionInteractable[] sceneTransitionInteractables = FindObjectsOfType<SceneTransitionInteractable>(true);
        SceneTransitionInteractable sceneTransitionInteractable = sceneTransitionInteractables.FirstOrDefault(x => x.DoorEnum == currentDoorEnum);
        if (sceneTransitionInteractable == null)
            return;
        
        // Set Player Position
        CharacterController2D player = FindObjectOfType<CharacterController2D>();
        player.transform.position = sceneTransitionInteractable.SpawnPoint.position;
    }

    public void Transition(string sceneName, DoorEnum doorEnum, TransitionSettings transitionSettings, float duration)
    {
        Debug.Log($"Transitioning to {sceneName} through {doorEnum}");
        currentDoorEnum = doorEnum;
        TransitionManager.Instance().Transition(sceneName, transitionSettings, duration);
    }
}
