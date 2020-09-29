using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shield : MonoBehaviour
{
    public int health = 1;
    public bool reflectsSpells = false;
    private Vector3 finalPosition;
    private Vector3 startPosition;

    public GameObject owner;

    public AudioClip emergeClip;
    public AudioClip breakClip;

    // Start is called before the first frame update
    void Start() {
        finalPosition = transform.position;
        transform.position += Vector3.down * 4f;
        startPosition = transform.position;
        StartCoroutine(RiseUp());

        GetComponent<AudioSource>().clip = emergeClip;
        GetComponent<AudioSource>().Play();
    }

    IEnumerator RiseUp() {
        float duration = 0.3f;
        float currentTime = 0f;
        while (currentTime < duration) {
            float lerp = currentTime / duration;
            float vertical = Mathf.Sin(lerp * Mathf.PI * 0.5f) * 4f;
            transform.position = startPosition + Vector3.up * vertical;
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.position = finalPosition;
    }

    public void Break() {
        Destroy(GetComponent<MeshCollider>());
        GetComponent<AudioSource>().clip = breakClip;
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 1f);
    }
}
