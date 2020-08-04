using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fizzle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += Vector3.up;
        Destroy(gameObject, 2f);
    }
}
