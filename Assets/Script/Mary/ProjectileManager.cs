using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField, Header("Debug")] private bool debugModeEnabled = false;
    [SerializeField] private Transform projectileorigin;
    [SerializeField] private Transform player;
    [SerializeField, Range(0.1f, 5.0f)] private float interval = 3f;
    [SerializeField, Range(0.1f, 5.0f)] private float reachTime = 0.2f;
    [SerializeField] private bool followPlayer = true;
    [SerializeField, Range(0.0f, 1.0f)] private float _followTime = 0.5f;

    //public TestDelegate m_methodToCall;
    public delegate void CustomDelegate();

    public void InitiateProjectile(Transform owner, Transform projectile, Vector3 startPoint, Vector3 endPoint, float time, CustomDelegate returnCallBack)
    {
        StartCoroutine(ProjectileLoop(owner, projectile, startPoint, endPoint, time, returnCallBack));
    }


    /// <summary>
    /// follow time should be from 0.0f to 1.0f
    /// </summary>
    public void InitiateProjectile(Transform owner, Transform projectile, Vector3 startPoint, Transform followTarget, float followTime, float time, CustomDelegate returnCallBack)
    {
        if (followTime > 1.0f) Debug.LogWarning("WARNING: followTime should be float between 0.01f and 1.0f");
        Mathf.Clamp(followTime, 0.0f, 1.0f);
        StartCoroutine(ProjectileFollow(projectile, startPoint, followTarget, followTime, time, returnCallBack));
    }

    public void InitiateProjectileWithDirection(Transform owner, Transform projectile, Vector3 startPoint, Vector3 directionVector, float lastingtime, CustomDelegate returnCallBack)
    {
        StartCoroutine(ProjectilePlain(owner, projectile, startPoint, directionVector, lastingtime, returnCallBack));
    }

    private IEnumerator ProjectileLoop(Transform owner, Transform projectile, Vector3 startPoint, Vector3 endPoint, float time, CustomDelegate returnCallBack)
    {
        float timeElapsed = 0.0f;
        float lerp = 0.0f;
        Vector3 velocity;
        float timePow = Mathf.Pow(1.0f, 2); 
        float gravity = 4.9f;

        // calculate intial X and Y
        velocity.x = (endPoint.x - startPoint.x);     //\frac{\left(x_{t}-x_{i}\right)}{t_{t}}
        velocity.z = (endPoint.z - startPoint.z);     //\frac{\left(x_{t}-x_{i}\right)}{t_{t}}
        velocity.y = (endPoint.y + (gravity * timePow) - startPoint.y); //v_{y}=\frac{\left(y_{t}+4.9t_{t}^{2}-y_{i}\right)}{t_{t}}

        var proj = Instantiate(projectile, startPoint, Quaternion.identity);
        proj.LookAt(endPoint);

        Vector3 lastLoc = proj.position;

        while (lerp < 1.0f)
        {
            timeElapsed += Time.deltaTime;
            lerp = (timeElapsed / time) * 1.0f;
            timePow = Mathf.Pow(lerp, 2);

            // calculate position  \left(v_{x}t+x_{i},-4.9t^{2}+v_{y}t+y_{i}\right)
            proj.position = new Vector3((velocity.x * lerp) + startPoint.x, (-gravity * timePow) + (velocity.y * lerp) + startPoint.y, (velocity.z * lerp) + startPoint.z);

            // update rotation
            Vector3 relativePos = proj.position - lastLoc;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            proj.rotation = rotation;

            lastLoc = proj.position;

            yield return null;
        }

        DestroyProjectile(proj);

        if (returnCallBack != null)
        {
            returnCallBack();
        }
    }

    private IEnumerator ProjectileFollow(Transform projectile, Vector3 startPoint, Transform followTarget, float followTime, float time, CustomDelegate returnCallBack)
    {
        float timeElapsed = 0.0f;
        float lerp = 0.0f;

        var proj = Instantiate(projectile, startPoint, Quaternion.identity);
        proj.LookAt(followTarget.position);
        Vector3 targetPoint = new Vector3(followTarget.position.x, startPoint.y, followTarget.position.z);
        Vector3 originPoint = startPoint;
        Vector3 lastLoc = proj.position;
        float height = originPoint.y;
        bool changeTargetPoint = false;

        while (lerp < 1.0f)
        {
            timeElapsed += Time.deltaTime;
            lerp = (timeElapsed / time) * 1.0f;
            

            if (lerp < followTime)
            {
                // still following player
                height = Mathf.Lerp(0.0f, 8.0f, lerp / followTime);
                targetPoint.y = height;
            }
            else
            {
                if (!changeTargetPoint)
                {
                    originPoint = proj.position;
                    targetPoint.x = followTarget.position.x;
                    targetPoint.z = followTarget.position.z;
                    changeTargetPoint = true;
                }
                //(currentX - minX) / (maxX - minX)
                lerp = (lerp - followTime) / (1.0f - followTime);
                targetPoint.y = Mathf.Lerp(height, 0.0f, lerp);
            }

            // update position
            Vector3 newPos = Vector3.Lerp(originPoint, targetPoint, lerp);
            proj.position = newPos;

            // update rotation
            Vector3 relativePos = proj.position - lastLoc;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            //proj.DORotateQuaternion(rotation.eulerAngles, 1f, RotateMode.WorldAxisAdd);
            proj.DORotateQuaternion(rotation, 0.15f);
            lastLoc = proj.position;

            yield return null;
        }

        DestroyProjectile(proj);

        if (returnCallBack != null)
        {
            returnCallBack();
        }
    }

    private IEnumerator ProjectilePlain(Transform owner, Transform projectile, Vector3 startPoint, Vector3 directionVector, float lastingTime, CustomDelegate returnCallBack)
    {
        var proj = Instantiate(projectile, startPoint, Quaternion.identity);
        var script = proj.gameObject.AddComponent<MaryProjectile>();
        script.Initialization("Player");
        float timeElapsed = 0.0f;

        proj.LookAt(proj.position + directionVector);

        while (timeElapsed < lastingTime)
        {
            timeElapsed += Time.deltaTime;

            proj.DOMove(proj.position + directionVector.normalized, Time.deltaTime, false);

            Transform target = script.IsCollidedWithTarget();
            if (target != null)
            {
                // collided
                if (target.GetComponent<PlayerController>() != null)
                {
                    target.GetComponent<PlayerController>().TakeDamage(1, owner);
                }
            }

            yield return null;
        }

        // Destroy the projectile
        DestroyProjectile(proj);
        if (returnCallBack != null)
        {
            returnCallBack();
        }
    }

    private void DestroyProjectile(Transform proj)
    {
        List<Transform> trails = new List<Transform>(); ;
        Transform[] allChildren = proj.GetComponentsInChildren<Transform>();
        float removeTime = 0.0f;

        foreach (Transform child in allChildren)
        {
            var t = child.GetComponent<TrailRenderer>();
            var p = child.GetComponent<ParticleSystem>();
            if (t != null || p != null)
            {
                trails.Add(child);
            }
        }

        if (trails.Count > 0)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                trails[i].transform.parent = null;
                var ps = trails[i].GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop();
                    float delay = ps.main.duration + ps.main.startLifetime.constantMax;
                   
                    Destroy(ps.gameObject, delay);
                    ps.transform.DOScale(0.0f, removeTime);
                    if (delay > removeTime)
                    {
                        removeTime = delay;
                    }
                }
            }
        }

        proj.DOScale(0.0f, removeTime);
        Destroy(proj.gameObject, removeTime);
    }

    ///Debug-------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        if (debugModeEnabled)
        {
            StartCoroutine(debug(interval, reachTime));
        }
    }

    private IEnumerator debug(float inter, float time)
    {
        yield return new WaitForSeconds(inter);

        if (!followPlayer)
        {
            InitiateProjectile(transform, projectileorigin, new Vector3(0.0f, 0.0f, 0.0f), player.position, time, null);
        }
        else
        {
            InitiateProjectile(transform, projectileorigin, new Vector3(0.0f, 0.0f, 0.0f), player, _followTime, time, null);
        }

        StartCoroutine(debug(interval, reachTime));
    }

}