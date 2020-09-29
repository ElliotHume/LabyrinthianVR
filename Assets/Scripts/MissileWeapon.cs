using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : Weapon
{
    Transform target;
    public GameObject missile;

    public override void Attack() {
        if (swingSound != null) swingSound.Play();
        Invoke(nameof(ShootMissile), attackDuration);
    }

    void ShootMissile() {
        GameObject newMissile = Instantiate(missile, transform.position, transform.rotation);

        MagicMissile m = newMissile.GetComponent<MagicMissile>();
        if  (m != null) {
            m.HitPlayer();
            m.TargetPlayer();
        }

        Fireball f = newMissile.GetComponent<Fireball>();
        if  (f != null) {
            print("Targetting player");
            f.HitPlayer();
            f.TargetPlayer();
        }
    }
}
