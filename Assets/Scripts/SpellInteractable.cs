using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellInteractable : MonoBehaviour
{
    public string triggerSpell;
    public List<GameObject> toggleActiveGameObjects;
    public List<GameObject> lowerGameObjects;
    public List<GameObject> raiseGameObjects;
    public List<GameObject> slideGameObjects;

    public float lowerDuration=5f, raiseDuration = 5f, slideDuration = 5f;
    public float displacementScale = 1f;

    public Vector3 slideVector = Vector3.zero;

    public UnityEvent OnPress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Trigger(string spell) {
        if (spell == triggerSpell) {
            Debug.Log(gameObject.name+" triggered by "+spell);
            ToggleGameObjects();
            LowerGameObjects();
            RaiseGameObjects();
            try {
                GetComponent<AudioSource>().Play();
            } catch {
                Debug.Log("No audio source");
            }
            OnPress.Invoke();
        }
    }

    void ToggleGameObjects() {
        if (toggleActiveGameObjects.Count > 0 ) {
            Debug.Log(gameObject.name+" toggling objects: "+toggleActiveGameObjects.Count);
            foreach( GameObject go in toggleActiveGameObjects ) {
                go.SetActive(!go.activeInHierarchy);
            }
        } 
    }

    void LowerGameObjects() {
        if (lowerGameObjects.Count > 0 ) {
            Debug.Log(gameObject.name+" lowering objects: "+lowerGameObjects.Count);
            StartCoroutine(Lower());
        } 
    }

    void RaiseGameObjects() {
        if (raiseGameObjects.Count > 0 ) {
            Debug.Log(gameObject.name+" raising objects: "+raiseGameObjects.Count);
            StartCoroutine(Raise());
        }
    }

    void SlideGameObjects() {
        if (slideGameObjects.Count > 0 ) {
            Debug.Log(gameObject.name+" sliding objects: "+slideGameObjects.Count);
            StartCoroutine(Slide());
        }
    }

    IEnumerator Raise() {
        float currentTime = 0f;
        while (currentTime < raiseDuration) {
            float lerp = currentTime / raiseDuration;
            foreach( GameObject go in raiseGameObjects ) {
                float step = lerp * Time.deltaTime;
                go.transform.position = Vector3.MoveTowards(go.transform.position, (go.transform.position+(Vector3.up * displacementScale)), step);
                // float vertical = Mathf.Sin(lerp * Mathf.PI * 0.5f);
                // go.transform.position += Vector3.down * vertical;
            }
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Lower() {
        float currentTime = 0f;
        while (currentTime < lowerDuration) {
            float lerp = currentTime / lowerDuration;
            foreach( GameObject go in lowerGameObjects ) {
                float step = lerp * Time.deltaTime;
                go.transform.position = Vector3.MoveTowards(go.transform.position, (go.transform.position+(Vector3.down * displacementScale)), step);
                // float vertical = Mathf.Sin(lerp * Mathf.PI * 0.5f);
                // go.transform.position += Vector3.down * vertical;
            }
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Slide() {
        float currentTime = 0.01f;
        while (currentTime < slideDuration) {
            float lerp = Mathf.Clamp((slideDuration / currentTime) - 0.99f, 0, 1);
            foreach( GameObject go in slideGameObjects ) {
                float step = lerp * Time.deltaTime;
                go.transform.position = Vector3.MoveTowards(go.transform.position, (go.transform.position+(slideVector * displacementScale)), step);
                // float vertical = Mathf.Sin(lerp * Mathf.PI * 0.5f);
                // go.transform.position += Vector3.down * vertical;
            }
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
