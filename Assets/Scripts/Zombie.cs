using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Zombie : MonoBehaviour, IEnemy
{
    public int health = 90;
    public int damage = 15;
    public float speed = 3.5F;
    public float attackDistance = 0.8F;
    public float noticeDistance = 10F; // The distance between the player and the zombie before the zombie targets the player.
    public float movementRadius = 20F; // The distance around (0, 0) where the zombie can move in.
    public float locationRadius = 2F; // The distance between the new location and the zombie before the zombie will go to a new location.
    public GameObject target;

    private NavMeshAgent navAgent;
    private Vector3 newLocation;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = speed;
        GameManager.enemies[GameManager.enemies.Length + 1] = this; // Add this IEnemy instance to the GameManeger's enemies array.
        newLocation = new Vector3(Random.Range(-movementRadius, movementRadius), transform.position.y, Random.Range(-movementRadius, movementRadius));
    }

    void Update()
    {
        // Move the enemy towards the player if the player is within notice radius, if not
        // go to a new location if the old location was not reached yet, else go to the old location.
        navAgent.SetDestination(Vector3.Distance(transform.position, target.transform.position) <= noticeDistance
            ? target.transform.position
            : Vector3.Distance(transform.position, newLocation) <= locationRadius
                ? newLocation = new Vector3(Random.Range(-movementRadius, movementRadius), transform.position.y, Random.Range(-movementRadius, movementRadius))
                : newLocation);

        // Attack the target if it is within the attack distance of the enemy.
        if (Vector3.Distance(transform.position, target.transform.position) <= attackDistance)
        {
            target.GetComponent<Player>().health -= damage;
            // TODO: Add Death
        }
    }

    public void Attack()
    {

    }
}
