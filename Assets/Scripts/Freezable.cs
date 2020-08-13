using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    public Material freezeMaterial, baseMaterial;
    public float duration=0f;
    private Vector3 frozenPosition;
    bool frozen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (frozen) transform.position = frozenPosition;
    }

    public void Freeze() {
        frozenPosition = transform.position;
        frozen = true;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null ) ps.Play();

        AudioSource asrce = GetComponent<AudioSource>();
        if (asrce != null) asrce.Play();

        gameObject.GetComponent<MeshRenderer>().material = freezeMaterial;
        if (duration > 0) StartCoroutine(Unfreeze());
    }

    IEnumerator Unfreeze() {
        float currentTime = 0f;
        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        gameObject.GetComponent<MeshRenderer>().material = baseMaterial;
        frozen = false;
    }
}
