using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fireball : MonoBehaviour
{

    private bool targetPlayer;
    public float damage=10f, speed;
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


    void OnTriggerEnter(Collider other) {
        if ((other.tag != "BodyPart" && other.tag != "Player") || canHitPlayer) {
            GameObject newExplosion = Instantiate(fireballExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }       
    }

}
