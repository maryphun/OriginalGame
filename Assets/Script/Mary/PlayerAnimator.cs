using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Transform headAim;
    [SerializeField] private GameObject[] slashFX;
    [SerializeField] private GameObject[] hitFX;
    [SerializeField] private GameObject[] dashFX;

    private Animator anim;
    private Camera cam;
    private PlayerController controller;
    private List<GameObject> dashfxs;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponentInParent<PlayerController>();
        cam = Camera.main;
        dashfxs =  new List<GameObject>();
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
                var tmp = Instantiate(slashFX[1], (transform.position + (transform.forward * 1.5f)) +
                    new Vector3(0.0f, slashFX[1].transform.position.y, 0.0f), Quaternion.Euler(new Vector3(slashFX[1].transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, slashFX[1].transform.rotation.eulerAngles.z)));
                tmp.transform.DOMove(tmp.transform.position + transform.forward * 0.15f, 0.1f, false);
                Destroy(tmp, 1.5f);
                break;
            case 3:
                Destroy(Instantiate(slashFX[2], transform, false), 1.5f);
                break;
            default:
                break;
        }
    }

    public void HitFX(int variation, Vector3 targetPos)
    {
        if (variation < 0 || variation >= hitFX.Length) return;
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
        dashfxs.Clear();
        foreach (GameObject effect in dashFX)
        {
            Transform fx = Instantiate(effect, transform, false).transform;
            StartCoroutine(StopEmission(fx, 0.3f));
            dashfxs.Add(fx.gameObject);
        }
    }

    private IEnumerator StopEmission(Transform targetFX, float time)
    {
        yield return new WaitForSeconds(time);
        var tmp = targetFX.GetComponent<ParticleSystem>();
        if (tmp != null)
        {
            tmp.enableEmission = false;
        }
        else
        {
            var trail = targetFX.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                trail.emitting = false;
            }
        }
    }

    public void StartDealDamage(float time)
    {
        StartCoroutine(DealingDamge(time));
    }

    private IEnumerator DealingDamge(float time)
    {
        float timeLeft = time;
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            DealDamge();
            yield return null;
        }
        yield return 0;
    }

    public void DealDamge()
    {
        controller.DealDamage();
    }

    public void WokeUp()
    {
        controller.VariableInitialization();
    }
}
