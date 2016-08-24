using UnityEngine;
using System.Collections;

public class AttackBehavior : StateMachineBehaviour
{
    private Character character;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
        //character = GetCharacter(animator);
        //if (character.CurrentWeapon.IsAttacking)
        //{
        //    character.CurrentWeapon.IsAttacking = false;

        //    if (character.CurrentWeapon.attackAnimations.Length <= 0) return;
        //    var nextAnim = Random.Range(0, character.CurrentWeapon.attackAnimations.Length);

        //    //animator.CrossFade(character.CurrentWeapon.attackAnimations[nextAnim], 0f);
        //    animator.Play(character.CurrentWeapon.attackAnimations[nextAnim]);
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
