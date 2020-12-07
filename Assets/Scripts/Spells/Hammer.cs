using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hammer : MonoBehaviour
{
    public AudioSource HitSound, activateSound, deactivateSound;
    public float damage = 10f, hitTimeout = 0.1f;
    public string damageType = "mortal";
    public bool canHitPlayer = false;

    public List<GameObject> activateGlowObjects;
    public List<ParticleSystem> activateParticles;
    public Material baseMaterial, glowMaterial;

    bool onHitTimeout = false;
    float currentTimeout = 0;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     HitSound = GetComponent<AudioSource>();
    //     //StartCoroutine(HitBoxTimeout());
    // }

    void FixedUpdate() {
        if (onHitTimeout && currentTimeout > 0f) {
            currentTimeout -= Time.deltaTime;
        } else if (onHitTimeout) {
            onHitTimeout = false;
            currentTimeout = 0f;
            ActivateGlow();
        }
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
                if (HitSound) HitSound.Play();
            }

            Rigidbody r = collision.gameObject.GetComponent<Rigidbody>();
            if (r != null) r.AddExplosionForce(1500f, transform.position, 1f);

            onHitTimeout = true;
            currentTimeout = hitTimeout;
            DeactivateGlow();
        }
    }

    void ActivateGlow() {
        // Debug.Log("Activate glow");
        if (activateSound != null) activateSound.Play();

        foreach (GameObject obj in activateGlowObjects) {
            obj.GetComponent<Renderer>().material = glowMaterial;
        }

        foreach (ParticleSystem ps in activateParticles) {
            ps.Play();
        }
    }

    void DeactivateGlow(){
        // Debug.Log("Deactivate glow");
        if (deactivateSound != null && !HitSound.isPlaying) deactivateSound.Play();

        foreach (GameObject obj in activateGlowObjects) {
            obj.GetComponent<Renderer>().material = baseMaterial;
        }

        foreach (ParticleSystem ps in activateParticles) {
            ps.Stop();
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
