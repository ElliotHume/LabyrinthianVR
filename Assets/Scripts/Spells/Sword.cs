using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sword : MonoBehaviour
{
    public AudioSource HitSound;
    public float damage = 5f, hitTimeout = 0.2f;
    bool onHitTimeout = false;
    public string damageType;

    public GameObject FireSword, IceSword, ArcaneSword, PlanarSword, MortalSword;

    public XRGrabInteractable grabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        HitSound = GetComponent<AudioSource>();
        if (grabInteractable == null) grabInteractable = GetComponent<XRGrabInteractable>();
        //StartCoroutine(HitBoxTimeout());
    }

    void Awake() {
        StartCoroutine(HitBoxTimeout());
    }

    void OnCollisionEnter(Collision collision) {
        if (!onHitTimeout){
            //print("hitsomething");
            if (collision.gameObject.tag == "Spell_Interactable") {
                SpellInteractable si = collision.gameObject.GetComponent<SpellInteractable>();
                if (HitSound) HitSound.Play();
                if (si != null) si.Trigger("sword");
            } else if (collision.gameObject.tag == "Enemy" || (collision.gameObject.tag == "Ghost" && (damageType == "planar" || damageType == "arcane"))) {
                EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
                if (enemy != null) enemy.TakeDamage(damageType, damage);
                if (HitSound) HitSound.Play();
            } else if (collision.gameObject.name.Contains("Hammer") && damageType != "mortal") {
                grabInteractable.colliders.Clear();
                Destroy(gameObject);
                Instantiate(MortalSword, transform.position, transform.rotation);
            }

            onHitTimeout = true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!onHitTimeout){
            //print("hitsomething");
            if (other.tag == "Spell_Interactable") {
                SpellInteractable si = other.gameObject.GetComponent<SpellInteractable>();
                if (HitSound) HitSound.Play();
                if (si != null) si.Trigger("sword");
            } else if (other.gameObject.layer == 12 || other.gameObject.layer == 11) {
                GameObject prefab=null;

                if ((other.gameObject.name.Contains("FireballExplosion") || other.gameObject.name.Contains("Fireball")) && damageType != "fire") {
                    prefab = FireSword;
                } else if (other.gameObject.name.Contains("IceSpray") && damageType != "ice") {
                    prefab = IceSword;
                } else if (other.gameObject.name.Contains("DrainSphere") && damageType != "planar") {
                    prefab = PlanarSword;
                } else if (other.gameObject.name.Contains("MagicMissile") && damageType != "arcane") {
                    prefab = ArcaneSword;
                }

                if (prefab != null) {
                    grabInteractable.colliders.Clear();
                    Destroy(gameObject);
                    Instantiate(prefab, transform.position, transform.rotation);
                }
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
