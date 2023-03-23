using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require Rigidbody and EntityState components to be attached to the GameObject
[RequireComponent(typeof(Rigidbody), typeof(EntityState))]
public class MovementSystem : MonoBehaviour
{
    // Header for template state
    [Header("Template State")]
    public EntityState templateState;

    // Tooltip for movement variable
    [Tooltip("The movement vector for the character")]
    [SerializeField] Vector2 movement;

    // Reference to the character body transform
    [SerializeField] Transform characterBody;

    // Reference to the Rigidbody component
    Rigidbody rb;

    // Movement speed variables
    [SerializeField] float currentSpeed = 10f;
    [SerializeField] float rotationSpeed = 180f;

    // Current stamina and sprint variables
    public float currentStemina;
    [SerializeField] Animator anime;
    float isSprint = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize current stamina and get reference to Rigidbody component
        currentStemina = templateState.maxStamina;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        // Check if sprinting
        if (isSprint != 0f && currentStemina > 0f && (movement.x != 0f || movement.y != 0f))
        {
            // Increase speed and decrease stamina
            currentSpeed = templateState.normalSpeed * templateState.sprintMultiply;
            currentStemina = Mathf.Clamp(currentStemina - templateState.SSconsumeRate, 0f, templateState.maxStamina);
            anime.SetBool("run", true);
        }
        else
        {
            // Reset speed and animation
            currentSpeed = templateState.normalSpeed;
            anime.SetBool("run", false);
        }

        // Set animation speed
        anime.SetFloat("speed", currentSpeed);

        // Rotate character towards movement direction
        if (movement.x != 0 || movement.y != 0)
        {
            anime.SetBool("run", true);
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            characterBody.rotation = Quaternion.Lerp(characterBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move character
        Vector3 vertical = transform.forward * currentSpeed * movement.y * Time.deltaTime;
        Vector3 horizontal = transform.right * currentSpeed * movement.x * Time.deltaTime;
        rb.MovePosition(rb.position + (vertical + horizontal));
    }

    // Method to increase stamina
    public void IncreaseStemina(float stemina)
    {
        if (stemina > 0f)
        {
            currentStemina += stemina;
        }
    }

    // Method to decrease stamina
    public void DecreaseStemina(float stemina)
    {
        if (stemina > 0f)
        {
            currentStemina -= stemina;
        }
    }

    // Method to set sprinting status
    public void setSprint(float isSprint)
    {
        this.isSprint = isSprint;
    }

    // Method to set movement direction
    public void SetMovement(Vector2 movement)
    {
        this.movement = movement;
    }
}
