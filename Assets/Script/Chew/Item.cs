﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    public UnityEvent onPickUp;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
