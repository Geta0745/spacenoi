using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMain : MonoBehaviour
{
    MainMovementSystem movementMaster;
    [SerializeField] Transform target;
    private NavMeshPath path;
    [SerializeField] float shipSize = 1f;
    [HideInInspector] public List<Vector3> waypoints = new List<Vector3>();
    [SerializeField] private float waypointDistance = 5.0f;
    [SerializeField] float calPathRate = 0.2f;
    [SerializeField] float reachedDistance = 1f;
    [SerializeField] bool isAdjustWaypoint = false;
    [SerializeField] bool GizmosSize = false;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float PicknextWaypointDistance = 1f;
    Vector2 movementDirection;
    int currentNode;

    private void Start()
    {
        path = new NavMeshPath();
        InvokeRepeating("CalculatePath", 0f, calPathRate);
        movementMaster = GetComponent<MainMovementSystem>();
    }
    void Update()
    {
        if (currentNode < waypoints.Count && waypoints.Count > 0 && Vector3.Distance(transform.position, target.position) >= reachedDistance)
        {
            movementDirection.y = Mathf.Clamp(Vector3.Dot(transform.forward, waypoints[currentNode] - transform.position),-1f,1f);
            movementDirection.x = Mathf.Clamp(Vector3.Dot(transform.right, waypoints[currentNode] - transform.position),-1f,1f);
            movementMaster.SetMovementInput(movementDirection);
            if (Vector3.Distance(transform.position, waypoints[currentNode]) < PicknextWaypointDistance)//pick next node
            {
                currentNode++;
                Debug.DrawLine(transform.position, waypoints[currentNode]);
            }
        }
    }

    private void CalculatePath()
    {
        if (target == null || Vector3.Distance(transform.position, target.position) <= reachedDistance)
        {
            Debug.LogError("Target Null");
            return;
        }
        currentNode = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = target.position;

        NavMeshPath path = new NavMeshPath();
        NavMeshHit startHit;
        NavMeshHit endHit;

        // Find the nearest point on the NavMesh to the start and end positions
        if (NavMesh.SamplePosition(startPosition, out startHit, shipSize, NavMesh.AllAreas) &&
            NavMesh.SamplePosition(endPosition, out endHit, shipSize, NavMesh.AllAreas))
        {
            startPosition = startHit.position;
            endPosition = endHit.position;

            // Calculate the path using the adjusted start and end positions
            NavMesh.CalculatePath(startPosition, endPosition, NavMesh.AllAreas, path);

            waypoints.Clear();
            for (int i = 0; i < path.corners.Length; i++)
            {
                waypoints.Add(path.corners[i]);
            }

            if (isAdjustWaypoint)
            {
                AdjustWaypoints();
            }
        }
    }

    private bool CheckCollision(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, shipSize, obstacleMask);
        if (colliders.Length == 1)
        {
            //todo dont detect this gameobject
        }
        return colliders.Length > 0;
    }

    private void AdjustWaypoints()
    {
        if (waypoints.Count < 2)
            return;

        List<Vector3> adjustedWaypoints = new List<Vector3>();
        adjustedWaypoints.Add(waypoints[0]);

        for (int i = 1; i < waypoints.Count - 1; i++)
        {
            Vector3 previousWaypoint = waypoints[i - 1];
            Vector3 currentWaypoint = waypoints[i];
            Vector3 nextWaypoint = waypoints[i + 1];

            Vector3 direction = (nextWaypoint - previousWaypoint).normalized;
            float distance = Vector3.Distance(previousWaypoint, currentWaypoint);
            int numWaypoints = Mathf.CeilToInt(distance / waypointDistance);

            for (int j = 1; j <= numWaypoints; j++)
            {
                float waypointDistanceAlongPath = j * waypointDistance;
                Vector3 waypointPosition = previousWaypoint + direction * waypointDistanceAlongPath;

                if (CheckCollision(waypointPosition))
                {
                    // Handle obstacle avoidance
                    waypointPosition = AvoidObstacles(waypointPosition);
                }

                adjustedWaypoints.Add(waypointPosition);
            }
        }

        adjustedWaypoints.Add(waypoints[waypoints.Count - 1]);
        waypoints = adjustedWaypoints;
    }


    private Vector3 AvoidObstacles(Vector3 waypoint)
    {
        Collider[] colliders = Physics.OverlapSphere(waypoint, shipSize, obstacleMask);
        Vector3 averagePosition = Vector3.zero;
        NavMeshHit hit;
        foreach (Collider collider in colliders)
        {
            if (colliders.Length > 1)
            {
                averagePosition += collider.transform.position;
                averagePosition /= colliders.Length;

                Vector3 direction = (averagePosition - waypoint).normalized;
                float adjustedDistance = shipSize;
                Vector3 adjustedWaypoint = waypoint + direction * adjustedDistance;
                Debug.DrawLine(waypoint, adjustedWaypoint, Color.blue);
                return adjustedWaypoint;
            }
            else if (colliders.Length == 1)
            {
                Vector3 closestPoint = collider.ClosestPoint(waypoint);
                Vector3 normalDirection = (waypoint - closestPoint).normalized;
                float adjustedDistance = shipSize;
                Vector3 adjustedWaypoint = closestPoint + normalDirection * adjustedDistance;
                if (NavMesh.SamplePosition(adjustedWaypoint, out hit, Mathf.Infinity, NavMesh.AllAreas))
                {
                    Debug.DrawLine(waypoint, adjustedWaypoint, Color.green);
                    adjustedWaypoint = hit.position;
                    return adjustedWaypoint;
                }
                else
                {
                    Debug.DrawLine(waypoint, adjustedWaypoint, Color.blue);
                    return adjustedWaypoint;
                }

            }

        }
        return waypoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shipSize);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, reachedDistance);
        if (waypoints.Count > 0)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.color = Physics.OverlapSphere(waypoints[i], shipSize, obstacleMask).Length > 0 ? Color.red : Color.green;
                Gizmos.DrawSphere(waypoints[i], 0.1f);
                if (GizmosSize)
                {
                    Gizmos.DrawWireSphere(waypoints[i], shipSize);
                }

                if (i > 0)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
                }
            }
        }
    }

    public void InsertTarget(Transform target)
    {
        this.target = target;
    }
}
