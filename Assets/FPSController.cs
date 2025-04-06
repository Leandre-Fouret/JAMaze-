using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 90f;  // Adjusted to allow full vertical rotation

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float staminaRegenRate = 10f;         // Stamina regenerates per second
    public float staminaRegenDelay = 2f;         // Delay before regen if stamina = 0
    public float staminaUseRate = 1f;            // Stamina used per frame

    private float staminaRegenTimer = 0f;        // Timer for cooldown
    private bool isRecovering = true;            // Flag to block regen

    CharacterController characterController;

    public GameObject deathText;                // 'You DIED' message

    public Light spotlight; // Reference to the spotlight

    private Animator animator;

    // Walking bobbing variables
    private float walkBobSpeed = 14f;
    private float walkBobAmount = 0.0075f;
    private float timer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Stamina = MaxStamina;
        deathText.SetActive(false);
    }

    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (Stamina == 0)
            isRunning = false;

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (isRunning && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            Stamina -= staminaUseRate;
            if (Stamina < 0) Stamina = 0;
            staminaRegenTimer = 0f;
            isRecovering = false;
        }
        else
        {
            if (Stamina < MaxStamina)
            {
                if (!isRecovering)
                {
                    staminaRegenTimer += Time.deltaTime;
                    if (staminaRegenTimer >= staminaRegenDelay)
                        isRecovering = true;
                }

                if (isRecovering)
                {
                    Stamina += staminaRegenRate * Time.deltaTime;
                    if (Stamina > MaxStamina) Stamina = MaxStamina;
                }
            }
        }

        StaminaBar.fillAmount = Stamina / MaxStamina;
        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handles Rotation
        // Handles movement first
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            // Vertical Rotation (Looking Up/Down)
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit); // Full vertical rotation without limitation
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Horizontal Rotation (Turning Left/Right)
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Spotlight toggle with 'F' key
        if (Input.GetKeyDown(KeyCode.F))
        {
            spotlight.enabled = !spotlight.enabled;
        }
        #endregion

        #region Walking Bobbing (Head Bouncing Effect)
        if (characterController.isGrounded && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            timer += Time.deltaTime * walkBobSpeed;
            float waveSlice = Mathf.Sin(timer);

            if (waveSlice != 0)
            {
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                                                                    playerCamera.transform.localPosition.y + waveSlice * walkBobAmount,
                                                                    playerCamera.transform.localPosition.z);
            }
        }
        else
        {
            timer = 0f; // Reset when not moving
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                                                                playerCamera.transform.localPosition.y,
                                                                playerCamera.transform.localPosition.z);
        }
        #endregion
    }

    // If the enemy uses triggers, use this method instead of OnCollisionEnter
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            deathText.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }
}
