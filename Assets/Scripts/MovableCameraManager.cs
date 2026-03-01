using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MovableCameraManager : MonoBehaviour
{

    [Range(0.0f, 10.0f)]
    public float Transistion_Length = 1;

    [Range(1f, 200.0f)]
    public float Camera_Shake_Frequency = 100;

    [Range(.001f, .1f)]
    public float Camera_Shake_Intensity = .01f;


    public GameObject Object_To_Track;
    public Vector3 Track_Offset_Vector;
    public bool Relative_Track;

    private GameObject Temp_Return_Reference_Object;
    private Vector3 Temp_Return_Track_Offset_Vector;
    private bool Temp_Return_Relative_Track;
    private float Timer_Duration;
    private float Timer_Start;
    private bool Is_Currently_Temp_Focused;
    private bool Temp_Instant_Return;

    private float Transition_Timer_Start;
    private bool Is_Transition = false;

    Quaternion From_Rotation;
    Vector3 From_Position;

    private bool Is_Camera_Shaking;
    private float Camera_Shaking_Timer_Start;

    public void SetFocus(GameObject object_to_focus, Vector3 offset, bool relative = false, bool instant = false)
    {
        From_Rotation = transform.rotation;
        From_Position = transform.position;

        Object_To_Track = object_to_focus;
        Track_Offset_Vector = offset;
        Transition_Timer_Start = Time.time;
        Is_Transition = !instant;
        Relative_Track = relative;
    }

    public void SetTemporaryFocus(GameObject object_to_focus, Vector3 offset, bool relative = false, bool instant = false, bool instant_return = false)
    {
        Temp_Return_Reference_Object = Object_To_Track; // Keep a reference of what we were focused on before.
        Temp_Return_Track_Offset_Vector = Track_Offset_Vector;
        Temp_Instant_Return = instant_return;
        Temp_Return_Relative_Track = Relative_Track;

        SetFocus(object_to_focus, offset, relative: relative, instant: instant);
    }

    public void UnsetTemporaryFocus()
    {
        SetFocus(Temp_Return_Reference_Object, Temp_Return_Track_Offset_Vector, relative: Temp_Return_Relative_Track, instant: Temp_Instant_Return);
    }

    public void FocusForTime(GameObject object_to_focus, Vector3 offset, float duration, bool relative = false, bool instant = false, bool instant_return = false)
    {
        if (Is_Currently_Temp_Focused)
        {
            return; // We already have a temporary focus. Do nothing. 
        }

        Is_Currently_Temp_Focused = true;
        Timer_Start = Time.time;
        Timer_Duration = duration;

        SetTemporaryFocus(object_to_focus, offset, relative: relative, instant: instant, instant_return: instant_return);
    }

    public void CameraShake() {
        // Shake the camera at its current settings

        Is_Camera_Shaking = true;
        Camera_Shaking_Timer_Start = Time.time;
    }

    private float CameraShakeAmplitudeAtT(float amplitude, float frequency, float time, float cutoff = .005f)
    {
        float cutoff_time = Mathf.Sqrt(amplitude / cutoff) - Mathf.PI / frequency;

        float offset_time = time + Mathf.PI / frequency;

        float waveform = amplitude * Mathf.Sin(frequency * offset_time) / Mathf.Pow(offset_time, 2);

        float clamp_value = cutoff_time - time;

        if (time > cutoff_time)
        {
            Is_Camera_Shaking = false;
        }

        waveform *= Mathf.Clamp(clamp_value, 0, 1);

        return waveform;
    }

    void Update()
    {

        if (Is_Currently_Temp_Focused)
        {
            if (Time.time - Timer_Start > Timer_Duration)
            {
                // The requested amount of focus time has elapsed. 
                Is_Currently_Temp_Focused = false;
                UnsetTemporaryFocus();
            }
        }

        Vector3 target_position;

        if (Relative_Track)
        {
            target_position = Object_To_Track.transform.TransformPoint(Track_Offset_Vector);
        }
        else
        {
            target_position = Object_To_Track.transform.position + Track_Offset_Vector;
        }

        Vector3 delta_vector = Object_To_Track.transform.position - target_position;

        RaycastHit hit;
        float maxDistance = Track_Offset_Vector.magnitude; // Maximum distance to check

        if (Physics.Raycast(Object_To_Track.transform.position, -delta_vector, out hit, maxDistance))
        {
            Debug.DrawRay(transform.position, delta_vector, Color.green);


            Vector3 offset_vector = (float)(hit.distance * .9) * delta_vector.normalized;

            if (offset_vector.magnitude < delta_vector.magnitude)
            {
                target_position = Object_To_Track.transform.position - offset_vector;
            }
        }

        Quaternion target_rotation = Quaternion.LookRotation(delta_vector, Vector3.up);

        float lerp_time = (Time.time - Transition_Timer_Start) / Transistion_Length;

        if (lerp_time > 1)
        {
            Is_Transition = false;
        }

        // Apply smoothing to that lerp, so derivatives are zero starting out. 
        lerp_time = lerp_time * lerp_time * (3 - 2 * lerp_time); // Derived using basic calculus.

        if (Is_Transition)
        {
            target_position = Vector3.Lerp(From_Position, target_position, lerp_time);
            target_rotation = Quaternion.Lerp(From_Rotation, target_rotation, lerp_time);
        }

        float shake_y_offset = 0;
        if (Is_Camera_Shaking)
        {
            shake_y_offset = CameraShakeAmplitudeAtT(Camera_Shake_Intensity, Camera_Shake_Frequency, Time.time - Camera_Shaking_Timer_Start);

        }

        transform.position = target_position + new Vector3(0, shake_y_offset, 0);
        transform.rotation = target_rotation;
    }
}
