using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerableObject : MonoBehaviour
{
    public int numberOfTriggers = 1;
    private int triggersReceived;

    public UnityEvent OnTrigger;

    // Start is called before the first frame update
    void Start()
    {
        triggersReceived = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Trigger() {
        triggersReceived += 1;

        if (triggersReceived >= numberOfTriggers) {
            OnTrigger.Invoke();
        }
    }

    public void ReduceTriggers() {
        if (triggersReceived > 0) triggersReceived -= 1;
    }

    public void ResetTrigger() {
        triggersReceived = 0;
    }
}
