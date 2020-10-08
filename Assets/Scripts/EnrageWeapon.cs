using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnrageWeapon : Weapon
{
    Transform target;
    public GameObject prefab;
    public List<Transform> anchors;

    public float fireDelay = 0.1f;

    public override void Attack() {
        if (swingSound != null) swingSound.Play();
        StartCoroutine(FireWeapon());

        if (anchors.Count == 0) anchors.Add(transform);
    }

    IEnumerator FireWeapon() {
        yield return new WaitForSeconds(attackDuration);
        foreach (Transform anchor in anchors)
        {
            GameObject newPrefab = Instantiate(prefab, anchor.position, anchor.rotation);

            MagicMissile m = newPrefab.GetComponent<MagicMissile>();
            if  (m != null) {
                m.HitPlayer();
                m.TargetPlayer();
            }

            Fireball f = newPrefab.GetComponent<Fireball>();
            if  (f != null) {
                f.HitPlayer();
                f.TargetPlayer();
            }

            yield return new WaitForSeconds(fireDelay);
        }        
    }
}
