using UnityEngine;
using UnityEngine.InputSystem;

public class VerticalLookHack : MonoBehaviour
{
    public InputActionReference lookAction;
    public float sensitivity = 120f;   // degrees per second
    public float minPitch = -80f;
    public float maxPitch = 80f;

    float pitch = 0f;

    void OnEnable()
    {
        lookAction.action.Enable();
    }

    void OnDisable()
    {
        lookAction.action.Disable();
    }

    void Update()
    {
        Vector2 look = lookAction.action.ReadValue<Vector2>();

        // Y controls up/down look
        pitch -= look.y * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}