﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpray : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10.5f);
        Destroy(GetComponent<CapsuleCollider>(), 5.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        //Debug.Log("Ice Spray hit "+other.gameObject);
        if (other.tag == "Spell_Interactable") {
            SpellInteractable si = other.GetComponent<SpellInteractable>();
            Freezable b = other.GetComponent<Freezable>();
            if (si != null) si.Trigger("icespray");
            if (b != null) b.Freeze();

            Debug.Log("Freeze: "+other.gameObject);
        }
    }
}
