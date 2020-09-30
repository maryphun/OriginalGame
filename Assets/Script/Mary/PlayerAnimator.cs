using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponentInParent<PlayerController>();
    }
    
    /// <summary>
    /// 0 = false else = true
    /// </summary>
    /// <param name="boolean"></param>
    public void SetCanAttack(int boolean)
    {
        var attackScript = anim.GetBehaviour<Attack>();
        if (attackScript != null)
        {
            attackScript.canAttack = (boolean != 0);
        }

        if (attackScript.canAttack)
        {
            controller.ResetMoveSpeedMax();
        }
    }
}
