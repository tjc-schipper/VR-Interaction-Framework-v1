using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grabbable : MonoBehaviour
{

    protected List<GrabInstance> grabInstances;
    public Rigidbody rb;
    public bool allowMultipleGrabs = true;
    public bool isTool = false;
    public bool useWeight = false;

    protected MoveGrabbed moveGrabbed;
    protected MovementModifier modifier;

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

            if (modifier != null)
            {
                modifier.DoModify(grabInstances, moveGrabbed);
            }
        }
    }

    public GrabInstance Grab(Grabber grabber, GrabZone grabZone)
    {
        GrabInstance grabInstance = grabZone.gameObject.AddComponent<GrabInstance>();
        grabInstance.Init(this, grabber, grabZone);
        grabInstance.OnGrabButtonReleased += Instance_GrabButtonReleased;
        grabInstances.Add(grabInstance);
        CreateGrabHandler(grabInstance);
        return grabInstance;
    }

    void Instance_GrabButtonReleased(object sender, System.EventArgs e)
    {
        if (!isTool)
        {
            GrabInstance instance = (GrabInstance)sender;
            DestroyGrabInstance(instance);
        }

    }

    /// <summary>
    /// Update the GrabHandler to result in the behaviour that we want. Replace singlehand with doublehand, vice versa, tool or not, snap or not, etc.
    /// TODO: Create a GrabParameters object to pass here that uses the Grabber, the Grabbable and the GrabZone to determine what to do.
    /// </summary>
    /// <param name="grabInstance"></param>
    protected virtual void CreateGrabHandler(GrabInstance grabInstance)
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

            if (grabInstance.grabZone.isToolGrab)
            {
                moveGrabbed = gameObject.AddComponent<MoveGrabbedSingleHand_Tool>();
                moveGrabbed.Init(grabInstances[0]);
            }
            else
            {
                moveGrabbed = gameObject.AddComponent<MoveGrabbedSingleHand>();
                moveGrabbed.Init(grabInstances[0]);
            }

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

    public void DestroyGrabInstance(GrabInstance grabInstance)
    {
        // Do all the basic stuff here like unsubscribing from events
        grabInstance.Uninit();
        grabInstance.OnGrabButtonReleased -= Instance_GrabButtonReleased;

        // Remove from bookkeeping and destroy
        grabInstances.Remove(grabInstance);
        Destroy(grabInstance);

        // Reset grab instances if there still are any
        foreach (GrabInstance gi in grabInstances)
        {
            gi.CalcGrabOrientationOffset();
        }

        CreateGrabHandler(grabInstance);
    }
    public void DestroyGrabInstance(Grabber grabber)
    {
        GrabInstance gi = grabInstances.Find(item => item.grabber.Equals(grabber));
        if (gi != null)
        {
            DestroyGrabInstance(gi);
        }
    }
    public void DestroyGrabInstance(GrabZone grabZone)
    {
        GrabInstance gi = grabInstances.Find(item => item.grabZone.Equals(grabZone));
        if (gi != null)
        {
            DestroyGrabInstance(gi);
        }
    }

    
    #region Modifiers
    public GameObject AddModifier(MovementModifier mod)
    {
        modifier = mod;
        return this.gameObject;
    }
    public GameObject RemoveModifier(MovementModifier mod)
    {
        modifier = null;
        return this.gameObject;
    }

    #endregion
}
