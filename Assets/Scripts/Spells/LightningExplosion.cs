using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightningExplosion : MonoBehaviour
{
    public float damage = 10f;
    public string damageType = "lightning";

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
            CasterAI caster = other.GetComponent<CasterAI>();
            if (enemy != null) enemy.TakeDamage(damageType, damage);
            if (caster != null) caster.TakeDamage(damageType, damage);
        } else if (other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if (player != null) player.GetComponent<Player>().WeaponHit(damage);
        } else if (other.tag == "Shield") {
            Shield s = other.GetComponent<Shield>();
            if(s != null) s.Break();
        }
    }
}


