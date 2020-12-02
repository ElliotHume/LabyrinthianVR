using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    public float travelTime;
    public float maxHeight;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float startHeight;
    private float startTime;


    // Start is called before the first frame update
    void Awake() {
        startPosition = transform.position;
        startHeight = transform.position.y;
        endPosition = transform.position + transform.forward * 10f;
    }

    IEnumerator TravelToDestination() {
        float currentTime = 0;
        float verticalSpeed = maxHeight;
        float vertical = startHeight;
        while (currentTime < travelTime) {
            Vector3 horizontal = Vector3.Lerp(startPosition, endPosition, currentTime/travelTime);
            vertical += verticalSpeed * Time.deltaTime;
            verticalSpeed -= maxHeight * (2.2f / travelTime) * Time.deltaTime;

            transform.position = new Vector3(horizontal.x, vertical, horizontal.z);

            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        print("At End position: "+transform.position);
        enabled = false;
    }

    public void SetTarget(Vector3 pos) {
        endPosition = pos;
        // Debug.Log("New target set for thrown object: "+gameObject+"      to position: "+endPosition);
        StartCoroutine(TravelToDestination());
    }
}
