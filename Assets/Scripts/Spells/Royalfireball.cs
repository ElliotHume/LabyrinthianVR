using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Royalfireball : MonoBehaviour
{

    private Vector3 startPosition;
    private Vector3 endPosition;
    public float travelTime;
    private float startHeight;
    public float maxHeight;

    private float startTime;
    private Vector3 target;

    public GameObject royalFire;

    public GameObject ownerGO;
    private bool wasReflected = false;

    private float verticalSpeed;
    private float vertical;

    // Start is called before the first frame update
    public void Start()
    {
        startPosition = transform.position + Vector3.up;
        startTime = Time.time;
        startHeight = transform.position.y;

        StartCoroutine(TravelToDestination());
    }

    public void SetOwner( GameObject go) {
        //Debug.Log(connection);
        ownerGO = go;
    }

    public void SetTarget(Vector3 p) {
        endPosition = p;
    }

    IEnumerator TravelToDestination() {
        verticalSpeed = maxHeight;
        vertical = startHeight;
        while (true) {
            float currentTime = (Time.time - startTime) / travelTime;
            Vector3 horizontal = Vector3.Lerp(startPosition, endPosition, currentTime);
            vertical += verticalSpeed * Time.deltaTime;
            verticalSpeed -= maxHeight * (2.2f / travelTime) * Time.deltaTime;

            transform.position = new Vector3(horizontal.x, vertical, horizontal.z);

            /*
            if (currentTime > 1f) {
                GameObject newExplosion = Instantiate(fireballExplosion, transform.position, Quaternion.identity);
                NetworkServer.Spawn(newExplosion);
                Destroy(gameObject);
            }
            */

            yield return new WaitForEndOfFrame();
        }
    }

    void OnTriggerEnter(Collider other) {
        // Only hits the arena
        if (other.tag == "Arena") {
            SpawnExplosion();
        } else if (other.tag == "Shield") {
            other.GetComponent<Shield>().Break();
            Destroy(gameObject);
        } else if (other.tag == "ArcanePulse") {
            //doesn't reflect more than once... kind of a cop out
            if (wasReflected == true) {
                Destroy(gameObject);
            }

            startPosition = transform.position;
            startTime = Time.time;
            //startHeight = transform.position.y;
            verticalSpeed = maxHeight;

            endPosition = ownerGO.transform.position;
            wasReflected = true;
        }
    }

    public void SpawnExplosion() {
        GameObject newExplosion = Instantiate(royalFire, new Vector3(transform.position.x, 0f, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }

}
