using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fireball : MonoBehaviour
{

    private Vector3 startPosition;
    private Vector3 endPosition;
    public float travelTime;
    private float startHeight;
    public float maxHeight;

    private float startTime;
    private Vector3 target;

    public GameObject fireballExplosion;

    public GameObject ownerGO;
    // private bool wasReflected = false;
    public bool playerThrown; // TODO

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

    // public void Start () {
    //     startPosition = transform.position + Vector3.up;
    //     startTime = Time.time;
    //     startHeight = transform.position.y;

    //     StartCoroutine(TravelToDestination());
    // }

    public void SetTarget(Vector3 p) {
        endPosition = p;
    }

    IEnumerator TravelToDestination() {
        // verticalSpeed = maxHeight;
        // vertical = startHeight;
        while (true) {
            // float currentTime = (Time.time - startTime) / travelTime;
            // Vector3 horizontal = Vector3.Lerp(startPosition, endPosition, currentTime);
            // vertical += verticalSpeed * Time.deltaTime;
            // verticalSpeed -= maxHeight * (2f / travelTime) * Time.deltaTime;

            // transform.position = new Vector3(horizontal.x, vertical, horizontal.z);

            transform.position += transform.forward * Time.deltaTime * travelTime;
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
        // Should not hit the caster
        // This is pretty messy - reminder to clean up afterwards
        //print("ownerGO: " + ownerGO.ToString());
        print("Fireball hit: " + other.ToString());
        if (other.tag != "BodyPart" && other.tag != "Player") {
            //print(other.name);
            //print("explode here1");
            OfflineSpawnExplosion();
        }       
    }

    public void OfflineSpawnExplosion() {
        GameObject newExplosion = Instantiate(fireballExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
