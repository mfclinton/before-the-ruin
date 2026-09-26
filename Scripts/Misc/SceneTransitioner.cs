using EasyTransition;
using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float transitionTime = 1f;
    
    bool isTransitioning = false;
    
    public void LoadScene()
    {
        if (isTransitioning)
            return;
        
        TransitionManager.Instance().Transition(sceneName, transitionSettings, transitionTime);
        isTransitioning = true;
    }
}
