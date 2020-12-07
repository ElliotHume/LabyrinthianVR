using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class SocketEventObject : MonoBehaviour
{
    public XRSocketInteractor socket;
    public UnityEvent onSocket;
    public string correctGameObjectsName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckSocket() {
        try {
            IEnumerable<Collider> colls = socket.selectTarget.colliders;
            List<Collider> listColls = colls.ToList();
            string socketedObjectName = listColls[0].gameObject.name;
            print(socketedObjectName+"      "+correctGameObjectsName+"      "+ socketedObjectName == correctGameObjectsName);
            if (socketedObjectName == correctGameObjectsName) onSocket.Invoke();
        } catch {
            //incorrect socket item, or error
        }
    }
}
