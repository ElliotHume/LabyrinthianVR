using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingAnchor : MonoBehaviour
{
    public GameObject drawingPlane, playerGO;
    public Vector3 anchorHeight;
    public float planeOffsetForward, planeOffsetUp;
    // Start is called before the first frame update
    void Start()
    {
        if (drawingPlane == null) {
            drawingPlane = GameObject.Find("Drawing Plane");
        }
    }

    // Update is called once per frame
    void Update() {
        if (!drawingPlane) {
            drawingPlane = GameObject.Find("Drawing Plane");
        }

        if (Vector3.Distance(transform.position, playerGO.transform.position) > 2) {
            transform.position = Vector3.MoveTowards(transform.position, playerGO.transform.position+anchorHeight, 0.1f);
        }

        drawingPlane.transform.position = transform.position + (planeOffsetForward * transform.forward) + (planeOffsetUp * transform.up);
    }
}
