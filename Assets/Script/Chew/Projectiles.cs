using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public struct ProjectileEvent
{
    public UnityEvent onDetectingBlockableObject;

}

public class Projectiles : MonoBehaviour
{

    public float speed;
    public ProjectileEvent projectileEvent;
    public LayerMask blockableMask;
    public bool destroyOnDetect;
    private ParticleSystem particleSystem;
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public List<GameObject> trails;

    private bool collided;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveWithWallCheck();
        if (DetectObject())
        {
            projectileEvent.onDetectingBlockableObject.Invoke();
        }

    }

    private void MoveWithWallCheck()
    {
        RaycastHit hit;
        LayerMask wallMask = LayerMask.GetMask("Wall");

        if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime, wallMask))
        {
            particleSystem.enableEmission = false;
            

            Debug.Log("Wall Detected");
            Destroy(gameObject, 2f);
            return;
        }
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public bool DetectObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime, blockableMask))
        {
            if (destroyOnDetect)
            {
                Debug.Log("Object Detected");

                Destroy(gameObject);
            }
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.tag != "Bullet" && !collided)
        {
            collided = true;

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

            speed = 0;
            GetComponent<Rigidbody>().isKinematic = true;

            ContactPoint contact = co.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos, rot) as GameObject;

                var ps = hitVFX.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
                else
                    Destroy(hitVFX, ps.main.duration);
            }

            if (co.gameObject.tag == "Player")
            {
                co.gameObject.GetComponent<PlayerController>().TakeDamage(1, transform);
            }

            StartCoroutine(DestroyParticle(0f));
        }
    }

    public IEnumerator DestroyParticle(float waitTime)
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
