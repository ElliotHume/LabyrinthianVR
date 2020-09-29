using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightningExplosion : MonoBehaviour
{
    public float damage = 10f;

    void Start() {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Server call only, no call on client.
    //ServerCallback is similar to Server but doesn't generate a warning when called on client.
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spell_Interactable") {
            SpellInteractable si = other.GetComponent<SpellInteractable>();
            if (si != null) si.Trigger("lightning");
        } else if (other.tag == "Enemy") {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage("lightning", damage);
        }
    }
}


