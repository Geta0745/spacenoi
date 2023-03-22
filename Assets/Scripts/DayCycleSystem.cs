using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleSystem : MonoBehaviour
{
    public float cycle = 600f; //ครบวันภายใน x วินาที เช่น ครบวันภายใน 10 * 60 = 600 วินาที
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the angle to rotate the Sun object
        float angleToRotate = (Time.deltaTime / cycle) * 360f;
        Debug.Log(angleToRotate);
        // Rotate the Sun object around the Y axis
        transform.Rotate(Vector3.left, angleToRotate);
    }
}
