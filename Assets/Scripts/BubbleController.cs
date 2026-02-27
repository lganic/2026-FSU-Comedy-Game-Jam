using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BubbleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Text bubbleText;

    public List<string> Dialogs = new List<string>();
    public InputActionReference EnterAction;

    void OnEnable()
    {
        EnterAction.action.Enable();
    }

    void OnDisable()
    {
        EnterAction.action.Disable();
    }

    void UpdateToNext()
    {
        if (Dialogs.Count == 0)
        {
            Destroy(gameObject);
            return;
        }


        // Get next string to display on the dialog.
        string next_string = Dialogs[0];

        // Pop the next string.
        Dialogs.RemoveAt(0);

        bubbleText.text = next_string;
    }

    void Start()
    {
        UpdateToNext();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 myPosition = transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(myPosition - cameraPosition, Vector3.up);

        transform.rotation = targetRotation;

        if (EnterAction.action.WasPressedThisFrame())
        {
            UpdateToNext();
        }
    }
}
