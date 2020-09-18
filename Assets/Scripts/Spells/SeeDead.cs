using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeDead : MonoBehaviour
{
    public float duration = 15f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnableGhosts());
    }

    IEnumerator EnableGhosts() {
        Ghost[] ghosts = GameObject.FindObjectsOfType<Ghost>();
        foreach (Ghost ghost in ghosts) {
            ghost.Enable();
        }
        yield return new WaitForSeconds(duration);
        foreach (Ghost ghost in ghosts) {
            ghost.Disable();
        }
        Destroy(gameObject, 1);
    }
}
