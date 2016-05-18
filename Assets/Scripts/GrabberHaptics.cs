using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabberHaptics : MonoBehaviour
{

    const int ENTER_EXIT_PULSE = 1000;
    const int STAY_PULSE = 100;

    Grabber grabber;
    SteamVR_Controller.Device device;

    float distanceCounter;
    float distanceBetweenPulses = 0.005f;
    Vector3 lastPos;
    
    // Use this for initialization
    void Start()
    {
        // Get the controller input object relating to this TrackedObject
        SteamVR_TrackedObject to = gameObject.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)to.index);

        distanceCounter = 0f;

        grabber = gameObject.GetComponent<Grabber>();
        grabber.GrabbableTouchStarted += grabber_ColliderEntered;
        grabber.GrabbableTouchEnded += grabber_ColliderLeft;
    }

    void Update()
    {
        if (grabber.intersecting.Count > 0)
        {
            // You're inside a grabbable, make some noise
            distanceCounter += (grabber.transform.position - lastPos).magnitude;
            if (distanceCounter > distanceBetweenPulses)
            {
                ShortColliderPulse(STAY_PULSE);
                distanceCounter = 0f;
            }
        }

        lastPos = grabber.transform.position;
    }

    void grabber_ColliderLeft(object sender, GrabZone gz)
    {
        ShortColliderPulse(ENTER_EXIT_PULSE);
    }

    void grabber_ColliderEntered(object sender, GrabZone gz)
    {
        ShortColliderPulse(ENTER_EXIT_PULSE);
    }

    
    void ShortColliderPulse(int duration = 500)
    {
        Debug.Log("Haptics pulse triggered: " + duration.ToString());        
        device.TriggerHapticPulse((ushort)duration);
    }
}
