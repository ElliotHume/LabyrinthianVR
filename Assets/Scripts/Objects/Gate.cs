using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float duration=1f;
    public bool lowered = false;

    public bool timed = false;
    public float timerDuration = 1f;

    Vector3 raisedPosition, loweredPosition;
    private bool doneMoving = true;
    
    // Start is called before the first frame update
    void Start()
    {
        raisedPosition = lowered ? (transform.position+(Vector3.up * transform.lossyScale.y)) : transform.position;
        loweredPosition = lowered ? transform.position : (transform.position+(Vector3.down * transform.lossyScale.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle() {
        if (doneMoving) {
            Debug.Log("Toggling gate: "+gameObject.name);
            doneMoving = false;
            if (lowered) {
                StartCoroutine(MoveToPosition(raisedPosition));
            } else {
                StartCoroutine(MoveToPosition(loweredPosition));
            }
            if (timed) StartCoroutine(TimeToggle(timerDuration));
        }
    }

    public void TimedToggle(float duration) {
        StartCoroutine(TimeToggle(duration));
    }

    public void Lower() {
        if (doneMoving) {
            Debug.Log("Lowering gate: "+gameObject.name);
            doneMoving = false;
            StartCoroutine(MoveToPosition(loweredPosition));
        }
    }

    public void Raise() {
        if (doneMoving) {
            Debug.Log("Raising gate: "+gameObject.name);
            doneMoving = false;
            StartCoroutine(MoveToPosition(raisedPosition));
        }
    }


    IEnumerator MoveToPosition(Vector3 position) {
        var currentPos = transform.position;
        var t = 0f;
        while(t < 1){
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return new WaitForFixedUpdate();
        }
        doneMoving = true;
        lowered = !lowered;
        AudioSource asrce = GetComponent<AudioSource>();
        if (asrce != null) asrce.Play();
    }

    IEnumerator TimeToggle(float duration) {
        yield return new WaitForSeconds(duration);
        if (doneMoving) {
            Debug.Log("Toggling gate: "+gameObject.name);
            doneMoving = false;
            if (lowered) {
                StartCoroutine(MoveToPosition(raisedPosition));
            } else {
                StartCoroutine(MoveToPosition(loweredPosition));
            }
        }
    }
}
