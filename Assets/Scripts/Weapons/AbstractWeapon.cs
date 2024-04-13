using System.Collections;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    public abstract int Damage { get; }
    public abstract float ShootCooldown { get; }
    public abstract float MagazineReloadTime { get; }
    public abstract float ShootDistance { get; }
    public abstract KeyCode Key { get; }
    public abstract int MagazineCapacity { get; }
    public AudioClip shootSound;
    public AudioClip reloadSound;

    public int shellsInMagazine;

    private bool isOnCooldown = false;

    private void Start()
    {
        shellsInMagazine = MagazineCapacity;
    }

    void Update()
    {
        if (GameManager.isGameRunning &&Input.GetMouseButtonDown(0) && !isOnCooldown)
        {
            // For some reason I could not just change the shellsInMagazine value within this abstract class,
            // so I also did it in the player class to make it work. Don't ask me why it doesn't work, because I have no clue myself haha
            shellsInMagazine--;

            Player.Shoot();
            Player.PlaySound(shootSound);
            StartCoroutine(GunShootCooldown(ShootCooldown));

            // Reload the gun with new bullets if the magazine is empty
            if (shellsInMagazine <= 0)
            {
                StartCoroutine(GunReloadCooldown(MagazineReloadTime));
            }
        }

        // Methods for creating a cooldown for shooting and reloading the gun
        IEnumerator GunShootCooldown(float seconds)
        {
            isOnCooldown = true;
            float startTime = Time.time; // Keep track of when the cooldown started

            while (Time.time - startTime < seconds)
            {
                // Show how much time is left in the shooting indicator if the weapon has any bullets in it's megazine
                Player.UpdateShootIndicator(shellsInMagazine <= 0 ? 0 : Time.time - startTime, seconds);
                yield return null;
            }

            isOnCooldown = shellsInMagazine <= 0;
        }

        IEnumerator GunReloadCooldown(float seconds)
        {
            isOnCooldown = true;

            while (shellsInMagazine < MagazineCapacity)
            {
                yield return new WaitForSeconds(seconds / MagazineCapacity);

                shellsInMagazine++;
                Player.EnterBullet();
                Player.PlaySound(reloadSound);
                Player.UpdateShootIndicator(shellsInMagazine, MagazineCapacity);
            }

            isOnCooldown = false;
        }
    }
}
