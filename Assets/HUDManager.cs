using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Image hp;
    public Image stemina;
    public MovementSystem movement;
    public HPSystem hpSystem;
    // Start is called before the first frame update
    private void Update() {
        hp.fillAmount = 1f;
        stemina.fillAmount = movement.currentStemina / movement.templateState.maxStamina;
    }
}
