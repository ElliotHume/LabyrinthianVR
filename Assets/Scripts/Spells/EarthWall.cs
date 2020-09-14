using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : MonoBehaviour
{
    public float explosionForce = 1f, explosionRadius = 0.25f, riseDuration, lifetime;


    Vector3 raisedPosition;

    // Start is called before the first frame update
    void Start()
    {
        raisedPosition = transform.position;
        transform.position = transform.position+(Vector3.down * transform.lossyScale.y);

        StartCoroutine(MoveToPosition(raisedPosition));
        if (lifetime > 0) Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shatter(ContactPoint cp) {
        print("Shatter, with ContactPoint");
        GetComponent<Rigidbody>().isKinematic = false;
        foreach(Transform child in transform) {
            GameObject go = child.gameObject;
            go.GetComponent<Rigidbody>().isKinematic = false;
            go.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, cp.point, explosionRadius);
            Destroy(go, (lifetime+10)/3f);
        }
    }

    public void Shatter() {
        print("Shatter");
        GetComponent<Rigidbody>().isKinematic = false;
        foreach(Transform child in transform) {
            GameObject go = child.gameObject;
            go.GetComponent<Rigidbody>().isKinematic = false;
            go.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
            Destroy(go, (lifetime+10)/3f);
        }
    }

    IEnumerator MoveToPosition(Vector3 position) {
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1){
            t += Time.deltaTime / riseDuration;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return new WaitForFixedUpdate();
        }
    }
}
