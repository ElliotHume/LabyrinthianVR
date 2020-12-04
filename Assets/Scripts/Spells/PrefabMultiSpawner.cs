using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMultiSpawner : MonoBehaviour
{
    public GameObject prefab;
    public List<Transform> anchorPoints;

    // Start is called before the first frame update
    void Start() {
        foreach(Transform anchor in anchorPoints) {
            Instantiate(prefab, anchor.position, anchor.rotation);
        }
        Destroy(gameObject, 2f);
    }
}
