using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSnapbackObject : MonoBehaviour
{

    public float timeToSnapback;
    Vector3 startPos;
    bool interrupt = false;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    public void StartTimedSnapback() {
        StartCoroutine(Snapback());
    }

    public void Interrupt(){
        interrupt = true;
    }

    IEnumerator Snapback() {
        yield return new WaitForSeconds(timeToSnapback);
        if (!interrupt) transform.position = startPos;
        interrupt = false;
        
        AudioSource asrce = GetComponent<AudioSource>();
        if (asrce != null ) asrce.Play();
    }
}
