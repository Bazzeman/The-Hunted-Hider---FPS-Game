using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class AbstractEnemy : MonoBehaviour
{
    protected abstract int Health { get; set; }
    protected abstract int Damage { get; }
    protected abstract int AttackCooldown { get; } // the cooldown before the enemy can attack again in seconds.
    protected abstract float Speed { get; }
    protected abstract float AttackDistance { get; }
    protected abstract float NoticeDistance { get; } // The distance between the player and the enemy before the zombie targets the player.
    protected abstract float MovementRadius { get; } // The distance around (0, 0) where the enemy can move in.
    protected abstract float LocationRadius { get; } // The distance between the new location and the enemy before the enemy will go to a new location.

    protected GameObject Target { get; set; }
    protected NavMeshAgent NavAgent { get; set; }
    protected Vector3 NewLocation { get; set; }

    private bool canAttack = true;

    void Start()
    {
        Target = GameObject.FindWithTag("Player");
        NewLocation = new Vector3(Random.Range(-MovementRadius, MovementRadius), transform.position.y, Random.Range(-MovementRadius, MovementRadius));
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.speed = Speed;
    }

    void Update()
    {
        // Return if the attack cooldown has not ended yet.
        if (!canAttack)
        {
            return;
        }

        // Move towards the player if the player is within notice radius, if not
        // go to a new location if the old location was not reached yet, else go to the old location.
        NavAgent.SetDestination(Vector3.Distance(transform.position, Target.transform.position) <= NoticeDistance
            ? Target.transform.position
            : Vector3.Distance(transform.position, NewLocation) <= LocationRadius
                ? NewLocation = new Vector3(Random.Range(-MovementRadius, MovementRadius), transform.position.y, Random.Range(-MovementRadius, MovementRadius))
                : NewLocation);

        // Attack the target if it is within the attack distance of the enemy.
        if (Vector3.Distance(transform.position, Target.transform.position) <= AttackDistance)
        {
            Attack();
            StartCoroutine(DelayCooldown(AttackCooldown));


            IEnumerator DelayCooldown(int seconds)
            {
                canAttack = false;
                yield return new WaitForSeconds(seconds);
                canAttack = true;
            }
        }
    }

    /// <summary>
    /// Method that gets called when the target is within the attack distance of the enemy.
    /// By default this method will instantly damage the target.
    /// </summary>
    protected virtual void Attack()
    {
        Target.GetComponent<Player>().TakeDamage(Damage);
    }

    /// <summary>
    /// Method to call when a enemy takes damage.
    /// </summary>
    /// <param name="damage">The integer damage amount</param>
    public virtual void TakeDamage(int damage)
    {
        if ((Health -= damage) <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }
}
