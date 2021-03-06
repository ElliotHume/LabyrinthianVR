﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindSlash : MonoBehaviour
{
    public int damage = 20;
    public string damageType = "wind";
    public float speed = 25f;
    public float duration = 1f;

    public Vector3 direction;

    public GameObject owner;
    private GameObject hand;
    private CharacterController characterController;

    public GameObject hitParticle;

    // Start is called before the first frame update
    void Start() {
        Destroy(GetComponent<BoxCollider>(), duration);
        Destroy(gameObject, duration);
        int random = Random.Range(0, 5);
        //print(random);
    }

    public void SetDirection(Vector3 g) {
        direction = new Vector3(g.x, 0, g.z).normalized;
    }

    public void SetOwner(GameObject originHand, GameObject player) {
        hand = originHand;
        owner = player;
        characterController = owner.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Should counter gravity if airborne so that the player can cross gaps
        Vector3 gravity = characterController.isGrounded ? Vector3.zero : new Vector3(0, (Physics.gravity.y/2f) * Time.deltaTime, 0);

        if (characterController != null) {
            characterController.Move((direction * Time.deltaTime * speed) - gravity );
        } else {
            owner.transform.position += (direction * Time.deltaTime * speed) - gravity ;
        }
        
        transform.position = hand.transform.position;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spell_Interactable") {
            SpellInteractable si = other.GetComponent<SpellInteractable>();
            if (si != null) si.Trigger("windslash");
            SpawnHit();
            Destroy(gameObject);
        } else if (other.tag == "Enemy") {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            CasterAI caster = other.GetComponent<CasterAI>();
            if (enemy != null) enemy.TakeDamage(damageType, damage);
            if (caster != null) caster.TakeDamage(damageType, damage);
        }
    }

    public void SpawnHit() {
        GameObject newExplosion = Instantiate(hitParticle, transform.position, transform.rotation);
    }
}
