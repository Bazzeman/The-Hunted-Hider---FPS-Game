public class Zombie : AbstractEnemy
{
    protected override int Health { get; set; } = 90;
    protected override int Damage => 15;
    protected override int AttackCooldown => 2;
    protected override float Speed => 3.5F;
    protected override float AttackDistance => 2.5F;
    protected override float NoticeDistance => 20F;
    protected override float MovementRadius => 40F;
    protected override float LocationRadius => 2F;
}
