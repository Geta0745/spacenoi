using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    // Start is called before the first frame update
    private void Awake() {
        //init player input class
        input = new PlayerInput();
    }
    private void OnEnable() {
        Debug.LogWarning("fuck u");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("fuck u");
    }
}
