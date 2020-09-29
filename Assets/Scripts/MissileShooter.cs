using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileShooter : MonoBehaviour
{
    public float frequency = 2f;
    public bool active;

    public GameObject magicMissile, spawnPoint, playerTeleportAnchor;
    // Start is called before the first frame update
    /* test */
    void Start()
    {
        if (active) StartCoroutine(Shoot());
        if (playerTeleportAnchor == null) playerTeleportAnchor = GameObject.Find("SpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle() {
        active = !active;
    }

    public void ShootOnce() {
        GameObject newMagicMissile = Instantiate(magicMissile, spawnPoint.transform.position, spawnPoint.transform.rotation);
        MagicMissile mm = newMagicMissile.GetComponent<MagicMissile>();
        mm.HitPlayer();
    }

    public IEnumerator Shoot() {
        while (active) {
            ShootOnce();
            yield return new WaitForSeconds(frequency);
        }
    }
}
