using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grabbable : MonoBehaviour
{

    List<GrabInstance> grabInstances;
    public Rigidbody rb;
    public bool allowMultipleGrabs = true;
    public bool isTool = false;

    MoveGrabbed moveGrabbed;

    void Awake()
    {
        grabInstances = new List<GrabInstance>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (moveGrabbed != null)
        {
            moveGrabbed.DoMove();
        }
    }

    public GrabInstance Grab(Grabber grabber, GrabZone grabZone)
    {
        GrabInstance gi = grabZone.gameObject.AddComponent<GrabInstance>();
        gi.Init(this, grabber, grabZone);
        gi.OnGrabButtonReleased += Instance_GrabButtonReleased;
        grabInstances.Add(gi);
        SetMoveGrabbed();
        return gi;
    }

    void Instance_GrabButtonReleased(object sender, System.EventArgs e)
    {
        if (!isTool)
        {
            GrabInstance instance = (GrabInstance)sender;
            DestroyGrabInstance(instance);
        }

    }

    void SetMoveGrabbed()
    {
        if (grabInstances.Count == 0)
        {
            DestroyImmediate(moveGrabbed);
            moveGrabbed = null;
        }
        else if (grabInstances.Count == 1)
        {
            // Do simple grab
            DestroyImmediate(moveGrabbed);
            moveGrabbed = gameObject.AddComponent<MoveGrabbedSingleHand>();
            moveGrabbed.Init(grabInstances[0]);
        }
        else if (grabInstances.Count == 2)
        {
            if (allowMultipleGrabs)
            {
                // Do dual-hand grab
                DestroyImmediate(moveGrabbed);
                moveGrabbed = gameObject.AddComponent<MoveGrabbedDualHand>();
                moveGrabbed.Init(grabInstances[0], grabInstances[1]);
            }
        }
    }

    void DestroyGrabInstance(GrabInstance grabInstance)
    {
        // Do all the basic stuff here like unsubscribing from events
        grabInstance.OnGrabButtonReleased -= Instance_GrabButtonReleased;
        
        // Remove from bookkeeping and destroy
        grabInstances.Remove(grabInstance);
        Destroy(grabInstance);

        SetMoveGrabbed();
    }
    void DestroyGrabInstance(Grabber grabber)
    {
        GrabInstance gi = grabInstances.Find(item => item.grabber.Equals(grabber));
        if (gi != null)
        {
            DestroyGrabInstance(gi);
        }
    }
    void DestroyGrabInstance(GrabZone grabZone)
    {
        GrabInstance gi = grabInstances.Find(item => item.grabZone.Equals(grabZone));
        if (gi != null)
        {
            DestroyGrabInstance(gi);
        }
    }
}
