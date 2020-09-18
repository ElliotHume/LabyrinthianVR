using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    public Material freezeMaterial, baseMaterial;
    public float duration=0f;
    private Vector3 frozenPosition;

    Material[] baseMaterials, frozenMaterials;
    Renderer rend;

    //bool frozen = false;
    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        baseMaterials = rend.materials;

        frozenMaterials = new Material[rend.materials.Length];
        for (var j = 0; j < rend.materials.Length; j++) {
            frozenMaterials[j] = freezeMaterial;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (frozen) transform.position = frozenPosition;
    }

    public void Freeze() {
        Debug.Log("Freezing: "+gameObject);

        frozenPosition = transform.position;
        
        //gameObject.GetComponent<MeshRenderer>().material = freezeMaterial;

        // Change all materials to frozen, not just the first material
        rend.materials = frozenMaterials;

        if (duration > 0) StartCoroutine(Unfreeze());

        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null ) ps.Play();

        AudioSource asrce = GetComponent<AudioSource>();
        if (asrce != null) asrce.Play();

        
    }

    IEnumerator Unfreeze() {
        float currentTime = 0f;
        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        rend.materials = baseMaterials;
        //frozen = false;
    }
}
