using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballInteractable : MonoBehaviour
{
    public List<GameObject> toggleActiveGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Trigger() {
        toggleGameObject();
    }

    public void toggleGameObject() {
        if (toggleActiveGameObjects.Count > 0 ) {
            Debug.Log(gameObject.name+" toggling objects: "+toggleActiveGameObjects.Count);
            foreach( GameObject go in toggleActiveGameObjects ) {
                go.SetActive(!go.activeInHierarchy);
            }
        } 
    }
}
