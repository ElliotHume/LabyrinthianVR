using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellData : MonoBehaviour
{
    public string name, type, damagetype;

    public Vector3 GetDirection() {
        return transform.forward;
    }

    public GameObject GetObjectInPath() {
        RaycastHit raycastHit;
        if( Physics.SphereCast(transform.position+transform.forward, 1f, transform.forward, out raycastHit, 100f) ) {
            return raycastHit.transform.gameObject;
        }
        return null;
    }

    public bool WillHitObject(GameObject target) {
        if (type == "missile" || type == "projectile" ) {
            return target == GetObjectInPath();
        } else if (type == "seeker") {
            return true;
        }

        return false;
    }
}
