using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(EntityState))]
public class MovementSystem : MonoBehaviour
{
    [SerializeField] EntityState templateState;
    [SerializeField] Vector2 movement;
    [SerializeField] Transform characterBody;
    Rigidbody rb;
    [SerializeField] float currentSpeed = 10f;
    [SerializeField] float rotationSpeed = 180f;
    [SerializeField] float currentStemina;
    [SerializeField] Animator anime;
    float isSprint = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentStemina = templateState.maxStamina;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isSprint != 0f && currentStemina > 0f){
            currentSpeed = templateState.normalSpeed * templateState.sprintMultiply;
            currentStemina = Mathf.Clamp(currentStemina - templateState.SSconsumeRate,0f,templateState.maxStamina);
            anime.SetBool("run",true);
        }else{
            currentSpeed = templateState.normalSpeed;
            anime.SetBool("run",false);
        }
        anime.SetFloat("speed",currentSpeed);
        if (movement.x != 0 || movement.y != 0)
        {
            anime.SetBool("run",true);
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            characterBody.rotation = Quaternion.Lerp(characterBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        Vector3 vertical = transform.forward * currentSpeed * movement.y *Time.deltaTime;
        Vector3 horizontal = transform.right * currentSpeed * movement.x * Time.deltaTime;
        rb.MovePosition(rb.position + (vertical+horizontal));
    }

    public void IncreaseStemina(float stemina){
        if(stemina > 0f){
            currentStemina += stemina;
        }
    }
    public void DecreaseStemina(float stemina){
        if(stemina > 0f){
            currentStemina -= stemina;
        }
    }
    public void setSprint(float isSprint){
        this.isSprint = isSprint;
    }

    public void SetMovement(Vector2 movement){
        this.movement = movement;
    }
}
