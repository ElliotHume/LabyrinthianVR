using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellComponentDisplayPanel : MonoBehaviour
{
    public ComponentDisplay componentDisplay;
    public Transform anchor;
    void OnCollisionEnter(Collision collision) {
        //Debug.Log("ENTER: "+collision.gameObject);
        if (collision.gameObject.tag == "SpellComponent") {
            ProcessSpellComponent(collision.gameObject);
        }
    }

    void ProcessSpellComponent(GameObject componentGO) {
        SpellComponent sc = componentGO.GetComponent<SpellComponent>();
        componentDisplay.spellComponent = sc.component;
        componentDisplay.Rebuild();

        componentGO.transform.position = anchor.position;
        componentGO.GetComponent<Rigidbody>().useGravity = false;
    }
}
