using UnityEngine;

public class SetFrameRate : MonoBehaviour
{
    public int targetFrameRate = 60;

    void Awake()
    {
        // VSync must be disabled for targetFrameRate to work
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}
