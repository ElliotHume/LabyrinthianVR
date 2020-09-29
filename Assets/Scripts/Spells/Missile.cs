using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float damage=10f, speed=6f, decayTime=1.1f;

    public ParticleSystem explosionParticles;
    public AudioSource hitSound;
    public GameObject mesh;

    private bool canHitPlayer = false, targetPlayer = false;
    public bool active = true;
    GameObject player;
    CharacterController playerController;

    void Update() {
        if (active) {
            if (targetPlayer && playerController != null) {
                Vector3 playerPos = player.transform.TransformPoint(playerController.center);
                transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
                transform.LookAt(playerPos);
            } else {
                transform.position += transform.forward * Time.deltaTime * speed;
            }
        }
    }

    public void TargetPlayer() {
        targetPlayer = true;
    }

    public void HitPlayer() {
        canHitPlayer = true;
        player = GameObject.Find("XR Rig");
        playerController = player.GetComponent<CharacterController>();
    }

    public virtual void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" || canHitPlayer) {
            Destroy(GetComponent<Collider>());
            Destroy(gameObject, decayTime);
            active = false;
            
            if (explosionParticles != null) explosionParticles.Play();
            if (hitSound != null) hitSound.Play();
            if (mesh != null) mesh.SetActive(false);

            if (other.tag == "Spell_Interactable") {
                SpellInteractable si = other.GetComponent<SpellInteractable>();
                if (si != null) si.Trigger(gameObject.name);
            } else if (other.tag == "Player") {
                if (player != null) player.GetComponent<Player>().WeaponHit(damage);
            } else if (other.tag == "Enemy") {
                EnemyAI enemy = other.GetComponent<EnemyAI>();
                if (enemy != null) enemy.TakeDamage("mortal", damage);
            }
        }
    }

    public void Shoot() {
        transform.parent = null;
        active = true;
    }
}
