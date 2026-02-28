using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceTarget : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    ResourceTracker rt;
    GameObject playerref;
    ScoreManager sm;

    public int TargetResource = 2; // 0 for all?

    public float ActivationDistance = 4;
    public float AmountRequired = 10;

    public float AmountOfResource = 0;

    public int Reward = 100;

    public InputActionReference PeeAction;
    public InputActionReference PoopAction;

    Renderer _renderer;
    MaterialPropertyBlock _mpb;

    public Color TargetColor;

    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    void Start()
    {
        rt = GameObject.FindGameObjectWithTag("GameController").GetComponent<ResourceTracker>();
        playerref = GameObject.FindGameObjectWithTag("Player");
        sm = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }

    void OnEnable() {
        PeeAction.action.Enable();
        PoopAction.action.Enable();
    }

    void OnDisable() {
        PeeAction.action.Disable();
        PoopAction.action.Disable();
    }
    
    bool isMyTargetPressed()
    {
        if (TargetResource == 0)
        {
            return PeeAction.action.IsPressed() || PoopAction.action.IsPressed();
        }
        if (TargetResource == 1)
        {
            return PoopAction.action.IsPressed();
        }
        if (TargetResource == 2)
        {
            return PeeAction.action.IsPressed();
        }
        return false;
    }

    void Update()
    {
        float dist = Vector3.Distance(playerref.transform.position, transform.position);       

        if (dist < ActivationDistance && isMyTargetPressed() && rt.DoIHave(TargetResource) && AmountOfResource < AmountRequired)
        {
            AmountOfResource += Time.deltaTime;

            if (AmountOfResource >= AmountRequired)
            {
                sm.AddToScore(Reward);
            }

            rt.RemoveResource(TargetResource);
        }

        float t = AmountOfResource / AmountRequired;
        t = Mathf.Clamp(t, 0, 1);

        Color c = Color.Lerp(Color.white, TargetColor, t);

        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(BaseColorId, c);
        _renderer.SetPropertyBlock(_mpb);
    }
}
