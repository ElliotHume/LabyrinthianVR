using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidasTouch : MonoBehaviour
{
    public Material goldMaterial;
    public AudioSource hitSound;
    public float duration = 5f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter(Collider other) {
        print("Midas touch: "+other.gameObject);
        if (other.tag == "Arena") return;
        Renderer rend = other.gameObject.GetComponent<Renderer>();

        // Change all materials to gold, not just the first material
        var mats = new Material[rend.materials.Length];
        for (var j = 0; j < rend.materials.Length; j++)
        {
            mats[j] = goldMaterial;
        }
        rend.materials = mats;

        //if (rend != null) rend.material = goldMaterial;

        if (other.tag == "Spell_Interactable") {
            SpellInteractable si = other.gameObject.GetComponent<SpellInteractable>();
            if (hitSound) hitSound.Play();
            if (si != null) si.Trigger("midastouch");
        }
    }
}
