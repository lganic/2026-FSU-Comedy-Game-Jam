using System.Collections.Generic;
using UnityEngine;

public class testing_script : MonoBehaviour
{
    MovableCameraManager cameraManager;
    BubbleManager bm;
    float last_send_time = 0;
    float last_shake_time = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MovableCameraManager>();
        bm = GameObject.FindGameObjectWithTag("BubbleManager").GetComponent<BubbleManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - last_send_time > 20)
        {
            List<string> texts = new List<string> {"Yoooo", "Wassup"};
            last_send_time = Time.time;
            cameraManager.FocusForTime(gameObject, new Vector3(.78f, 4.33f, -11), 4, relative: true);
            bm.AddBubble(gameObject, texts);
        }

        transform.Rotate(0, 6 * Time.deltaTime, 0);

    }
}
