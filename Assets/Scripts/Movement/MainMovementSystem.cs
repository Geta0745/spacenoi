using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MainMovementSystem : MonoBehaviour
{
    // Header for template state
    [Header("Entity Stat")]
    [SerializeField] protected EntityState stat;
    [SerializeField] protected float currentSpeed = 0f;
    [SerializeField] protected float rotationSpeed = 10f;
    [SerializeField] protected float currentStemina = 0f;
    [SerializeField] protected Transform characterBody;
    [SerializeField,Range(0f,1f)] protected float sprintSensitivity = 0f;
    protected Vector2 movementInput;
    protected Rigidbody rb;
    protected virtual void Awake() {
        currentStemina = stat.maxStamina;
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        UpdateStat();
        Move();
    }

    protected virtual void UpdateStat(){
        if(sprintSensitivity != 0f && currentStemina > 0f && (movementInput.x != 0f || movementInput.y != 0f)){
            SprintCaseAction();
        }else{
            WalkCaseAction();
        }
    }

    protected virtual void SprintCaseAction(){
        currentSpeed = stat.normalSpeed * stat.sprintMultiply;
        currentStemina = Mathf.Clamp(currentStemina - stat.SSconsumeRate, 0f, stat.maxStamina);
    }

    protected virtual void WalkCaseAction(){
        currentSpeed = stat.normalSpeed;
    }

    protected virtual void Move()
    {
        if(movementInput.x != 0f || movementInput.y != 0f){
            float targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            characterBody.rotation = Quaternion.Lerp(characterBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move character
        Vector3 vertical = transform.forward * currentSpeed * movementInput.y * Time.deltaTime;
        Vector3 horizontal = transform.right * currentSpeed * movementInput.x * Time.deltaTime;
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
        this.sprintSensitivity = isSprint;
    }

    public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }
}
