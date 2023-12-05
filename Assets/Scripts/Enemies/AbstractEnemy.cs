using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class AbstractEnemy : MonoBehaviour
{
    protected abstract int health { get; }
    protected abstract int damage { get; }
    protected abstract float speed { get; }
    protected abstract float attackDistance { get; }
    protected abstract float noticeDistance { get; } // The distance between the player and the zombie before the zombie targets the player.
    protected abstract float movementRadius { get; } // The distance around (0, 0) where the zombie can move in.
    protected abstract float locationRadius { get; } // The distance between the new location and the zombie before the zombie will go to a new location.
    protected GameObject target { get; set; }
    protected NavMeshAgent navAgent { get; set; }
    protected Vector3 newLocation { get; set; }

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = speed;
        newLocation = new Vector3(Random.Range(-movementRadius, movementRadius), transform.position.y, Random.Range(-movementRadius, movementRadius));
    }

    void Update()
    {
        // Move the  towards the player if the player is within notice radius, if not
        // go to a new location if the old location was not reached yet, else go to the old location.
        navAgent.SetDestination(Vector3.Distance(transform.position, target.transform.position) <= noticeDistance
            ? target.transform.position
            : Vector3.Distance(transform.position, newLocation) <= locationRadius
                ? newLocation = new Vector3(Random.Range(-movementRadius, movementRadius), transform.position.y, Random.Range(-movementRadius, movementRadius))
                : newLocation);

        // Attack the target if it is within the attack distance of the .
        if (Vector3.Distance(transform.position, target.transform.position) <= attackDistance)
        {
            Attack();
        }
    }

    protected abstract void Attack();
}
