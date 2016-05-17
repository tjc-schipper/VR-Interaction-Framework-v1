using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grabber : MonoBehaviour
{
    public delegate void GrabberButtonEvent(object sender, System.EventArgs e);
    public event GrabberButtonEvent GrabButtonDown;
    public event GrabberButtonEvent GrabButtonUp;
    
    SteamVR_Controller.Device device;
    List<GrabZone> intersecting;
    GrabInstance currentGrabInstance;
    Rigidbody rb;

    public Transform actionPoint;

    void Awake()
    {
        intersecting = new List<GrabZone>();
        rb = gameObject.GetComponent<Rigidbody>();
        if (actionPoint == null) actionPoint = transform;
    }

    void Start()
    {
        // Get the controller input object relating to this TrackedObject
        SteamVR_TrackedObject to = gameObject.GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)to.index);
    }

    void Update()
    {
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            HandleGrabButtonDown();
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            HandleGrabButtonUp();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GrabZone gz = collider.GetComponent<GrabZone>();
        if (gz != null)
        {
            intersecting.Add(gz);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        GrabZone gz = collider.GetComponent<GrabZone>();
        if (gz != null)
        {
            intersecting.Remove(gz);
        }
    }

    void HandleGrabButtonDown()
    {
        // Nothing grabbed yet
        if (currentGrabInstance == null)
        {
            // Touching something grabbable
            if (intersecting.Count > 0)
            {
                Grabbable grabbable = intersecting[0].grabbable;
                currentGrabInstance = grabbable.Grab(this, intersecting[0]);
                currentGrabInstance.OnDestroyInstance += HandleGrabInstanceDestroyed;
            }
        }

        if (GrabButtonDown != null) GrabButtonDown(this, System.EventArgs.Empty);
    }

    void HandleGrabButtonUp()
    {
        // Do I need to do anything myself?
        if (GrabButtonUp != null) GrabButtonUp(this, System.EventArgs.Empty);
    }

    void HandleGrabInstanceDestroyed(object sender, System.EventArgs e)
    {
        // Unsubscribe from events on this grabInstance!
        GrabInstance gi = (GrabInstance)sender;
        gi.OnDestroyInstance -= HandleGrabInstanceDestroyed;

        if (currentGrabInstance.Equals(gi))
        {
            Debug.Log("Grabber reset currentGrabInstance");
            currentGrabInstance = null;
        }
    }
}
