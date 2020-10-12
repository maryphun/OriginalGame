using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Transform headAim;
    [SerializeField] private GameObject[] slashFX;
    [SerializeField] private GameObject dashFX;

    private Animator anim;
    private Camera cam;
    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponentInParent<PlayerController>();
        cam = Camera.main;
    }

    /// <summary>
    /// 0 = false else = true
    /// </summary>
    /// <param name="boolean"></param>
    public void SetCanAttack(int boolean)
    {
        controller.canAttack = (boolean != 0);

        if (controller.canAttack)
        {
            controller.SetIsAttacking(false);
            controller.ResetMoveSpeedMax();
            controller.AttackEnd();
        }
    }

    public void MoveForward(float distance)
    {
        controller.Move(distance);
    }

    public void SlashFX(int variation)
    {
        switch (variation)
        {
            case 1:
                Destroy(Instantiate(slashFX[0], transform, false), 1.5f);
                break;
            case 2:
                Destroy(Instantiate(slashFX[1], transform, false), 1.5f);
                break;
            case 3:
                Destroy(Instantiate(slashFX[2], transform, false), 1.5f);
                break;
            default:
                break;
        }

    public void HitFX(int variation, Vector3 targetPos)
    {
        if (variation < 0 || variation > hitFX.Length) return;
        Instantiate(hitFX[variation], targetPos, Quaternion.identity);
    }

    public void StartRegisterAttack()
    {
        controller.StartRegisterAttack();
    }

    public void DashEnd()
    {
        controller.ResetMoveSpeedMax();
        controller.Dashing(false);
    }

    public void DashFX()
    {
        ParticleSystem fx = Instantiate(dashFX, transform, false).GetComponent<ParticleSystem>();
        StartCoroutine(StopEmission(fx, 0.3f));
    }

    private IEnumerator StopEmission(ParticleSystem targetFX, float time)
    {
        yield return new WaitForSeconds(time);
        targetFX.enableEmission = false;
    }
}
