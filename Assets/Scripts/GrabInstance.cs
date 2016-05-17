using UnityEngine;
using System.Collections;

public class GrabInstance : MonoBehaviour
{

    public delegate void GrabInstanceEvent(object sender, System.EventArgs e);
    public event GrabInstanceEvent OnDestroyInstance;
    public event GrabInstanceEvent OnGrabButtonReleased;
    public event GrabInstanceEvent OnGrabButtonPressed; //TODO: Unused!

    public Grabbable grabbable;
    public Grabber grabber;
    public GrabZone grabZone;

    bool inited = false;

    public Vector3 actionPoint;
    public const float BREAK_DISTANCE = 0.25f;

    void Start()
    {
        grabber.GrabButtonUp += GrabButtonReleased;
    }

    public void Init(Grabbable grabbable, Grabber grabber, GrabZone grabZone)
    {
        this.grabbable = grabbable;
        this.grabber = grabber;
        this.grabZone = grabZone;
        
        // Set the location of the grab initiation relative to the grabbable rigidbody position
        this.actionPoint = grabber.actionPoint.position - grabbable.rb.position;

        inited = true;
    }

    void OnDestroy()
    {
        Debug.Log("Instance says: Destroyed!");
        if (OnDestroyInstance != null) OnDestroyInstance(this, System.EventArgs.Empty);
    }

    void GrabButtonReleased(object sender, System.EventArgs e)
    {
        if (OnGrabButtonReleased != null) OnGrabButtonReleased(this, System.EventArgs.Empty);
    }
}
