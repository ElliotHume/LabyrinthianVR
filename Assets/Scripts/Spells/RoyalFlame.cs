using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoyalFlame : MonoBehaviour
{
    public float damagePerTick = 0.1f;
    public AudioSource hitSound;

    // Start is called before the first frame update
    public void Start()
    {
        Destroy(GetComponent<CapsuleCollider>(), 10f);
        Destroy(gameObject, 14f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Enemy") {
            if (hitSound != null && !hitSound.isPlaying) hitSound.Play();
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage("arcane", damagePerTick);
        } else if (other.tag == "Player") {
            if (hitSound != null && !hitSound.isPlaying) hitSound.Play();
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.WeaponHit(damagePerTick);
        }
    }
}
