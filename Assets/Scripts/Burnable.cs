using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public Material burnMaterial;
    public float duration=5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Burn() {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null ) ps.Play();
        foreach(Transform child in transform) {
            child.gameObject.GetComponent<MeshRenderer>().material = burnMaterial;
        }
        Destroy(gameObject, duration);
    }
}
