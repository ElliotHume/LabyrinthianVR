using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindSlash : MonoBehaviour
{
    public int damage = 1;
    public float speed = 25f;
    public float duration = 1f;

    public Vector3 direction;

    public GameObject owner;

    public GameObject hitParticle;

    // Start is called before the first frame update
    void Start() {
        Destroy(GetComponent<BoxCollider>(), duration);
        Destroy(gameObject, duration);
        int random = Random.Range(0, 5);
        //print(random);
        GetComponents<AudioSource>()[random].Play();
    }

    public void SetDirection(Vector3 g) {
        direction = new Vector3(g.x, 0, g.z).normalized;
    }

    public void SetOwner(GameObject g) {
        owner = g;
    }

    // Update is called once per frame
    void Update()
    {
        // Should counter gravity so that the player can cross gaps
        Vector3 gravity = new Vector3(0, Physics.gravity.y * 0.15f * Time.deltaTime, 0);

        owner.transform.position += (direction * Time.deltaTime * speed) - gravity ;
        transform.position = owner.transform.position + (direction * 0.5f) + Vector3.up;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spell_Interactable") {
            SpellInteractable si = other.GetComponent<SpellInteractable>();
            if (si != null) si.Trigger("windslash");
            SpawnHit();
            Destroy(gameObject);
        }
    }

    public void SpawnHit() {
        GameObject newExplosion = Instantiate(hitParticle, transform.position, transform.rotation);
    }
}
