using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public float damage=10f, speed;
    public GameObject castFX, hitFX;
    public AudioSource hitSound, loopSound;
    public string damageType = "arcane", spellInteractorTriggerKey = "projectile";
    public bool canHitPlayer = false;
    public List<GameObject> destroyOnCollision;



    bool hitSomething = false, targetPlayer;
    GameObject player, hand, owner = null;
    CharacterController playerController;

    void Start() {
        if (castFX != null) Instantiate(castFX, transform.position, transform.rotation);
    }

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

    public void SetOwner(GameObject go) {
        owner = go;
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
        if ((other.tag != "Player" || canHitPlayer) && other.gameObject != owner) {
            Destroy(GetComponent<Collider>());
            Destroy(gameObject, 1f);
            if (loopSound != null && loopSound.isPlaying) loopSound.Stop();
            hitSomething = true;
            
            foreach (GameObject go in destroyOnCollision) {
                Destroy(go);
            }

            if (hitFX != null) Instantiate(hitFX, transform.position, transform.rotation * Quaternion.Euler(-90, 0, 0));
            if (hitSound != null) hitSound.Play();

            if (other.tag == "Spell_Interactable") {
                SpellInteractable si = other.GetComponent<SpellInteractable>();
                if (si != null) si.Trigger(spellInteractorTriggerKey);
            } else if (other.tag == "Shield") {
                Shield s = other.GetComponent<Shield>();
                if(s != null) s.Break();
            } else if (other.tag == "Player") {
                if (player != null) player.GetComponent<Player>().WeaponHit(damage);
            } else if (other.tag == "Enemy") {
                EnemyAI enemy = other.GetComponent<EnemyAI>();
                if (enemy != null) enemy.TakeDamage(damageType, damage);
            }
        }
    }
}
