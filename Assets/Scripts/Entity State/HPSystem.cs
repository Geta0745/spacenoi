using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSystem : MonoBehaviour
{
    [SerializeField] EntityState templateState;
    [SerializeField] float currentHP;
    [SerializeField] float currentStamina;
    // Start is called before the first frame update
    void Start()
    {
        if(templateState != null){
            currentHP = templateState.maxHp;
        }else{
            Die();
        }
    }

    public void Heal(float heal){
        if(heal > 0){ //เช็ค heal ไม่ติดลบ ไม่งั้นมันจะกลายเป็นดาเมจ
            //ฮีลโดยค่าฮีลจะไม่ทะลุ max hp
            currentHP = Mathf.Clamp(currentHP + heal,0f,templateState.maxHp);
        }
    }
    public void TakeDamage(float damage){
        if(damage > 0f){//เช็ค damage ไม่ติดลบ ไม่งั้นจะกลายเป็นฮีล
            //take damage
            currentHP -= damage;
        }
        if(currentHP <= 0f){//ถ้า hp ต่ำกว่าหรือเท่ากับ 0 = ตาย
            Die();
        }
    }

    void Die(){
        Destroy(gameObject);
    }
}
