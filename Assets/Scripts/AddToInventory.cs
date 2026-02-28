using UnityEngine;

public class AddToInventory : MonoBehaviour
{
    private GameObject playerref;
    private InventoryManager im;

    public float ActivationDistance = 3;

    void Start()
    {
        playerref = GameObject.FindGameObjectWithTag("Player");
        im = GameObject.FindGameObjectWithTag("GameController").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(playerref.transform.position, transform.position);
        
        if (distance < ActivationDistance && im.CanAddToInventory())
        {
            im.Add1ToInventory();
            Destroy(gameObject);
        }
    }
}
