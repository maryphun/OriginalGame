using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour
{
    public bool canAttack;
    
    private void Awake()
    {
        canAttack = true;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canAttack = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canAttack = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

}


//private void Update()
//{

//    // Get mouse position Input
//    Vector2 mousePositionValue = mouseInput.Camera.MousePosition.ReadValue<Vector2>();

//    Ray camRay = Camera.main.ViewportPointToRay(mousePositionValue);
//    float t = (followCharacter.position.y - camRay.origin.y) / camRay.direction.y;
//    Vector3 worldPosition = camRay.origin + camRay.direction * t;

//    // Follow Target GameObject
//    Vector3 targetPos = Vector3.Lerp(followCharacter.position, worldPosition, .5f);  //picking the middle between mouse position and followTarget
//    transform.DOMove(new Vector3(targetPos.x + cameraDistance.x, transform.position.y, targetPos.z + cameraDistance.z), followSpeed, false);
//}