using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    public AudioSource attackSound, swingSound;
    public LayerMask whatIsPlayer;
    public float attackDuration = 1f, attackRange = 1f;
    public float damage = 20f;

    public ParticleSystem[] particles;

    BoxCollider bcollider;
    Player player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("XR Rig").GetComponent<Player>();
    }

    public virtual void Attack() {
        if (swingSound != null) swingSound.Play();
        Invoke(nameof(CheckAttack), attackDuration);
    }

    void CheckAttack() {
        if (attackSound != null) attackSound.Play();
        if (particles.Length > 0) foreach(ParticleSystem p in particles) p.Play();
        if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer)) {
            player.WeaponHit(damage);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" || other.tag == "BodyPart") {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.WeaponHit(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a blue sphere for the hitscan attack range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
