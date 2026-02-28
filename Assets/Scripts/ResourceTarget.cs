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
    static readonly int ColorId = Shader.PropertyToID("_Color");

    int _colorPropId;
    Color _initialColor;

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

        var mat = _renderer.sharedMaterial;
        if (mat != null && mat.HasProperty(BaseColorId))
            _colorPropId = BaseColorId;
        else
            _colorPropId = ColorId;

        // Grab the starting color from the material (fallback to white if missing)
        if (mat != null && mat.HasProperty(_colorPropId))
            _initialColor = mat.GetColor(_colorPropId);
        else
        {
            _initialColor = Color.white;
            Debug.Log("Fallback");
        }
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

                if (TryGetComponent<AutoRedHighlight>(out AutoRedHighlight arh))
                {
                    arh.SetHighlightVisible(false);
                }

            }
        }

        float t = AmountOfResource / AmountRequired;
        t = Mathf.Clamp(t, 0, 1);

        // Overlay tint amount (white -> TargetColor)
        Color overlay = Color.Lerp(Color.white, TargetColor, t);

        // Combine with original material color
        Color combined = _initialColor * overlay; // multiply blend

        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(_colorPropId, combined);
        _renderer.SetPropertyBlock(_mpb);
    }
}
