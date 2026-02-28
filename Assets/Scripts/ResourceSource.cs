using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    ResourceTracker rt;
    GameObject player_reference;

    public float Trigger_Distance = 4;
    public int ResourceToAdd = 2;

    public float AmountInContainer = 200;

    private float StartingAmountInContainer = 0;

    [SerializeField] GameObject resource_indicator;

    void Start()
    {
        rt = GameObject.FindGameObjectWithTag("GameController").GetComponent<ResourceTracker>();
        player_reference = GameObject.FindGameObjectWithTag("Player");

        StartingAmountInContainer = AmountInContainer;
    }

    // Update is called once per frame
    void Update()
    {

        if (AmountInContainer > 0 && rt.DoINeed(ResourceToAdd))
        {
            float distance = Vector3.Distance(player_reference.transform.position, transform.position);

            if (distance < Trigger_Distance)
            {
                rt.AddResource(ResourceToAdd);
                AmountInContainer -= Time.deltaTime;
            }
        }

        float quant = AmountInContainer / StartingAmountInContainer;
        resource_indicator.transform.localScale = new Vector3(quant, 1, 1);
    }
}
