using UnityEngine;
using UnityEngine.InputSystem;

public class PressEscapeToExit : MonoBehaviour
{
    [SerializeField] private InputActionReference exitAction;

    void OnEnable()
    {
        exitAction.action.Enable();
        exitAction.action.performed += OnExit;
    }

    void OnDisable()
    {
        exitAction.action.performed -= OnExit;
        exitAction.action.Disable();
    }

    private void OnExit(InputAction.CallbackContext ctx)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}