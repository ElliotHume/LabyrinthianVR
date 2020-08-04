using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IceSpike : MonoBehaviour
{
    public int damage = 1;

    // Start is called before the first frame update
    public void Start()
    {
        Destroy(GetComponent<CapsuleCollider>(), 6f);
        Destroy(gameObject, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            // other.GetComponent<CharacterBehaviour>().TakeDamage(damage);
            // other.GetComponent<CharacterBehaviour>().TargetShowDamageEffects(other.GetComponent<NetworkIdentity>().connectionToClient);
            // other.GetComponent<CharacterBehaviour>().TargetThrowPlayerBack(other.GetComponent<NetworkIdentity>().connectionToClient, 0.6f, 0, 40);
        }
    }
}
