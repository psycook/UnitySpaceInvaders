using UnityEngine;
using UnityEngine.InputSystem;

public class IntroManagerBehaviour : MonoBehaviour
{
    public InputAction StartAction;

    void OnEnable()
    {
        StartAction.Enable();
    }

    void OnDisable()
    {
        StartAction.Disable();
    }
    
    void Update()
    {
        if(StartAction.triggered)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}
