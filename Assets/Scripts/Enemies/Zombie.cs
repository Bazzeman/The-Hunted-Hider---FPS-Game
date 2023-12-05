public class Zombie : AbstractEnemy
{
    protected override int health { get { return 90; } }
    protected override int damage { get { return 15; } }
    protected override float speed { get { return 3.5F; } }
    protected override float attackDistance { get { return 0.8F; } }
    protected override float noticeDistance { get { return 10F; } }
    protected override float movementRadius { get { return 20F; } }
    protected override float locationRadius { get { return 2F; } }

    protected override void Attack()
    {
        target.GetComponent<Player>().health -= damage;
    }
}
