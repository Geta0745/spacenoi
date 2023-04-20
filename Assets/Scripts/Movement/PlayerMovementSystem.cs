using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require Rigidbody and EntityState components to be attached to the GameObject
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementSystem : MainMovementSystem
{
    [SerializeField] Animator anime;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize current stamina and get reference to Rigidbody component
        currentStemina = stat.maxStamina;
        rb = GetComponent<Rigidbody>();
    }

    protected override void SprintCaseAction()
    {
        base.SprintCaseAction();
        anime.SetBool("run",true);
    }

    protected override void WalkCaseAction()
    {
        base.WalkCaseAction();
        anime.SetBool("run",false);
    }

    protected override void UpdateStat()
    {
        base.UpdateStat();
        anime.SetFloat("speed",currentSpeed);
    }

}
