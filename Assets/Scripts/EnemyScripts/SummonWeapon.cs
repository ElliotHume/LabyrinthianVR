using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : CasterWeapon
{
    public AudioSource summonSound;
    GameObject player;
    CharacterController playerController;
    Vector3 playerPos = Vector3.zero;

    public override void Fire(GameObject owner) {
        if (player == null || playerController == null) GetPlayer();
        playerPos = player.transform.TransformPoint(playerController.center);

        RaycastHit raycastHit;
        Vector3 target = (playerPos + transform.position) / 2f;
        if( Physics.Raycast( target, Vector3.down, out raycastHit, 50f, LayerMask.GetMask("Ground") ) ) {
            print("Snapping to position: "+raycastHit.point+"   object: "+ raycastHit.collider.gameObject);
            target = raycastHit.point;
        }


        if (summonSound != null) summonSound.Play();
        GameObject newSummon = Instantiate(spellPrefab, target, transform.rotation);
    }

    void GetPlayer(){
        player = GameObject.Find("XR Rig");
        playerController = player.GetComponent<CharacterController>();
        playerPos = player.transform.TransformPoint(playerController.center);
    }
}
