using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    public GameObject ldoor, rdoor;
    public float openDuration = 1f;
    public bool opened = false;
    public AudioSource openSound, closeSound;
    public Collider dcoll = null;

    bool moving = false;
    Quaternion startRotation, lEndRotation, rEndRotation;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = Quaternion.Euler(0, 0, 0);
        lEndRotation = Quaternion.Euler(0, -90, 0);
        rEndRotation = Quaternion.Euler(0, 90, 0);
    }

    public void Open() {
        if (!opened && !moving) StartCoroutine(OpenDoors());
    }

    public void Close() {
        if (opened && !moving) StartCoroutine(CloseDoors());
    }

    public void Toggle() {
        // Calling both will toggle the state, as only one function will run
        Open();
        Close();
    }

    IEnumerator OpenDoors() {
        //print("opening");
        moving = true;
        var t = 0f;
        if (openSound != null) openSound.Play();
        if (dcoll != null) dcoll.enabled = false;
        while(t < 1f){
            t += Time.deltaTime / openDuration;
            ldoor.transform.localRotation = Quaternion.Lerp(ldoor.transform.localRotation, lEndRotation, t);
            rdoor.transform.localRotation = Quaternion.Lerp(rdoor.transform.localRotation, rEndRotation, t);
            //print(ldoor.transform.localRotation.eulerAngles+ "      "+lEndRotation.eulerAngles+"        "+t);
            yield return new WaitForFixedUpdate();
        }
        //print("done opening");
        opened = true;
        moving = false;
    }

    IEnumerator CloseDoors(){
        //print("closing");
        moving = true;
        var t = 0f;
        if (closeSound != null) closeSound.Play();
        if (dcoll != null) dcoll.enabled = true;
        while(t < 1f){
            t += Time.deltaTime / openDuration;
            ldoor.transform.rotation = Quaternion.Lerp(ldoor.transform.rotation, startRotation, t);
            rdoor.transform.rotation = Quaternion.Lerp(rdoor.transform.rotation, startRotation, t);
            yield return new WaitForFixedUpdate();
        }
        opened = false;
        moving = false;
    }

}
