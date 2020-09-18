using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject figure;
    public float bobbingHeight = 0.1f;
    public float bobbingSpeed = 0.1f;
    public GameObject[] enableObjects;
    
    Vector3 resetPosition;
    // Start is called before the first frame update
    void Start()
    {
        figure.SetActive(false);
        resetPosition = transform.position;

        foreach (GameObject go in enableObjects){
            go.SetActive(false);
        }
    }

    void Update() {
        // BOB UP AND DOWN

        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight + resetPosition.y;

        //set the object's Y to the new calculated Y
        transform.position = new Vector3(transform.position.x, newY, transform.position.z) ;
    }

    public void Enable() {
        figure.SetActive(true);
        foreach (GameObject go in enableObjects){
            go.SetActive(true);
        }
    }

    public void Disable() {
        figure.SetActive(false);
    }

}
