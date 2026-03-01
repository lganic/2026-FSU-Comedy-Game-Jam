using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelSpinner : MonoBehaviour
{
    [Tooltip("Names of the different games")]
    public List<string> gameNames = new List<string>();
    [Tooltip("List of all the wheel category parent objects")]
    public List<Transform> gameParents = new List<Transform>();
    [Tooltip("The game wheel object itself")]
    public Transform gameWheel;
    [Tooltip("How fast the wheel will spin")]
    public float spinForce = 100f;
    [Tooltip("The variation in spin speed to produce different results")]
    public float spinVariation = 20f;
    [Tooltip("When to stop the wheel based on its spin speed")]
    public float stopSpeed = .5f;

    bool isSpinning = false;
    string chosenGame = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < gameParents.Count; i++)
        {
            WheelCategory[] categoryArray = gameParents[i].GetComponentsInChildren<WheelCategory>();
            foreach (WheelCategory category in categoryArray)
            {
                category.InitializeCategory(i, gameNames[i]);
            }
        }

        SpinWheel();
    }

    void SpinWheel()
    {
        Rigidbody wheelBody = gameWheel.GetComponent<Rigidbody>();
        float variation = Random.Range(-spinVariation, spinVariation);
        wheelBody.AddTorque(Vector3.up * (spinForce + variation) * Time.deltaTime, ForceMode.Impulse);
        isSpinning = true;
    }

    private void Update()
    {
        if (isSpinning && gameWheel.GetComponent<Rigidbody>().angularVelocity.y <= stopSpeed)
        {
            gameWheel.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            isSpinning = false;
            Debug.Log("Chosen game: " + chosenGame);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSpinning && other.GetComponent<WheelCategory>())
        {
            chosenGame = other.GetComponent<WheelCategory>().gameName;
        }
    }
}
