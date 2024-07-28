using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PatrolAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Slider healthBar; // Reference to the health bar Slider
    public Transform healthBarCanvas; // Reference to the canvas holding the health bar
    public Camera mainCamera; // Reference to the main camera, set this in the editor
    public float waitTimeMin = 2f; // Minimum time to wait when idling
    public float waitTimeMax = 5f; // Maximum time to wait when idling
    public int maxHits = 10; // Maximum number of hits before resetting
    public Transform[] waypoints; // Array of waypoints for patrolling

    private Animator animator; // Reference to the Animator component
    private int hitCount = 0; // Track the number of times hit
    private bool isWaiting = false;
    private int currentWaypointIndex = 0; // Track the current waypoint index
    private Vector3 initialPosition; // Store the initial position of the agent

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
        InitializeHealthBar(); // Initialize the health bar
        initialPosition = transform.position; // Store the initial position
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            ReachedWaypoint(agent.transform.position);
        }

        UpdateHealthBar();
    }

    public void ReachedWaypoint(Vector3 collisionPosition)
    {
        StartCoroutine(IdleAtPoint(collisionPosition));
    }

    IEnumerator IdleAtPoint(Vector3 collisionPosition)
    {
        isWaiting = true;
        agent.isStopped = true;
        agent.Warp(collisionPosition); // Stop the agent at the collision position
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true); // Set isIdle animation

        yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

        animator.SetBool("isIdle", false); // Reset isIdle animation
        agent.isStopped = false;
        isWaiting = false;

        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    IEnumerator HandleDeath()
    {
        StopAllCoroutines(); // Stop all coroutines, including patrolling
        agent.isStopped = true; // Stop the agent's movement
        animator.SetBool("isWalking", false);
        animator.Rebind(); // Reset animator state

        isWaiting = false; // Reset the isWaiting flag


        ResetAgentPosition();
        hitCount = 0; // Reset hit count
        UpdateHealthBar(); // Reset the health bar

        yield return new WaitForSeconds(1f); // Delay agent movement

        RespawnAgent();
    }

    void RespawnAgent()
    {
        agent.isStopped = false; // Ensure the agent is active again
        MoveToNextWaypoint(); // Resume patrolling
    }

    public void HitByProjectile()
    {
        hitCount++;
        if (hitCount >= maxHits)
        {
            StartCoroutine(HandleDeath());
        }
        else
        {
            UpdateHealthBar();
        }
    }

    void ResetAgentPosition()
    {
        agent.Warp(initialPosition); // Immediately set the agent's position to its initial position
    }

    void InitializeHealthBar()
    {
        healthBar.maxValue = maxHits;
        healthBar.minValue = 0;
        healthBar.value = maxHits; // Start with full health
    }

    void UpdateHealthBar()
    {
        healthBar.value = maxHits - hitCount;

        // Make the health bar face the camera
        Vector3 directionToCamera = mainCamera.transform.position - healthBarCanvas.position;
        directionToCamera.x = directionToCamera.z = 0; // Keep only the y-axis rotation
        healthBarCanvas.LookAt(mainCamera.transform.position - directionToCamera); // Look at a point aligned on the y-axis

        // Adjust the rotation by 180 degrees on the y-axis if needed
        healthBarCanvas.Rotate(0, 0, 0);
    }
}
