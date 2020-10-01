using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    bool active;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active) transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag != "Player"){
            transform.parent = other.gameObject.transform;
            active = false;
        }
    }
}
