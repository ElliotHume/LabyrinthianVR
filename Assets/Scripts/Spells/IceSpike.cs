using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IceSpike : MonoBehaviour
{
    public int damage = 30;

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
        if (other.tag == "Enemy") {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage(damage);
        }
    }
}
