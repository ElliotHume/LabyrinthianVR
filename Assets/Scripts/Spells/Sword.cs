using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sword : MonoBehaviour
{
    public AudioSource HitSound;
    public float damage = 10f, hitTimeout = 1f;
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
        if (hitTimeout == 0f){
            //print("hitsomething");
            if (collision.gameObject.tag == "Spell_Interactable") {
                SpellInteractable si = collision.gameObject.GetComponent<SpellInteractable>();
                if (HitSound) HitSound.Play();
                if (si != null) si.Trigger("sword");
            } else if (collision.gameObject.tag == "Enemy") {
                EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
                if (enemy != null) enemy.TakeDamage(damageType, damage);
                if (HitSound) HitSound.Play();
            } else if (collision.gameObject.name.Contains("Hammer") && damageType != "mortal") {
                grabInteractable.colliders.Clear();
                Destroy(gameObject);
                Instantiate(MortalSword, transform.position, transform.rotation);
            }

            hitTimeout = 1f;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (hitTimeout == 0f){
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

            hitTimeout = 1f;
        }
    }

    IEnumerator HitBoxTimeout() {
        while (true) {
            if (hitTimeout > 0f) {
                yield return new WaitForSeconds(hitTimeout);
                hitTimeout = 0f;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
