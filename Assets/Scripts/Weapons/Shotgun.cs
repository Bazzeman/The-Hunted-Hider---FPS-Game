using UnityEngine;

public class Shotgun : AbstractWeapon
{
    public override int Damage => 100;
    public override float ShootCooldown => 2F;
    public override int MagazineCapacity => 2;
    public override float MagazineReloadTime => 8F;
    public override float ShootDistance => 8F;
    public override KeyCode Key => KeyCode.Alpha2;
}
