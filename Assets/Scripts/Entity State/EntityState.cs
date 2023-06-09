using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute]
public class EntityState : ScriptableObject
{
    public float maxHp = 100f;
    public float maxStamina = 200f;
    public float normalSpeed = 10f;
    public float normalAttackDamage = 5f;
    public float standardStamina = 100f;
    public float sprintMultiply = 2f;//ตัวคูณการวิ่ง
    public float SSconsumeRate = 0.01f; //stamina sprint consume rate
}
