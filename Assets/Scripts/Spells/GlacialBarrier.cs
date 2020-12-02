using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlacialBarrier : MonoBehaviour
{
    public GameObject iceSpikesPrefab;
    public List<Transform> anchorPoints;

    // Start is called before the first frame update
    void Start() {
        foreach(Transform anchor in anchorPoints) {
            Instantiate(iceSpikesPrefab, anchor.position, anchor.rotation);
        }
    }
}
