using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Tooltip("Name of the chosen game")]
    public string chosenGame = "";

    bool isSpinning = false;
    float spinTime = 0;

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

        string message = DialogueManager.instance.SetText(0, DialogueManager.TextType.normal);
        SubtitleManager.instance.SetText("Host Doggo", message);
    }

    public void SpinWheel()
    {
        Rigidbody wheelBody = gameWheel.GetComponent<Rigidbody>();
        float variation = Random.Range(-spinVariation, spinVariation);
        wheelBody.AddTorque(Vector3.up * (spinForce + variation) * Time.deltaTime, ForceMode.Impulse);
        isSpinning = true;
    }

    private void Update()
    {
        if (isSpinning & spinTime < 2)
        {
            spinTime += Time.deltaTime;
        }
        if (isSpinning && gameWheel.GetComponent<Rigidbody>().angularVelocity.y <= stopSpeed && spinTime > 1)
        {
            gameWheel.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Debug.Log("Chosen game: " + chosenGame);
            isSpinning = false;

            DialogueManager.instance.SetText(0, DialogueManager.TextType.cutscene);
            string message = chosenGame + "!";
            SubtitleManager.instance.SetText("Host Doggo", message);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSpinning && other.GetComponent<WheelCategory>())
        {
            chosenGame = other.GetComponent<WheelCategory>().gameName;
        }
    }

    public void ChangeScene()
    {
        Debug.Log("switching");
        //int index = SceneManager.GetSceneByName(chosenGame).buildIndex;
        //LoadAsync(index);
    }

    IEnumerator LoadAsync(int levelName)
    {
        // Wait until scene is fully loaded before switching to it
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        while (!operation.isDone)
        {
            // If we want to have a loading bar, we can set its progress using this variable
            float progress = Mathf.Clamp01((float)operation.progress);

            // Code to increase loading bar value

            // To not overwhelm the code while in the while statement
            yield return null;
        }
    }
}
