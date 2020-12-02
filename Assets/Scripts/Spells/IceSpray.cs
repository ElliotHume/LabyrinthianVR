using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpray : MonoBehaviour
{
    public float damagePerTick = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10.5f);
        Destroy(GetComponent<Collider>(), 5.5f);
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
        } else if (other.tag == "Enemy") {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            CasterAI caster = other.GetComponent<CasterAI>();
            if (enemy != null) {
                enemy.Slow(15f);
                enemy.TakeDamage("ice", damagePerTick);
            }
            if (caster != null) caster.TakeDamage("ice", damagePerTick);
        } else if (other.tag == "Missile") {
            Destroy(other.gameObject);
        }
    }

}
