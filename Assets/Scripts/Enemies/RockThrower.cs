using UnityEngine;

public class RockThrower : AbstractEnemy
{
    protected override int Health { get; set; } = 75;
    protected override int Damage => 8;
    protected override int AttackCooldown => 2;
    protected override float Speed => 2F;
    protected override float AttackDistance => 20F;
    protected override float NoticeDistance => 25F;
    protected override float MovementRadius => 40F;
    protected override float LocationRadius => 2F;

    public GameObject projectile;

    protected override void Attack()
    {
        GameObject rock = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 1.75F, transform.position.z), Quaternion.identity);
        rock.GetComponent<AbstractProjectile>().SetDamage(Damage);
    }
}