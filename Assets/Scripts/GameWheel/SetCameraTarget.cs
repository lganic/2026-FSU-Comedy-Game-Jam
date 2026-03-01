using UnityEngine;

public class SetCameraTarget : MonoBehaviour
{
    public void SetTarget(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
