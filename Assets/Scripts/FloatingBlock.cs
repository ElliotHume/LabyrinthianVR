using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBlock : MonoBehaviour
{
    //private bool posX, negX, posZ, negZ;
    private Vector3 resetPosition;
    private Vector3 Xdirection = new Vector3(1,0,0);
    private Vector3 Zdirection = new Vector3(0,0,1);

    public Rigidbody rigidBody;
    private Vector3 moveDirection;
    void Start()
    {
        resetPosition = transform.position;
        if (!rigidBody) rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(string dir){

        switch (dir) {
            case "posX":
                moveDirection = Xdirection;
                break;
            case "negX":
                moveDirection = -Xdirection;
                break;
            case "posZ":
                moveDirection = Zdirection;
                break;
            case "negZ":
                moveDirection = -Zdirection;
                break;
        }

        RaycastHit rayHit;
        if (!rigidBody.SweepTest(moveDirection, out rayHit, 3f, QueryTriggerInteraction.Ignore)) {
            StartCoroutine(Slide());
        } else {
            print("Cannot move block, would hit: "+rayHit.transform.gameObject);
        }
    }

    // void CheckCanMove() {
    //     RaycastHit rayHit;
    //     posX = rigidBody.SweepTest(Xdirection, out rayHit, 3f);
    //     negX = rigidBody.SweepTest(-Xdirection, out rayHit, 3f);
    //     posZ = rigidBody.SweepTest(Zdirection, out rayHit, 3f);
    //     negZ = rigidBody.SweepTest(-Zdirection, out rayHit, 3f);
    //     // posX = Physics.SphereCast(gameObject.transform.position+Vector3.up, 2.5f, Xdirection, out rayHit, 3f);
    // }

    IEnumerator Slide() {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos+(moveDirection*3f);

        float currentTime = 0.01f;
        while (currentTime < 5) {
            float lerp = Mathf.Clamp((5 / currentTime) - 0.99f, 0, 1);
            float step = lerp * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPos, step);
            // float vertical = Mathf.Sin(lerp * Mathf.PI * 0.5f);
            // go.transform.position += Vector3.down * vertical;
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.position = endPos;
    }

    public void ResetPosition() {
        transform.position = resetPosition;
    }
}
