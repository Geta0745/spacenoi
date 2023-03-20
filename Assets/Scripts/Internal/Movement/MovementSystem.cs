using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] Vector2 movement;
    Rigidbody rb;
    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 90f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveDir = transform.forward * speed * movement.y *Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0f, movement.x * rotationSpeed * Time.deltaTime, 0f);
        rb.MovePosition(rb.position + moveDir);
        rb.MoveRotation(rb.rotation * rotation);
    }

    public void SetMovement(Vector2 movement){
        this.movement = movement;
    }
}
