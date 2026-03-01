using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceSubtractor : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference action;


    [Header("VFX")]
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private Vector3 offset;

    private ParticleSystem activeParticles;
    private ResourceTracker rt;

    public int resource = 2;

    private bool buttonHeld = false;

    private AudioSource ass;

    private void Start()
    {
        rt = GameObject.FindGameObjectWithTag("GameController").GetComponent<ResourceTracker>();
        ass = gameObject.GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        action.action.Enable();
        action.action.started += OnPressed;
        action.action.canceled += OnReleased;
    }

    void OnDisable()
    {
        action.action.started -= OnPressed;
        action.action.canceled -= OnReleased;
        action.action.Disable();
    }

    void StartParticles()
    {
        ass.Play();
        if (activeParticles == null)
        {
            activeParticles = Instantiate(
                particlePrefab,
                transform.position + offset,
                transform.rotation,
                transform
            );
        }

        activeParticles.Play();
    }

    void EndParticles()
    {
        if (activeParticles != null)
        {
            ass.Stop();
            activeParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void OnPressed(InputAction.CallbackContext ctx)
    {
        if (rt.DoIHave(resource))
        {
            buttonHeld = true;
            StartParticles();
        }
    }

    void OnReleased(InputAction.CallbackContext ctx)
    {
        buttonHeld = false;
        EndParticles();
    }

    private void Update()
    {
        if (buttonHeld)
        {
            rt.RemoveResource(resource);

            if (!rt.DoIHave(resource))
            {
                buttonHeld = false;
                EndParticles();
            }
        }
    }
}