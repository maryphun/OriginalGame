using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaryProjectile : MonoBehaviour
{
    private bool collided;
    private string targetTag;
    private Transform target;

    public void Initialization(string targettag)
    {
        targetTag = targettag;
        target = null;
    }

    public Transform IsCollidedWithTarget()
    {
        return target;
    }

    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.tag != targetTag)
        {
            //do nothing
            return;
        }

        if (!collided)
        {
            collided = true;
            GetComponent<Rigidbody>().isKinematic = true;

            target = co.transform;
        }
    }
}
