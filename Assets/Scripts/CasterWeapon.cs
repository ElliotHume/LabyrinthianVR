using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterWeapon : MonoBehaviour
{
    public GameObject spellPrefab, windupParticles;
    public float animationTime = 0f, cooldown = 3f;
    public int castAnimationType = 0;
    public bool targeted = true;
    public float offsetForward = 0f;

    public virtual void Fire(GameObject owner) {
        GameObject newSpell = Instantiate(spellPrefab, transform.position+(transform.forward*offsetForward), transform.rotation);

        BasicProjectile bp = newSpell.GetComponent<BasicProjectile>();
        if (bp != null) {
            bp.SetOwner(owner);
            bp.HitPlayer();
            if (targeted) {
                bp.TargetPlayer();
            }
        }
    }

    public virtual void PlayWindupParticles() {
        if (windupParticles != null) Instantiate(windupParticles, transform.position+(transform.forward*offsetForward), transform.rotation);
    }
}
