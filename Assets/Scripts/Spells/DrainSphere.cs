using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainSphere : MonoBehaviour
{
    public float duration, speed=1f, damagePerTick;
    public AudioSource hitSound;
    GameObject hand;
    Vector3 prevHandLocation = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    public void LinkCastingHand(GameObject castingHand) {
        hand = castingHand;
        prevHandLocation = hand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (hand != null) {
            // Get velocity of the casting hand
            Vector3 handVelocity = (hand.transform.position - prevHandLocation) / Time.deltaTime;
            Vector3 absoluteVelocity = new Vector3( Mathf.Abs(handVelocity.x), Mathf.Abs(handVelocity.y), Mathf.Abs(handVelocity.z));
            // square the velocity, so fast movements are more impactful and slow sweeps will not move the sphere as much
            moveDirection += Vector3.Scale(handVelocity, absoluteVelocity) / 10f;

            transform.position += moveDirection * speed * Time.deltaTime;

            prevHandLocation = hand.transform.position;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Enemy" || other.tag == "Ghost") {
            if (hitSound != null && !hitSound.isPlaying) hitSound.Play();
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage("planar", damagePerTick);
        } else if (other.tag == "Player") {
            if (hitSound != null && !hitSound.isPlaying) hitSound.Play();
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.WeaponHit(damagePerTick);
        }
    }
}
