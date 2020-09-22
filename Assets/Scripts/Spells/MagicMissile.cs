using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicMissile : MonoBehaviour
{

    private Vector3 startPosition;
    private Vector3 endPosition;
    public float damage=10f, speed;

    public ParticleSystem explosionParticles;
    public GameObject mesh;
    public AudioSource hitSound;

    private bool hitSomething = false;
    private GameObject TPAnchor;
    public bool canHitPlayer = false;
    

    // private bool wasReflected = false;
    // public bool playerThrown; TODO


    // Start is called before the first frame update
    public void Start()
    {
        if (hitSound == null ) hitSound = GetComponent<AudioSource>();
    }

    void Update() {
        if (!hitSomething) transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void SetTarget(Vector3 p) {
        endPosition = p;
    }

    public void HitPlayer() {
        canHitPlayer = true;
    }

    public void TeleportAnchor(GameObject anchor) {
        TPAnchor = anchor;
    }

    void OnTriggerEnter(Collider other) {
        //print("MagicMissile hit: " + other.ToString());
        if ((other.tag != "BodyPart" && other.tag != "Player") || canHitPlayer) {
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
            } else if ((other.tag == "BodyPart" || other.tag == "Player") && TPAnchor != null) {
                Debug.Log("Hit Player, moving to TPAnchor");
                other.gameObject.transform.position = TPAnchor.transform.position;
                other.gameObject.transform.rotation = TPAnchor.transform.rotation;
            } else if (other.tag == "Enemy") {
                EnemyAI enemy = other.GetComponent<EnemyAI>();
                if (enemy != null) enemy.TakeDamage(damage);
            }
        } 
    }
}
