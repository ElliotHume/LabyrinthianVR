using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterWeapon : MonoBehaviour
{
    public GameObject spellPrefab;
    public float animationTime = 0f, cooldown = 3f;
    public int castAnimationType = 0;

    public virtual void Fire(GameObject owner) {
        GameObject newSpell = Instantiate(spellPrefab, transform.position, transform.rotation);

        BasicProjectile bp = newSpell.GetComponent<BasicProjectile>();
        if (bp != null) {
            bp.SetOwner(owner);
            bp.HitPlayer();
            bp.TargetPlayer();
        }
    }
}
