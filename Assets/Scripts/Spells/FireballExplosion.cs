using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireballExplosion : MonoBehaviour
{
    public int damage = 0;
    public GameObject ownerGO;

    

    // Start is called before the first frame update
    public void Start() {
        Destroy(GetComponent<SphereCollider>(), 0.05f);
        Destroy(gameObject, 1f);
    }

    public void SetOwner(GameObject go, bool playerOwned) {
        ownerGO = go;
        if (playerOwned) GetComponent<SphereCollider>().radius += 1f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //Server call only, no call on client.
    //ServerCallback is similar to Server but doesn't generate a warning when called on client.
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Shield") {
            other.GetComponent<Shield>().Break();
            Destroy(GetComponent<SphereCollider>(), 0);
        } else if (other.tag == "Fireball_Interactable") {
            try {
                other.GetComponent<FireballInteractable>().Trigger();
                Destroy(GetComponent<SphereCollider>(), 0);
            } catch (System.Exception e) {
                print("error with hitting fireball interactable: "+e);
            }
            
        }
    }
}


