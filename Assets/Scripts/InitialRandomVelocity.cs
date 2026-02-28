using UnityEngine;

public class InitialRandomVelocity : MonoBehaviour
{
    public float min = 3;
    public float max = 9;
    void Start()
    {
        Rigidbody rg = gameObject.GetComponent<Rigidbody>();

        // This method isn't perfect, and slightly biases towards the corners, but whatever we don't really need perfect accuracy, and I don't feel like implementing the proper solution

        float component_x = Random.Range(-1, 1);
        float component_y = Random.Range(-1, 1);
        float component_z = Random.Range(-1, 1);

        Vector3 velocity_vector = new Vector3(component_x, component_y, component_z);

        velocity_vector = velocity_vector.normalized;

        float finalMagnitude = Random.Range(min, max);

        velocity_vector *= finalMagnitude;

        rg.linearVelocity = velocity_vector;
    }
}
