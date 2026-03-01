using UnityEngine;
using System.Collections;

public class switchforcat : MonoBehaviour
{
    [Header("Settings")]
    public float delaySeconds = .2f;
    public AudioClip newClip;

    void Start()
    {
        StartCoroutine(DelayedScan());
    }

    IEnumerator DelayedScan()
    {
        yield return new WaitForSeconds(delaySeconds);
        ScanAndSwap();
    }

    void ScanAndSwap()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (!obj.name.StartsWith("CAT"))
                continue;

            AudioSource audio = gameObject.GetComponent<AudioSource>();
            if (audio == null || newClip == null)
                continue;

            audio.clip = newClip;
            audio.Play();

            break;
        }
    }
}