using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farseer : MonoBehaviour
{
    public float speed = 20f;
    public float duration = 3f;

    public GameObject owner, hand;
    private CharacterController characterController;

    Vector3 startPos;

    bool stay = false, moveDown = false;

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, duration*5);
        
        StartCoroutine(SectionTiming());
    }

    public void SetOwner(GameObject player, GameObject castingHand) {
        owner = player;
        characterController = owner.GetComponent<CharacterController>();
        startPos = player.transform.position;
        hand = castingHand;
    }

    // Update is called once per frame
    void Update()
    {
        // Should counter gravity
        Vector3 gravity = new Vector3(0, Physics.gravity.y * Time.deltaTime, 0);
        Vector3 direction = moveDown ? Vector3.down : stay ? Vector3.zero : Vector3.up;

        if (characterController != null) {
            characterController.Move((direction * Time.deltaTime * speed) - gravity);
        }
        
        owner.transform.position = new Vector3(startPos.x, owner.transform.position.y, startPos.z);
        transform.position = hand.transform.position+Vector3.down;
    }

    IEnumerator SectionTiming() {
        yield return new WaitForSeconds(duration);
        stay = true;
        yield return new WaitForSeconds(duration*3);
        moveDown = true;
    }
}
