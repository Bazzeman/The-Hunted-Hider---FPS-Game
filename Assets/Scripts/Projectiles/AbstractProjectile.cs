using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class AbstractProjectile : MonoBehaviour
{
    protected abstract float ThrowForce { get; }
    protected abstract float ThrowUpwardForce { get; }
    protected Rigidbody Body { get; set; }
    protected Transform Target { get; set; }

    private int damage = 9;

    void Start()
    {
        Body = GetComponent<Rigidbody>();
        Target = GameObject.FindWithTag("Player").transform;

        transform.LookAt(Target); // Make the rock look towards the target.
        Body.AddForce(transform.forward * ThrowForce + transform.up * ThrowUpwardForce, ForceMode.Impulse); // Apply force to the projectile so that it flies forward
    }

    // Damage the player when it is hit by the rock, and destroy the rock
    // when it hits any kind of collider except the enemy's collider which is throwing this projectile.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Define the damage that this projectile will deal when it hits the target.
    /// </summary>
    /// <param name="setDamage">The damage amount</param>
    public void SetDamage(int setDamage)
    {
        damage = setDamage;
    }
}
