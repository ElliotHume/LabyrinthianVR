using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreaker : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spell_Interactable") {
            Breakable b = other.gameObject.GetComponent<Breakable>();
            if (b != null) b.Break();
        }
    }
}
