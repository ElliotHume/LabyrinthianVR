using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    public AudioSource hitSound, swingSound;
    public LayerMask whatIsPlayer;
    public float attackDuration = 1f, attackRange = 1f;
    public float damage = 20f;

    BoxCollider bcollider;
    Player player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("XR Rig").GetComponent<Player>();
    }

    public void Attack() {
        swingSound.Play();
        Invoke(nameof(CheckAttack), attackDuration);
    }

    void CheckAttack() {
        if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer)) {
            player.WeaponHit(damage);
            hitSound.Play();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" || other.tag == "BodyPart") {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.WeaponHit(damage);
            hitSound.Play();
        }
    }
}
