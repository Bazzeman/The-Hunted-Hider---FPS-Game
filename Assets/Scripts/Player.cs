using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    public int health = 100;
    public int damage = 30;
    public int walkSpeed = 10;
    public int runSpeed = 15;
    public int cameraSensitivity = 85;
    public float jumpSpeed = 0.1F;
    public float gravity = -0.35F;
    public Camera cam;
    public Transform playerHead;

    private float horizontalSpeed, verticalSpeed, cameraRotationX;
    private CharacterController controller;
    private RaycastHit hit;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Move the player to the direction it is looking at, at the set speed multiplied by deltatime.
        controller.Move(transform.right * Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime
            + transform.up * verticalSpeed
            + transform.forward * Input.GetAxis("Vertical") * horizontalSpeed * Time.deltaTime);

        // Make the player jump and switch between walking and running if it is on the ground. 
        /** Note: It's difficult to make the hitbox of the Character Controller perfect,
         * thus the Box Colliders of the player's body will attempt to fix the position.
         * Because the Character Controller hitbox is not perfectly alligned, the player will move up and
         * down rapidly by a very small amount of pixels, making controller.isGrounded not always return true!
         * Hopefully we can find a better way to resolve this problem in class. **/
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

        // Makes a ray if the left mouse button is klicked, which will destroy the game object the camera is looking at and if it has a enemy tag.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Destroy(hit.collider.gameObject.transform.parent.gameObject);
                }
            }
        }
    }

    public void Attack()
    {

    }
}
