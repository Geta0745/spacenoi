using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] MovementSystem movementMaster;
    InputAction move,mousePos,sprint;

    private void Awake() {
        //init player input class
        input = new PlayerInput();
        movementMaster = GetComponent<MovementSystem>();
    }
    private void OnEnable() {
        move = input.Player.Move;
        mousePos = input.Player.Look;
        sprint = input.Player.sprint;
        mousePos.Enable();
        sprint.Enable();
        move.Enable();
    }
    private void OnDisable() {
        move.Disable();
        sprint.Disable();
        mousePos.Disable();
    }
    private void Update() {
        movementMaster.SetMovement(move.ReadValue<Vector2>());
        Debug.Log(sprint.ReadValue<float>());
        movementMaster.setSprint(sprint.ReadValue<float>());
    }
}
