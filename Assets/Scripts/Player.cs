using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Slider))]
public class Player : MonoBehaviour
{
    public int health = 100;
    public int damage = 30;
    public int walkSpeed = 10;
    public int runSpeed = 15;
    public int cameraSensitivity = 85;
    public float jumpSpeed = 0.1F;
    public float gravity = -0.35F;
    public static Camera cam;
    public Transform playerHead;
    public GameObject healthbarGroup;
    public Gradient healthbarGradient;
    public Image healthbarFill;
    public GameObject shootIndicatorGroup;
    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI maxAmmoText;
    public TextMeshProUGUI cursor;
    public Color defaultCursorColor;
    public Color enemyInRangeCursorColor;
    public GameObject[] weapons;
    public GameObject defaultWeaponPrefab;
    public GameObject weaponHolderObject;

    private float horizontalSpeed, verticalSpeed, cameraRotationX;
    private CharacterController controller;
    private Slider healthBar;
    private static Slider shootIndicator;
    private static AbstractWeapon equipedWeapon;
    private static GameObject weaponHolder;
    private static TextMeshProUGUI currentAmmo;
    private static TextMeshProUGUI maxAmmo;
    private static AudioSource audioSource;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        audioSource = GetComponentInChildren<AudioSource>();
        controller = GetComponent<CharacterController>();
        weaponHolder = weaponHolderObject;
        maxAmmo = maxAmmoText;
        currentAmmo = currentAmmoText;

        healthBar = healthbarGroup.GetComponentInChildren<Slider>();
        shootIndicator = shootIndicatorGroup.GetComponentInChildren<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;
        healthbarFill.color = healthbarGradient.Evaluate(1);
        
        SwitchWeapon(defaultWeaponPrefab);
    }

    void Update()
    {
        if (GameManager.isGameRunning) // Only make the player do something if the game is run
        { 
            // Move the player to the direction it is looking at, at the set speed multiplied by deltatime.
            controller.Move(transform.right * Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime
                + transform.up * verticalSpeed
                + transform.forward * Input.GetAxis("Vertical") * horizontalSpeed * Time.deltaTime);

            // Make the player jump and switch between walking and running if it is on the ground. 
            if (controller.isGrounded)
            {
                horizontalSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
                verticalSpeed = Input.GetButtonDown("Jump") ? jumpSpeed : 0;
            }
            else
            {
                verticalSpeed += gravity * Time.deltaTime;
            }

            // Moves the head of the player on the X-axis based on the cursor's vertical location, which is limited by -90 and 90 decrees.
            cameraRotationX = Mathf.Clamp(cameraRotationX -= Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime, -90F, 90F);
            playerHead.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);

            // Moves the player on the Y-axis based on the cursor's horizontal position.
            transform.Rotate(cameraSensitivity * Input.GetAxis("Mouse X") * Time.deltaTime * Vector3.up);

            // Check if the player pressed a button which equals the required button to be pressed of a weapon in order to be switched to.
            foreach (GameObject weaponObject in weapons)
            {
                if (Input.GetKey(weaponObject.GetComponent<AbstractWeapon>().Key))
                {
                    SwitchWeapon(weaponObject);
                }
            }

            // Create ray to detect when the a enemy which the player is looking at is close enough in order for the gun to be able to shoot it.
            // If so change the cursor color and make the cursor thicker, if not change it back to the default color and default weight.
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, equipedWeapon.ShootDistance))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    cursor.fontStyle = FontStyles.Bold;
                    cursor.color = new Color(enemyInRangeCursorColor.r, enemyInRangeCursorColor.g, enemyInRangeCursorColor.b, 1F); // Make sure the alpha is not null, else the cursor will be 
                }
            }
            else
            {
                cursor.fontStyle = FontStyles.Normal;
                cursor.color = new Color(defaultCursorColor.r, defaultCursorColor.g, defaultCursorColor.b, 1F); // Make sure the alpha is not null, else the cursor will be 
            }
        }
    }

    /// <summary>
    /// Method to make the player switch to the provided weapon.
    /// </summary>
    /// <param name="weapon">The weapon to switch to</param>
    public static void SwitchWeapon(GameObject weapon)
    {
        if (weapon != equipedWeapon)
        {
            equipedWeapon = weapon.GetComponent<AbstractWeapon>();

            // Remove the currently held weapon (there can be multiple if switch buttons are spammed) which is being held, if there is one
            int weaponsToDestroy = weaponHolder.transform.childCount;
            for (int i = 0; i < weaponsToDestroy; i++)
            {
                Destroy(weaponHolder.transform.GetChild(i).gameObject);
            }

            equipedWeapon.shellsInMagazine = equipedWeapon.MagazineCapacity;
            maxAmmo.text = equipedWeapon.MagazineCapacity.ToString();
            currentAmmo.text = equipedWeapon.shellsInMagazine.ToString();
            UpdateShootIndicator(equipedWeapon.shellsInMagazine, equipedWeapon.MagazineCapacity);

            GameObject weaponObject = Instantiate(weapon, weaponHolder.transform); // Instantiate the weapon prefab onto the weapon holder
            weaponObject.transform.localPosition = Vector3.zero; // Make sure the new weapon is in the center of the weapon holder
        }
    }

    /// <summary>
    /// Play the passed audio clip on the player.
    /// </summary>
    /// <param name="audio">The audio clip to play</param>
    public static void PlaySound(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }

    /// <summary>
    /// Method to update the shooting indicator under the cursor using the time which has already been passed and the total time to wait.
    /// </summary>
    /// <param name="passedTime">The passed time</param>
    /// <param name="timeToWait">The total time</param>
    public static void UpdateShootIndicator(float passedTime, float timeToWait)
    {
        shootIndicator.maxValue = timeToWait;
        shootIndicator.value = Mathf.Clamp(passedTime, 0F, timeToWait);
    }
    
    /// <summary>
    /// Method that makes the player shoot at the direction it is looking at.
    /// </summary>
    public static void Shoot()
    {
        // Makes a ray if the left mouse button is klicked, which will damage the enemy which has the enemy tag and where the ray passes trough by the weapon damage.
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, equipedWeapon.ShootDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<AbstractEnemy>().TakeDamage(equipedWeapon.Damage);
            }
        }

        equipedWeapon.shellsInMagazine--;
        currentAmmo.text = equipedWeapon.shellsInMagazine.ToString();
    }

    /// <summary>
    /// Add a new bullet to the gun's magazine.
    /// </summary>
    public static void EnterBullet()
    {
        equipedWeapon.shellsInMagazine++;
        currentAmmo.text = equipedWeapon.shellsInMagazine.ToString();
    }

    /// <summary>
    /// Method to call when the player takes damage.
    /// </summary>
    /// <param name="damage">The integer damage amount</param>
    public void TakeDamage(int damage)
    {
        if ((health -= damage) <= 0)
        {
            GameManager.GameOver(false);
        }
        healthbarFill.color = healthbarGradient.Evaluate(healthBar.normalizedValue); // Set the color of the healthbar based on the healthbar's slider's normalized value.
        healthBar.value = health;
    }
}
