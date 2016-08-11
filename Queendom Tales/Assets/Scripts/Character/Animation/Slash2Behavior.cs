﻿using UnityEngine;
using System.Collections;

public class Slash2Behavior : StateMachineBehaviour
{
    private Character character;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Character chara = GetCharacter(animator);
        var obj = GameObject.Instantiate(animator.GetFloat("x") == -1 ? chara.Slash2Prefab_Left : chara.Slash2Prefab_Right);
        obj.transform.SetParent(animator.transform, false);
        obj.transform.localPosition = GetCharacter(animator).Slash2Offset * animator.GetFloat("x");

        animator.SetBool("combo", false);
        animator.SetFloat("comboTime", 0f);
    }

    Character GetCharacter(Animator animator)
    {
        if (character == null)
        {
            character = animator.GetComponentInParent<Character>();
        }
        return character;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //animator.SetBool("combo", true);
        }

        animator.SetFloat("comboTime", animator.GetFloat("comboTime") + Time.smoothDeltaTime);

        if (animator.GetFloat("comboTime") > 0.35f && animator.GetBool("combo"))
        {
            var rnd = Random.Range(1, 3);
            animator.CrossFade("Melee " + (rnd % 2 == 0 ? rnd + 1 : rnd), 0.15f);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //if (Input.GetKeyDown(KeyCode.Z) && !animator.GetBool("combo"))
        //{
        //    animator.CrossFade("Melee " + Random.Range(1, 3), 0f);
        //}
        //var rnd = Random.Range(1, 3);
        //animator.CrossFade("Melee " + (rnd % 2== 0 ? rnd+1 : rnd), 0f);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
