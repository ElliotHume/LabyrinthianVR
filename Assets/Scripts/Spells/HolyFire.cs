using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HolyFire : MonoBehaviour
{
    public float damagePerTick = 0.1f;
    public AudioSource hitSound;
    public bool hitPlayer = false;

    // Start is called before the first frame update
    public void Start()
    {
        Destroy(gameObject, 15f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HitPlayer() {
        hitPlayer = true;
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Enemy" || other.tag == "Ghost") {
            if (hitSound != null && !hitSound.isPlaying) hitSound.Play();
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage("planar", damagePerTick);
        } else if (other.tag == "Player" && hitPlayer) {
            if (hitSound != null && !hitSound.isPlaying) hitSound.Play();
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.WeaponHit(damagePerTick);
        } else if (other.tag == "Spell_Interactable") {
            SpellInteractable si = other.GetComponent<SpellInteractable>();
            if (si != null) si.Trigger("holyfire");
        }
    }
}
