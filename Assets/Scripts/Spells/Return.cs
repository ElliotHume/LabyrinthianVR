using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{
    public float duration = 8f;

    public GameObject owner, hand;
    public AudioSource returnSound;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, duration+2f);
        StartCoroutine(Timer());
    }

    public void SetOwner(GameObject player, GameObject castingHand) {
        owner = player;
        startPos = player.transform.position;
        hand = castingHand;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = hand.transform.position;
        transform.rotation = hand.transform.rotation;
    }

    IEnumerator Timer() {
        yield return new WaitForSeconds(duration);
        owner.transform.position = startPos;
        if (returnSound) returnSound.Play();
    }
}
