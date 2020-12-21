using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    public UnityEvent onPickUp;
    [SerializeField]
    private Collider collider;
    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.tag == "Player")
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        onPickUp.Invoke();
        Destroy(gameObject);
    }

    public void Heal()
    { 
    }
}
