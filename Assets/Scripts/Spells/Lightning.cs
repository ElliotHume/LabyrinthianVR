using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Lightning : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject lightningExplosion;
    public GameObject owner;
    public Vector3 target;
    public bool canHitPlayer = false;
    public int damage = 10;

    Vector3 startPos, endPos;
    bool fired=false;

    void Update() {
        if (!fired && owner != null && endPos != null)
            Fire();
    }

    void Fire()
    {
        fired = true;
        startPos = owner.transform.position;
        Vector3 direction = target - startPos;

        Debug.DrawRay(startPos, direction, Color.red, 5f);

        RaycastHit[] rayhits = Physics.RaycastAll(startPos, direction, 100f);
        IEnumerable<RaycastHit> rayhitsOrdered = rayhits.OrderBy(rh => rh.distance);

        foreach (RaycastHit rh in rayhitsOrdered) {
            if ((rh.collider.tag == "Player" || rh.collider.tag == "BodyPart") && canHitPlayer) {
                GameObject player = GameObject.Find("XR Rig");
                CharacterController playerController = player.GetComponent<CharacterController>();
                endPos = player.transform.TransformPoint(playerController.center);
                break;
            }
            else if (rh.collider.tag != "Ghost"){
                endPos = rh.point;
                break;
            }
        }

        // Set line positions for client
        lineRenderer.positionCount = 9;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(8, endPos);

        // Add zigzags to line
        lineRenderer.SetPosition(1, Vector3.Lerp(startPos, endPos, 0.125f) + (Random.insideUnitSphere * 1f));
        lineRenderer.SetPosition(2, Vector3.Lerp(startPos, endPos, 0.25f) + (Random.insideUnitSphere * 1f));
        lineRenderer.SetPosition(3, Vector3.Lerp(startPos, endPos, 0.375f) + (Random.insideUnitSphere * 2f));
        lineRenderer.SetPosition(4, Vector3.Lerp(startPos, endPos, 0.5f) + (Random.insideUnitSphere * 2f));
        lineRenderer.SetPosition(5, Vector3.Lerp(startPos, endPos, 0.625f) + (Random.insideUnitSphere * 2f));
        lineRenderer.SetPosition(6, Vector3.Lerp(startPos, endPos, 0.75f) + (Random.insideUnitSphere * 1f));
        lineRenderer.SetPosition(7, Vector3.Lerp(startPos, endPos, 0.875f) + (Random.insideUnitSphere * 1f));

        GameObject newExplosion = Instantiate(lightningExplosion, endPos, transform.rotation);

        StartCoroutine(FadeOut());
    }

    public void SetOwner(GameObject o) {
        owner = o;
    }

    public void SetTarget(Vector3 t) {
        target = t;
    }

    public void HitPlayer() {
        canHitPlayer = true;
    }

    public IEnumerator FadeOut() {
        float alpha = 1f;
        while (true) {
            if (lineRenderer.widthMultiplier <= 0.001f) {
                lineRenderer.widthMultiplier = 0f;
                Destroy(gameObject, 1.5f);
            }
            else {
                lineRenderer.widthMultiplier *= 0.9f;
            }

            alpha -= 0.08f;
            lineRenderer.startColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, alpha);
            yield return new WaitForFixedUpdate();
        }
    }
}
