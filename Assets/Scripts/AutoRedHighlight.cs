using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AutoRedHighlight : MonoBehaviour
{
    [Header("Highlight Look")]
    [Range(0f, 1f)] public float alpha = 0.145f;
    public Color color = Color.red;

    [Tooltip("How much bigger the highlight mesh is vs the original (local scale multiplier).")]
    public float inflateScale = 1.2f;

    [Tooltip("Push highlight slightly towards the camera to reduce z-fighting (works best with uniform surfaces).")]
    public float zOffset = 0.0005f;

    [Header("Lifecycle")]
    public bool createOnEnable = true;
    public bool destroyOnDisable = true;

    [Header("Pulse")]
    public bool pulse = true;
    public float pulseSpeed = 1f;          // cycles per second-ish
    public float pulseAmount = 0.2f;      // extra scale on top of inflateScale (e.g. 0.02 = +2%)

    Vector3 highlightBaseScale;

    GameObject highlightGO;
    Material highlightMat;

    void OnEnable()
    {
        if (createOnEnable) EnsureHighlight();
        SetHighlightVisible(true);
    }

    void OnDisable()
    {
        if (!highlightGO) return;

        if (destroyOnDisable)
        {
            DestroyHighlight();
        }
        else
        {
            SetHighlightVisible(false);
        }
    }

    void OnDestroy()
    {
        DestroyHighlight();
    }

    public void EnsureHighlight()
    {
        if (highlightGO) return;

        var mf = GetComponent<MeshFilter>();
        var mr = GetComponent<MeshRenderer>();

        // Child object that renders the highlight
        highlightGO = new GameObject("HighlightOverlay");
        highlightGO.transform.SetParent(transform, false);
        highlightGO.transform.localPosition = Vector3.zero;
        highlightGO.transform.localRotation = Quaternion.identity;
        highlightGO.transform.localScale = Vector3.one * inflateScale;

        highlightGO.transform.localScale = Vector3.one * inflateScale;
        highlightBaseScale = highlightGO.transform.localScale;

        // Copy mesh
        var childMF = highlightGO.AddComponent<MeshFilter>();
        childMF.sharedMesh = mf.sharedMesh;

        // Copy renderer settings where it matters (shadows / probes / etc.)
        var childMR = highlightGO.AddComponent<MeshRenderer>();
        childMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        childMR.receiveShadows = false;
        childMR.lightProbeUsage = mr.lightProbeUsage;
        childMR.reflectionProbeUsage = mr.reflectionProbeUsage;

        // Make / assign material
        highlightMat = BuildHighlightMaterial();
        childMR.sharedMaterial = highlightMat;

        // Try to render after the original
        childMR.sortingLayerID = mr.sortingLayerID;
        childMR.sortingOrder = mr.sortingOrder + 1;

        // Optional tiny offset to reduce z-fighting (camera-facing-ish hack)
        if (Mathf.Abs(zOffset) > 0f)
            highlightGO.transform.localPosition = new Vector3(0f, 0f, -zOffset);
    }

    public void SetHighlightVisible(bool on)
    {
        if (highlightGO) highlightGO.SetActive(on);
    }

    void DestroyHighlight()
    {
        if (highlightGO) Destroy(highlightGO);
        highlightGO = null;

        // We created it at runtime; clean it up
        if (highlightMat) Destroy(highlightMat);
        highlightMat = null;
    }

    Material BuildHighlightMaterial()
    {
        // Prefer URP Unlit for a clean overlay that respects alpha.
        Shader shader =
            Shader.Find("Universal Render Pipeline/Unlit") ??
            Shader.Find("Universal Render Pipeline/Lit") ??
            Shader.Find("Standard");

        var mat = new Material(shader);

        var c = color;
        c.a = alpha;

        // ---------- URP (Unlit or Lit) ----------
        if (mat.HasProperty("_BaseColor"))
        {
            mat.SetColor("_BaseColor", c);

            // Force transparent surface (URP uses these properties)
            if (mat.HasProperty("_Surface")) mat.SetFloat("_Surface", 1f);   // 0=Opaque, 1=Transparent
            if (mat.HasProperty("_Blend")) mat.SetFloat("_Blend", 0f);     // 0=Alpha, 1=Premultiply, etc.

            // Force blending + no depth write
            if (mat.HasProperty("_SrcBlend")) mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            if (mat.HasProperty("_DstBlend")) mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            if (mat.HasProperty("_ZWrite")) mat.SetFloat("_ZWrite", 0f);

            // Make sure it lands in transparent queue
            mat.SetOverrideTag("RenderType", "Transparent");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            // Keywords matter in URP for some versions/settings
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            // Optional: make it look less “shaded”
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0f);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", 0f);

            return mat;
        }

        // ---------- Built-in Standard ----------
        if (mat.HasProperty("_Color"))
        {
            mat.SetColor("_Color", c);

            // Standard "Transparent" setup
            mat.SetFloat("_Mode", 3f);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            return mat;
        }

        return mat;
    }

    void Update()
    {
        if (!pulse || !highlightGO) return;

        // 0..1
        float t = (Mathf.Sin(Time.time * pulseSpeed * Mathf.PI * 2f) + 1f) * 0.5f;

        // Smooth it a bit (optional)
        t = t * t * (3f - 2f * t);

        float s = 1f + pulseAmount * t;
        highlightGO.transform.localScale = highlightBaseScale * s;
    }
}