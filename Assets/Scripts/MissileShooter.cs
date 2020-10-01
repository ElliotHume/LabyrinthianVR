using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileShooter : MonoBehaviour
{
    public float cooldown = 2f;
    public bool active;

    public GameObject magicMissile, spawnPoint;
    // Start is called before the first frame update
    /* test */
    void Start()
    {
        if (active) StartCoroutine(Shoot());
    }

    public void Toggle() {
        active = !active;
    }

    public void ShootOnce() {
        GameObject newMagicMissile = Instantiate(magicMissile, spawnPoint.transform.position, spawnPoint.transform.rotation);
        MagicMissile mm = newMagicMissile.GetComponent<MagicMissile>();
        mm.HitPlayer();
        mm.TargetPlayer();
    }

    public IEnumerator Shoot() {
        while (true) {
            if (active) ShootOnce();
            yield return new WaitForSeconds(cooldown);
        }
    }
}
