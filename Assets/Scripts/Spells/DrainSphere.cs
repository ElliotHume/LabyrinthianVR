﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainSphere : MonoBehaviour
{
    public float duration, speed=1f, damagePerTick;
    public AudioSource hitSound;
    GameObject hand;
    Vector3 prevHandLocation = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    public void LinkCastingHand(GameObject castingHand) {
        hand = castingHand;
        prevHandLocation = hand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 handVelocity = (hand.transform.position - prevHandLocation) / Time.deltaTime;
        moveDirection += handVelocity/4f;

        transform.position += moveDirection * speed * Time.deltaTime;

        prevHandLocation = hand.transform.position;
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Enemy") {
            if (hitSound != null && !hitSound.isPlaying) hitSound.Play();
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage("planar", damagePerTick);
        } else if (other.tag == "Player") {
            if (hitSound != null && !hitSound.isPlaying) hitSound.Play();
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.WeaponHit(damagePerTick);
        }
    }
}