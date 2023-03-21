using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MovementSystem))]
public class AIMain : MonoBehaviour
{
    MovementSystem movementMaster;
    [SerializeField] Transform target;
    NavMeshPath path;
    Vector2 movement;
    float PicknextWaypointDistance = 4f;
    int currentNode = 0;
    [SerializeField] float calPathRate = 10f;
    [SerializeField] float dotProdFront, dotProdRear;
    private Color RandomPathColor;
    // Start is called before the first frame update
    void Start()
    {
        RandomPathColor = new Color(Random.value, Random.value, Random.value);
        path = new NavMeshPath();
        movementMaster = GetComponent<MovementSystem>();
        InvokeRepeating("CalculatePath", 0f, calPathRate); //คำนวณเส้นทางทุกๆ calPathRate
    }

    // Update is called once per frame
    void Update()
    {
        if(currentNode < path.corners.Length){ //เช็คว่าสุดเส้นทางยัง
            dotProdRear = Vector3.Dot(transform.right, path.corners[currentNode] - transform.position); //คิดค่าคำนวณการหัน
            dotProdFront = Vector3.Dot(transform.forward, path.corners[currentNode] - transform.position); //คิดค่าคำนวณการเดิน
            if(Vector3.Distance(transform.position,target.position) >= 10f){ //ระยะวิ่ง
                movementMaster.setSprint(1f);
            }else{
                movementMaster.setSprint(0f);
            }
            movementMaster.SetMovement(new Vector2(Mathf.Clamp(dotProdRear,-1f,1f),Mathf.Clamp(dotProdFront,-1f,1f))); //สั่งเดิน
            if (Vector3.Distance(transform.position, path.corners[currentNode]) < PicknextWaypointDistance)//pick next node
            {
                currentNode++;
            }
        }else{
            movement = Vector2.zero;
            movementMaster.SetMovement(movement);
        }
        for (int i = 0; i < path.corners.Length - 1; i++)//draw line
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], RandomPathColor);
        }
    }
    void CalculatePath()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas))
        {
            // object is on NavMesh
            if (target != null)
            {
                NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
            }
            /*else
            {
                NavMesh.CalculatePath(transform.position, RandomPositionOnNavmesh(), NavMesh.AllAreas, path);
            }*/
        }
        else
        {
            // object is not on NavMesh

            // Calculate the closest point on the NavMesh to the object's position
            NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas);
            Vector3 closestPoint = hit.position;

            // Calculate a new NavMesh path to the desired end point
            NavMesh.CalculatePath(closestPoint, target.position, NavMesh.AllAreas, path);
        }
        currentNode = 0;

    }
}
