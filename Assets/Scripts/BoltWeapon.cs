using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltWeapon : CasterWeapon
{
    GameObject player;
    CharacterController playerController;

    void Awake() {
        GetPlayer();
    }

    

    public override void Fire(GameObject owner) {
        GameObject newSpell = Instantiate(spellPrefab, transform.position+(transform.forward*offsetForward), transform.rotation);

        Lightning lt = newSpell.GetComponent<Lightning>();
        if (lt != null) {
            lt.HitPlayer();
            lt.SetOwner(gameObject);
            lt.SetTarget(GetPlayerPos());
        }
    }

    Vector3 GetPlayerPos(){
        return player.transform.TransformPoint(playerController.center);
    }

    void GetPlayer() {
        player = GameObject.Find("XR Rig");
        playerController = player.GetComponent<CharacterController>();
    }
}
