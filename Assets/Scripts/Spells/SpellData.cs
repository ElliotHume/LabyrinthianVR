using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellData : MonoBehaviour
{
    public string name, type, damageType;
    public float spellRadius = 1f;

    public Vector3 GetDirection() {
        return transform.forward;
    }

    public GameObject GetObjectInPath() {
        RaycastHit raycastHit;
        if( Physics.SphereCast(transform.position+transform.forward, spellRadius, transform.forward, out raycastHit, 100f) ) {
            return raycastHit.transform.gameObject;
        }
        return null;
    }

    public bool WillHitObject(GameObject target) {
        if (type == "missile" || type == "projectile" ) {
            return target == GetObjectInPath();
        } else if (type == "AoE") {
            // Debug.Log("AoE check: "+ Vector3.Distance(target.transform.position, transform.position));
            return Vector3.Distance(target.transform.position, transform.position) <= spellRadius;
        } else if (type == "seeker") {
            return true;
        }

        return false;
    }
}
