using System.Collections;
using System.Collections.Generic;
using Ludiq.PeekCore;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    public UnityEvent onPickUp;
    [SerializeField]
    private Collider collider;
    private Rigidbody rigidbody;
    private Transform player;

    public ParticleSystem healPrefab;
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
            player = co.transform;
            PickUp();
        }
        
            if (co.gameObject.tag == "Enemy")
            {
                Debug.Log("rip");
            }
        
    }

    public void PickUp()
    {
        onPickUp.Invoke();
        Destroy(gameObject);
    }

    public void Heal(int val)
    {
        player.GetComponent<PlayerController>().Heal(val);
        var healPS = Instantiate(healPrefab, player.position, Quaternion.identity);
        healPS.transform.parent = player;
        healPS.Play();
        healPS.AddComponent<AutoDestroyParticle>();
    }
}
