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
    public float lookXLimit = 0f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float staminaRegenRate = 10f;         // Stamina regenerée par seconde
    public float staminaRegenDelay = 2f;         // Délai avant la regen si stamina = 0
    public float staminaUseRate = 1f;            // Stamina consommée par frame

    private float staminaRegenTimer = 0f;        // Timer pour le cooldown
    private bool isRecovering = true;            // Flag pour bloquer la regen

    CharacterController characterController;

    public GameObject deathText;                // Le message 'You DIED'

    public Light spotlight; // Référence au spotlight

    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Stamina = MaxStamina;
        deathText.SetActive(false);
        animator = GetComponent<Animator>();
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

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        float speed = Mathf.Abs(moveX) + Mathf.Abs(moveY);
        print("Speed is "+ speed);

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        animator.SetFloat("Speed", speed);
        if (isRunning && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            Stamina -= staminaUseRate;
            if (Stamina < 0) Stamina = 0;
            staminaRegenTimer = 0f; // reset le timer
            isRecovering = false;  // désactive la regen
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
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Vérifier si la touche F est pressée
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Alterner l'état du spotlight
            spotlight.enabled = !spotlight.enabled;
        }
        #endregion
    }

    // If the enemy uses triggers, use this method instead of OnCollisionEnter
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("COLLISION DETECTED");
            // Optional: Destroy enemy or handle game-over logic
            deathText.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
         }
    }
}
