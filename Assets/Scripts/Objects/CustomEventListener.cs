using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEventListener : MonoBehaviour
{
    public int numberOfEventsToActivate;
    int events = 0;

    public UnityEvent onActivate;

    public void Activate() {
        events += 1;
        if (events >= numberOfEventsToActivate) {
            onActivate.Invoke();

            events = 0;
        }
    }

    public void Deactivate() {
        if ( events > 0) events -= 1;
    }
}
