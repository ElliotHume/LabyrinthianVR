using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    bool active = true;
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
            active = false;
            GetComponent<Rigidbody>().detectCollisions = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            SpellInteractable s = other.gameObject.GetComponent<SpellInteractable>();
            if (s != null ) s.Trigger("marker");
        }
    }
}
