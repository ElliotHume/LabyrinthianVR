﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoyalFlame : MonoBehaviour
{
    public float royalBurnRate = 0.1f;

    // Start is called before the first frame update
    public void Start()
    {
        Destroy(GetComponent<CapsuleCollider>(), 10f);
        Destroy(gameObject, 14f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other) {
        // if (other.tag == "Player" && other.GetComponent<CharacterBehaviour>().health > 0) {
        //     other.GetComponent<CharacterBehaviour>().royalBurn += royalBurnRate * Time.deltaTime;
        //     if (other.GetComponent<CharacterBehaviour>().royalBurn >= 1f) {
        //         RpcPlaySound();
        //         other.GetComponent<CharacterBehaviour>().TakeDamage(1);
        //         other.GetComponent<CharacterBehaviour>().TargetShowDamageEffects(other.GetComponent<NetworkIdentity>().connectionToClient);
        //         other.GetComponent<CharacterBehaviour>().royalBurn = 0f;
        //     }
        // } else if (other.tag == "ArcanePulse") {
        //     Destroy(gameObject, 2f);
        // }
    }

    // void RpcPlaySound() {
    //     GetComponents<AudioSource>()[1].Play();
    // }
}