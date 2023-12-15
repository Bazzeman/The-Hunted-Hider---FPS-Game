using UnityEngine;

public class Sniper : AbstractWeapon
{
    public override int Damage => 40;
    public override float ShootCooldown => 3F;
    public override int MagazineCapacity => 3;
    public override float MagazineReloadTime => 12F;
    public override float ShootDistance => 50F;
    public override KeyCode Key => KeyCode.Alpha3;
}
