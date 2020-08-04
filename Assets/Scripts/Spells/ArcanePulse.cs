using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArcanePulse : MonoBehaviour
{
	public float MaxDiameter = 2.5f;
	public float expansionTime;
	public GameObject owner;

	private Vector3 startScale;
	private float startTime;

    public void Start()
    {
        //startScale = transform.localScale;
        //startTime = Time.time;

        //StartCoroutine(Expand());
        Destroy(GetComponent<SphereCollider>(), 1.1f);
        Destroy(gameObject, 1.2f);
    }

    public void SetOwner(GameObject o) {
    	owner = o;
    }

    void OnTriggerEnter(Collider other) {
        //don't collide with ragdolls
        if (other.tag == "BodyPart" || other.tag == "Player") {

        } else if (other.tag == "Shield") {
            other.GetComponent<Shield>().Break();
        }
    }

    /*
    IEnumerator Expand() {
    	while (true) {
    		// transform.scale = new Vector3(tranform.scale.x + expansionSpeed);
            float currentTime = (Time.time - startTime) / expansionTime;
            transform.localScale = Vector3.Lerp(startScale, new Vector3(MaxDiameter,MaxDiameter,MaxDiameter), currentTime);;
            Debug.Log(transform.localScale);
            if (transform.localScale.x >= MaxDiameter) {
                // GameObject newExplosion = Instantiate(fireballExplosion, transform.position, Quaternion.identity);
                // NetworkServer.Spawn(newExplosion);
                Destroy(gameObject);
            }

            yield return new WaitForEndOfFrame();
        }
    }
    */
}
