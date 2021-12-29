using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAttack : StateMachineBehaviour
{

    private float timePassed = 0;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timePassed += Time.deltaTime;

        if(timePassed < stateInfo.length) return;

        animator.SetBool("Attack", false);
    }
}
