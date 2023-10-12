using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public enum GridType
    {
        Empty,
        Obstacle,
        FarmLand,
        Building
    }

    public GridType type;
    public Vector3 position;

    public GridNode(GridType type, Vector3 position)
    {
        this.type = type;
        this.position = position;
        
    }

}
