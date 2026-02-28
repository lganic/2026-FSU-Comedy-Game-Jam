using UnityEngine;

public class ResourceTracker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float FoodQuant = 100;
    public float WaterQuant = 80;

    [Range(10f, 100.0f)]
    public float FoodRefillSpeed = 10;
    [Range(0.0f, 10.0f)]
    public float WaterRefillSpeed = 30;

    [SerializeField] public GameObject PeeSlider;

    public void AddResource(int type)
    {
        // Register that a selected source should be increased.
        if (type == 1)
        {
            FoodQuant += Time.deltaTime * FoodRefillSpeed;
        }

        else if (type == 2)
        {
            WaterQuant += Time.deltaTime * WaterRefillSpeed;
        }
        else
        {
            Debug.Log("Invalid resource type:" + type);
        }
    }

    private bool close_enough(float a, float b)
    {
        return Mathf.Abs(a - b) < .001;
    }

    public bool DoINeed(int type)
    {
        if (type == 1)
        {
            return !close_enough(FoodQuant, 100);
        }

        else if (type == 2)
        {
            return !close_enough(WaterQuant, 100);
        }
        else
        {
            Debug.Log("Invalid resource type:" + type);
            return false;
        }
    }

    void Start()
    {
             
    }

    // Update is called once per frame
    void Update()
    {
        WaterQuant = Mathf.Clamp(WaterQuant, 0, 100);
        FoodQuant = Mathf.Clamp(FoodQuant, 0, 100);

        float water_perc = WaterQuant / 100;

        PeeSlider.transform.localScale = new Vector3(water_perc, 1, 1);
    }
}
