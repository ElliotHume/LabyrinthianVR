using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightningExplosion : MonoBehaviour
{
    // Doesn't deal damage; damage is dealt directly by Lightning.

    void Start() {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


