using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindSlash : MonoBehaviour
{
    public int damage = 1;

    public Vector3 direction;

    public GameObject owner;

    public GameObject hitParticle;

    // Start is called before the first frame update
    void Start() {
        Destroy(GetComponent<BoxCollider>(), 1f);
        Destroy(gameObject, 1.5f);
        int random = Random.Range(0, 5);
        //print(random);
        GetComponents<AudioSource>()[random].Play();
    }

    public void SetDirection(Vector3 g) {
        direction = g;
    }

    public void SetOwner(GameObject g) {
        owner = g;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = owner.transform.position + (direction * 2f) + Vector3.up;
        owner.transform.position += direction * Time.deltaTime * 25f;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spell_Interactable") {
            SpellInteractable si = other.GetComponent<SpellInteractable>();
            if (si != null) si.Trigger("windslash");
            SpawnHit();
            Destroy(gameObject, 0.2f);
        } else if (other.tag != "Player" && other.tag != "BodyPart" ) {
            SpawnHit();
            Destroy(gameObject, 0.2f);
        }
    }

    public void SpawnHit() {
        GameObject newExplosion = Instantiate(hitParticle, transform.position, transform.rotation);
    }
}
