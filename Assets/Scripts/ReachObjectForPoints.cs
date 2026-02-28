using UnityEngine;

public class ReachObjectForPoints : MonoBehaviour
{
    private GameObject playerReference;
    private ScoreManager sm;

    public float ActivationDistance = 3;
    public int NumPoints = 50;

    void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        sm = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float player_distance = Vector3.Distance(transform.position, playerReference.transform.position);

        if (player_distance < ActivationDistance)
        {
            sm.AddToScore(NumPoints);
            Destroy(gameObject);
        }
    }
}
