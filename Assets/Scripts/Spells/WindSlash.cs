using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindSlash : MonoBehaviour
{
    public int damage = 1;

    public Vector3 target;

    public GameObject owner;

    public GameObject hitParticle;

    // Start is called before the first frame update
    void Start() {
        Destroy(GetComponent<BoxCollider>(), 0.6f);
        Destroy(gameObject, 1f);
        int random = Random.Range(0, 5);
        //print(random);
        // GetComponents<AudioSource>()[random].Play();
    }

    public void SetTarget(Vector3 g) {
        target = g;
    }

    public void SetOwner(GameObject g) {
        owner = g;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = owner.transform.position + (transform.forward * 2f) + Vector3.up;
    }

    void OnTriggerStay(Collider other) {
        if (owner != null && target != null) {
            if (other.gameObject.transform.position == target) {
                //Debug.Log("testing  GO: "+other.gameObject+"     owner: "+owner);
                // other.GetComponent<CharacterBehaviour>().TakeDamage(damage);
                // other.GetComponent<CharacterBehaviour>().TargetShowDamageEffects(other.GetComponent<NetworkIdentity>().connectionToClient);
                // owner.GetComponent<CharacterBehaviour>().TargetThrowPlayerBack(owner.GetComponent<NetworkIdentity>().connectionToClient, 0.8f, 2, 40);
                // owner.GetComponent<CharacterBehaviour>().TargetSetAnimTrigger(owner.GetComponent<NetworkIdentity>().connectionToClient, "WindSlashRecoil");
                SpawnHit();
                Destroy(gameObject);
            } else if (other.tag == "Shield") {
                other.GetComponent<Shield>().Break();
                // owner.GetComponent<CharacterBehaviour>().TargetThrowPlayerBack(owner.GetComponent<NetworkIdentity>().connectionToClient, 0.4f, 2, 40);
                // owner.GetComponent<CharacterBehaviour>().TargetSetAnimTrigger(owner.GetComponent<NetworkIdentity>().connectionToClient, "WindSlashRecoil");
                SpawnHit();
                Destroy(gameObject);
            }
        }
    }

    public void SpawnHit() {
        GameObject newExplosion = Instantiate(hitParticle, transform.position, transform.rotation);
    }
}
