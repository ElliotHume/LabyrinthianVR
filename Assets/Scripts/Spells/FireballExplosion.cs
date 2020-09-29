using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireballExplosion : MonoBehaviour
{
    public float damage = 25;
    public GameObject ownerGO;

    

    // Start is called before the first frame update
    public void Start() {
        Destroy(GetComponent<SphereCollider>(), 0.05f);
        Destroy(gameObject, 2f);
    }

    public void SetOwner(GameObject go, bool playerOwned) {
        ownerGO = go;
        if (playerOwned) GetComponent<SphereCollider>().radius += 1f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //Server call only, no call on client.
    //ServerCallback is similar to Server but doesn't generate a warning when called on client.
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Shield") {
            other.GetComponent<Shield>().Break();
            Destroy(GetComponent<SphereCollider>(), 0);
        } else if (other.tag == "Spell_Interactable") {
            SpellInteractable si = other.GetComponent<SpellInteractable>();
            Burnable b = other.GetComponent<Burnable>();
            if (si != null) si.Trigger("fireball");
            if (b != null) b.Burn();
            Destroy(GetComponent<SphereCollider>(), 0);
        } else if (other.tag == "Enemy") {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage("fire", damage);
        } else if (other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if (player != null) player.WeaponHit(damage);
        }
    }
}


