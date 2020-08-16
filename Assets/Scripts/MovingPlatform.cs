using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform anchor1, anchor2;
    public float travelTime;
    public bool waitAtAnchor;
    public float waitDuration;

    bool frozen = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveToAnchor1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Freeze(){
        if (!frozen) {
            frozen = true;
            try {
                StopAllCoroutines();
            } catch {
                print("Failed to stop moving");
            }

            StartCoroutine(UnFreeze());

            ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in particles) {
                ps.Play();
            }
            Debug.Log("Freeze platform: "+gameObject);
        }
    }

    IEnumerator UnFreeze() {
        yield return new WaitForSeconds(15);
        Debug.Log("Unfreeze platform: "+gameObject);
        frozen = false;
        StartCoroutine(MoveToAnchor1());
    }

    IEnumerator MoveToAnchor1() {
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1){
            t += Time.deltaTime / travelTime;
            transform.position = Vector3.Lerp(currentPos, anchor1.position, t);
            yield return new WaitForFixedUpdate();
        }


        AudioSource asrce = GetComponent<AudioSource>();
        if (asrce != null) asrce.Play();

        if (waitAtAnchor) yield return new WaitForSeconds(waitDuration);

        StartCoroutine(MoveToAnchor2());
    }

    IEnumerator MoveToAnchor2() {
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1){
            t += Time.deltaTime / travelTime;
            transform.position = Vector3.Lerp(currentPos, anchor2.position, t);
            yield return new WaitForFixedUpdate();
        }

        
        AudioSource asrce = GetComponent<AudioSource>();
        if (asrce != null) asrce.Play();

        if (waitAtAnchor) yield return new WaitForSeconds(waitDuration);

        StartCoroutine(MoveToAnchor1());
    }
}
