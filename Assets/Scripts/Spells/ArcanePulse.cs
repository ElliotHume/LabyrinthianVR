using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArcanePulse : MonoBehaviour
{
	public float MaxDiameter = 2.5f;
	public float expansionTime;
	public GameObject owner;
    public AudioSource hitsound;

	private Vector3 startScale;
	private float startTime;

    public void Start()
    {
        Destroy(GetComponent<SphereCollider>(), 1.1f);
        Destroy(gameObject, 1.2f);
    }

    public void SetOwner(GameObject o) {
    	owner = o;
    }

    void OnTriggerEnter(Collider other) {
        //don't collide with ragdolls
        if (other.tag == "Shield") {
            other.GetComponent<Shield>().Break();
            if (hitsound != null) hitsound.Play();
        } else if (other.tag == "Magic" || other.gameObject.layer == 11) {
            Destroy(other.gameObject, 0.2f);
            if (hitsound != null) hitsound.Play();
        }
    }
}
