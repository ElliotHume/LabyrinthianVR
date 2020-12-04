using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MagnoSphere : MonoBehaviour
{
    public float speed = 0f, checkRadius = 6f, damage = 10f;
    public string damageType = "mortal";
    public GameObject hitFX;
    public AudioSource homingSound;

    public bool canHitPlayer = false;
    bool targettingPlayer = false, firstTimeTarget = true;
    GameObject player;
    CharacterController playerController;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Find the closest magnet/enemy, if any
        float closestMagnet = 100f;
        GameObject target = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (Collider collider in hitColliders) {
            if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Magnet" || (collider.gameObject.tag == "Player" && canHitPlayer)) {
                if( Vector3.Distance(collider.gameObject.transform.position, transform.position) < closestMagnet ) {
                    closestMagnet = Vector3.Distance(collider.gameObject.transform.position, transform.position);
                    target = collider.gameObject;

                    // Check if the target is the player, it will need a special movement pattern
                    targettingPlayer = (collider.gameObject.tag == "Player" && canHitPlayer);
                }
            }
        }

        if (canHitPlayer && player == null) HitPlayer();

        // if you have found a target, move towards it
        if (target != null) {
            // If its the first time something is being targetted, play the sound
            if (firstTimeTarget) {
                homingSound.Play();
                firstTimeTarget = false;
            }
            
            // Stop floating around, and disable the grab interactable
            FloatingObject fo = GetComponent<FloatingObject>();
            XRGrabInteractable gi = GetComponent<XRGrabInteractable>();
            if (fo != null) fo.enabled = false;
            if (gi != null) gi.enabled = false;

            // Look at the target
            transform.LookAt(target.transform.position+target.transform.up * 0.5f);
            if (targettingPlayer) transform.LookAt(player.transform.TransformPoint(playerController.center));

            // Move towards the target with ramping speed
            speed += 0.1f;
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }


    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Spell_Interactable") {
            SpellInteractable si = collision.gameObject.GetComponent<SpellInteractable>();
            Breakable b = collision.gameObject.GetComponent<Breakable>();
            if (si != null) si.Trigger("magnosphere");
            if (b != null) b.Break();
            Break();
        } else if (collision.gameObject.tag == "Shield") {
            Shield s = collision.gameObject.GetComponent<Shield>();
            if(s != null) s.Break();
            Break();
        } else if (collision.gameObject.tag == "Enemy") {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            CasterAI caster = collision.gameObject.GetComponent<CasterAI>();
            if (enemy != null) enemy.TakeDamage(damageType, damage);
            if (caster != null) caster.TakeDamage(damageType, damage);
            Break();
        } else if (collision.gameObject.tag == "Player" && canHitPlayer) {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null) player.WeaponHit(damage);
            Break();
        }

        Rigidbody r = collision.gameObject.GetComponent<Rigidbody>();
        if (r != null) r.AddExplosionForce(1500f, transform.position, 1f);
    }

    public void HitPlayer(){
        canHitPlayer = true;
        gameObject.layer = 12;
        player = GameObject.Find("XR Rig");
        playerController = player.GetComponent<CharacterController>();
    }


    public void Break() {
        if (hitFX != null) Instantiate(hitFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
