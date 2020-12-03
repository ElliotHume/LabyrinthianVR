using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public AudioSource HitSound;
    public float damage = 10f, hitTimeout = 0.1f;
    public string damageType = "mortal";
    public bool canHitPlayer = false;
    bool onHitTimeout = false;

    // Start is called before the first frame update
    void Start()
    {
        HitSound = GetComponent<AudioSource>();
        StartCoroutine(HitBoxTimeout());
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
                CasterAI caster = collision.gameObject.GetComponent<CasterAI>();
                if (enemy != null) enemy.TakeDamage(damageType, damage);
                if (caster != null) caster.TakeDamage(damageType, damage);
                if (HitSound) HitSound.Play();
            } else if (collision.gameObject.tag == "Player" && canHitPlayer) {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null) player.WeaponHit(damage);
            }

            Rigidbody r = collision.gameObject.GetComponent<Rigidbody>();
            if (r != null) r.AddExplosionForce(1500f, transform.position, 1f);

            onHitTimeout = true;
        }
    }

    public void HitPlayer() {
        canHitPlayer = true;
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
