using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class PlaySoundOnHit : MonoBehaviour
{
    public AudioClip hitSound;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hitSound == null) return;

        // Optional: scale volume by impact strength
        float impact = collision.relativeVelocity.magnitude;
        audioSource.PlayOneShot(hitSound, Mathf.Clamp01(impact / 10f));
    }
}