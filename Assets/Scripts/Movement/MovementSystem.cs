using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementSystem : MonoBehaviour
{
    [SerializeField] Vector2 movement;
    Rigidbody rb;
    [SerializeField] float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 vertical = transform.forward * speed * movement.y *Time.deltaTime;
        Vector3 horizontal = transform.right * speed * movement.x * Time.deltaTime;
        //Quaternion rotation = Quaternion.Euler(0f, movement.x * rotationSpeed * Time.deltaTime, 0f);
        rb.MovePosition(rb.position + (vertical+horizontal));
    }

    public void SetMovement(Vector2 movement){
        this.movement = movement;
    }
}
