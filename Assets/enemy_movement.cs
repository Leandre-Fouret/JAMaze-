using UnityEngine;
using UnityEngine.AI;

public class enemy_movement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator animator; // Reference to the Animator

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Ensure the baseOffset is adjusted (adjust value if needed)
        // navMeshAgent.baseOffset = 0.5f; // Adjust this to match the height of your character

        // Try to get the Animator component from the child object (monster_body)
        animator = GetComponent<Animator>(); // This will get the Animator from the child
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);

            // Check if the animator exists before trying to set parameters
            if (animator != null)
            {
                // Calculate the speed based on the NavMeshAgent velocity magnitude
                float speed = navMeshAgent.velocity.magnitude;

                // Only set speed if it's moving (optional: add a small threshold to prevent very small movements from triggering idle)
                if (speed > 0.1f)
                {
                    animator.SetFloat("Speed", speed); // Adjust the Animator's Speed parameter
                }
                else
                {
                    animator.SetFloat("Speed", 0); // Make sure to set to idle when not moving
                }
            }
        }
    }

    // Optional: stop enemy movement if it collides with the player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop the NavMeshAgent from moving
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath(); // Optional: Reset the path

            // Show death text or handle other logic as needed
            // deathText.SetActive(true); // Example of showing death text (if defined elsewhere)
        }
    }
}
