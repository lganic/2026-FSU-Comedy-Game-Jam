using UnityEngine;
using UnityEngine.AI;

public class Fleeing : MonoBehaviour
{
    [Header("Flee Settings")]
    public Transform player;
    public float detectionRange = 10f;
    public float fleeDistance = 5f;

    [Header("Wander Settings")]
    public float wanderRadius = 10f;
    public float wanderWaitTime = 2f;
    private float wanderTimer;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderWaitTime;

        // Ensure the cat starts on the blue NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // STATE 1: FLEEING (Player is close)
        if (distance < detectionRange)
        {
            Vector3 runDirection = transform.position - player.position;
            Vector3 targetPoint = transform.position + (runDirection.normalized * fleeDistance);

            MoveToNavPoint(targetPoint, fleeDistance);
        }
        // STATE 2: WANDERING (Player is far away)
        else
        {
            wanderTimer += Time.deltaTime;

            // Check if it's time to move or if the cat has reached its current destination
            if (wanderTimer >= wanderWaitTime || (!agent.pathPending && agent.remainingDistance < 0.5f))
            {
                Vector3 newPos = RandomNavMeshLocation(wanderRadius);
                agent.SetDestination(newPos);
                wanderTimer = 0;
            }
        }
    }

    // Helper to find a valid spot on the floor
    public Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = transform.position;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void MoveToNavPoint(Vector3 target, float range)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, range, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}