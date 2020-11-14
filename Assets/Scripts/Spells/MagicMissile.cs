using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicMissile : MonoBehaviour
{
    public float damage=10f, speed;

    public ParticleSystem explosionParticles;
    public GameObject mesh;
    public AudioSource hitSound;
    public string damageType = "arcane";

    private bool hitSomething = false, targetPlayer;
    public bool canHitPlayer = false;
    GameObject player, hand;
    CharacterController playerController;

    void Update() {
        if (!hitSomething) {
            if (targetPlayer && playerController != null) {
                Vector3 playerPos = player.transform.TransformPoint(playerController.center);
                transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
                transform.LookAt(playerPos);
            } else {
                if (hand != null) transform.rotation = hand.transform.rotation;
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

    public void LinkCastingHand(GameObject castingHand) {
        hand = castingHand;
    }

    public void Scale(float scale) {
        float scaleFactor = 0.6f + 0.8f*scale;
        speed *= scaleFactor;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" || canHitPlayer) {
            //print("MagicMissile hit: " + other.ToString());
            Destroy(GetComponent<SphereCollider>());
            Destroy(gameObject, 1.1f);
            hitSomething = true;
            
            explosionParticles.Play();
            hitSound.Play();
            mesh.SetActive(false);

            if (other.tag == "Spell_Interactable") {
                SpellInteractable si = other.GetComponent<SpellInteractable>();
                if (si != null) si.Trigger("magicmissile");
            } else if (other.tag == "Shield") {
                Shield s = other.GetComponent<Shield>();
                if(s != null) s.Break();
            } else if (other.tag == "Player") {
                if (player != null) player.GetComponent<Player>().WeaponHit(damage);
            } else if (other.tag == "Enemy" || other.tag == "Ghost") {
                EnemyAI enemy = other.GetComponent<EnemyAI>();
                if (enemy != null) enemy.TakeDamage(damageType, damage);
            }
        }
    }
}
