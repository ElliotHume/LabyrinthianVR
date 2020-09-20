using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IceSpikeProjectile : MonoBehaviour
{

    public GameObject iceSpike;
    public float damage=30f, speed;
    public float spikeDistance;

    public float maxDistanceFromCenter = 28f;

    private Vector3 startPosition;
    private float distanceTravelled;
    private float nextSpikeDistance;

    // Start is called before the first frame update
    public void Start()
    {
        distanceTravelled = 0f;
        nextSpikeDistance = spikeDistance;
        startPosition = transform.position;
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        float newDistance = speed * Time.deltaTime;
        transform.position += transform.forward * newDistance;
        distanceTravelled += newDistance;

        //transform.rotation *= Quaternion.Euler(transform.forward * 90f * Time.deltaTime);

        if (distanceTravelled >= nextSpikeDistance) {
            GameObject newIceSpike = Instantiate(iceSpike, startPosition + (transform.forward * (nextSpikeDistance - 1f)) - Vector3.up, transform.rotation);
            nextSpikeDistance += spikeDistance;
        }

        //check distance from center if it's out of bounds
        if (Vector3.Distance(transform.position, new Vector3(0f, transform.position.y, 0f)) > maxDistanceFromCenter) {
            Destroy(gameObject);
        }
    }

    // void OnTriggerEnter(Collider other) {
    //     if (other.tag == "Shield") {
    //         other.GetComponent<Shield>().Break();
    //         Destroy(gameObject);
    //     }
    //     else if (other.tag == "Enemy") {
    //         EnemyAI enemy = other.GetComponent<EnemyAI>();
    //         if (enemy != null) enemy.TakeDamage(damage);
    //     }
    // }
}
