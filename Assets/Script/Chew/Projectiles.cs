using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
//public struct ProjectileEvent
//{
//    public UnityEvent onDetectingBlockableObject;

//}

[RequireTag("Bullet")]
public class Projectiles : MonoBehaviour
{
    //public ProjectileEvent projectileEvent;
    //public LayerMask blockableMask;
    //public bool destroyOnDetect;
    private ParticleSystem particleSystem;
    public GameObject muzzlePrefab; //asset effect
    public GameObject hitPrefab;    //asset effect
    public List<GameObject> trails; //asset effect
    private Transform target;
    private Transform owner;


    private bool collided;

    public void Initialization(Transform ownerTransform)
    {
        owner = ownerTransform;
        target = null;

    }

    public Transform IsCollidedWithTarget()
    {
        return target;
    }

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        //projectile effect
        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(muzzleVFX, ps.main.duration);
            }
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //public bool DetectObject()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime, blockableMask))
    //    {
    //        if (destroyOnDetect)
    //        {
    //            Debug.Log("Object Detected");

    //            Destroy(gameObject);
    //        }
    //        return true;
    //    }
    //    return false;
    //}

    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.tag == "Bullet" || co.gameObject.tag == owner.tag)
        {
            //do nothing
            return;
        }
        if (co.gameObject.tag != "Bullet" && !collided)
        {
            collided = true;
            GetComponent<Rigidbody>().isKinematic = true;
            target = co.transform;


            //stop trails effect
            if (trails.Count > 0)
            {
                for (int i = 0; i < trails.Count; i++)
                {
                    trails[i].transform.parent = null;
                    var ps = trails[i].GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Stop();
                        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                    }
                }
            }


            // play hit effect
            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, co.ClosestPointOnBounds(transform.position), transform.rotation) as GameObject;

                var ps = hitVFX.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
                else
                {
                    Destroy(hitVFX, ps.main.duration);
                }
            }
            if (target.GetComponent<PlayerController>() != null)
            {
                target.GetComponent<PlayerController>().TakeDamage(1, owner);
            }
            StartCoroutine(DestroySelf(0f));


        }
    }

    public IEnumerator DestroySelf(float waitTime)
    {

        if (transform.childCount > 0 && waitTime != 0)
        {
            List<Transform> tList = new List<Transform>();

            foreach (Transform t in transform.GetChild(0).transform)
            {
                tList.Add(t);
            }

            while (transform.GetChild(0).localScale.x > 0)
            {
                yield return new WaitForSeconds(0.01f);
                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for (int i = 0; i < tList.Count; i++)
                {
                    tList[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
