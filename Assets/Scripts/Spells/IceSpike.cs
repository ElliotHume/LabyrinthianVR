using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IceSpike : MonoBehaviour
{
    public float lifetime = 15f, riseDuration = 1f;
    public int damage = 30;
    public AudioSource breakSound;
    public ParticleSystem emergeParticles, breakParticles;
    Vector3 raisedPosition;

    public void Start() {
        // Start in the lowered position and then rise up like an earth wall
        raisedPosition = transform.position;
        transform.position = transform.position+(Vector3.down * transform.lossyScale.y * 3f);
        StartCoroutine(MoveToPosition(raisedPosition));
        Invoke(nameof(Emerge), riseDuration);

        // Break after a time if a lifetime is set
        if (lifetime > 0f) Invoke(nameof(Break), 15f);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            CasterAI caster = collision.gameObject.GetComponent<CasterAI>();
            if (enemy != null) enemy.TakeDamage("ice", damage);
            if (caster != null) caster.TakeDamage("ice", damage);
            Break();
        } else if (collision.gameObject.tag == "Spell" || collision.gameObject.tag == "Missile") {
            SpellData sd = collision.gameObject.GetComponent<SpellData>();
            Debug.Log("Hit by spell with type: "+sd.damageType);
            if (sd != null && sd.damageType == "fire") Break();
        } else if (collision.gameObject.tag == "Player") {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null) p.WeaponHit(damage);
            Break();
        }
    }

    public void Break() {
        if (breakSound != null) breakSound.Play();
        if (breakParticles != null) breakParticles.Play();
        StartCoroutine(MoveToPosition(transform.position+(Vector3.down * transform.lossyScale.y * 3f)));
        Destroy(gameObject, riseDuration+2f);
    }

    void Emerge() {
        if (emergeParticles != null) emergeParticles.Play();
    }

    IEnumerator MoveToPosition(Vector3 position) {
        print("moving");
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1){
            t += Time.deltaTime / riseDuration;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return new WaitForFixedUpdate();
        }
    }
}
