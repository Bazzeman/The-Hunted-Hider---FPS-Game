using UnityEngine;

public class Revolver : AbstractWeapon
{
    public override int Damage => 17;
    public override float ShootCooldown => 1F;
    public override int MagazineCapacity => 6;
    public override float MagazineReloadTime => 5F;
    public override float ShootDistance => 15F;
    public override KeyCode Key => KeyCode.Alpha1;
}
