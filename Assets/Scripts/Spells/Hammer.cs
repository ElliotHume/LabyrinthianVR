using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public AudioSource HitSound;
    public float damage = 10f, hitTimeout = 0.1f;
    bool onHitTimeout = false;

    // Start is called before the first frame update
    void Start()
    {
        HitSound = GetComponent<AudioSource>();
        StartCoroutine(HitBoxTimeout());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if (!onHitTimeout){
            if (collision.gameObject.tag == "Spell_Interactable") {
                SpellInteractable si = collision.gameObject.GetComponent<SpellInteractable>();
                Breakable b = collision.gameObject.GetComponent<Breakable>();
                EarthWall ew = collision.gameObject.GetComponent<EarthWall>();
                if (HitSound) HitSound.Play();
                if (si != null) si.Trigger("hammer");
                if (ew != null) ew.Shatter(collision.GetContact(0));
                if (b != null) b.Break();
            } else if (collision.gameObject.tag == "Shield") {
                Shield s = collision.gameObject.GetComponent<Shield>();
                if(s != null) s.Break();
            } else if (collision.gameObject.tag == "Enemy") {
                EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
                if (enemy != null) enemy.TakeDamage("mortal", damage);
                if (HitSound) HitSound.Play();
            }

            onHitTimeout = true;
        }
    }

    IEnumerator HitBoxTimeout() {
        while (true) {
            if (onHitTimeout) {
                yield return new WaitForSeconds(hitTimeout);
                onHitTimeout = false;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
