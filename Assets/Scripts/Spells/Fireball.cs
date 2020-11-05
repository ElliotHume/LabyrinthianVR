using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fireball : MonoBehaviour
{

    private bool targetPlayer;
    public float speed;
    public bool canHitPlayer = false;
    GameObject player;
    CharacterController playerController;
    public GameObject fireballExplosion;

    void Update() {
        if (targetPlayer && playerController != null) {
            Vector3 playerPos = player.transform.TransformPoint(playerController.center);
            transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
            //transform.LookAt(targetPosition.position+Vector3.up);
        } else {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }

    public void TargetPlayer() {
        targetPlayer = true;
    }

    public void HitPlayer() {
        canHitPlayer = true;
        player = GameObject.Find("XR Rig");
        playerController = player.GetComponent<CharacterController>();
    }

    public void Scale(float scale){
        float scaleFactor = 0.4f + 0.8f*scale;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag !=  "NullZone" && other.tag != "Ghost" && ((other.tag != "BodyPart" && other.tag != "Player" && other.tag != "Weapon") || canHitPlayer)) {
            GameObject newExplosion = Instantiate(fireballExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }       
    }

}
