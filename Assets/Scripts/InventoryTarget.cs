using UnityEngine;

public class InventoryTarget : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float ActivationDistance;

    private GameObject playerref;
    private InventoryManager im;
    private ScoreManager sm;

    public int ScorePerSock = 100;

    void Start()
    {
        playerref = GameObject.FindGameObjectWithTag("Player");

        GameObject manager = GameObject.FindGameObjectWithTag("GameController");

        im = manager.GetComponent<InventoryManager>();
        sm = manager.GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(playerref.transform.position, transform.position);

        int num_in_inventory = im.getNumInInvetory();

        if (distance < ActivationDistance && num_in_inventory > 0)
        {
            im.SetInventory(0);

            sm.AddToScore(ScorePerSock * num_in_inventory);
        }
    }
}
