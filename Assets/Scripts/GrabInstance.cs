using UnityEngine;
using System.Collections;

/// <summary>
/// TODO: Visualize attachpoints and their connection to the grabber somehow?
/// Sphere that is stuck to controller, but gets stuck to Grabbable once you grab it? (And returns when you let go?)
/// </summary>
public class GrabInstance : MonoBehaviour
{
    public delegate void GrabInstanceEvent(object sender, System.EventArgs e);
    public event GrabInstanceEvent OnDestroyInstance;
    public event GrabInstanceEvent OnGrabButtonReleased;
    public event GrabInstanceEvent OnGrabButtonPressed; //TODO: Unused!

    public Grabbable grabbable;
    public Grabber grabber;
    public GrabZone grabZone;

    private Transform attachPoint;

    bool inited = false;

    public Vector3 grabOffset
    {
        get
        {
            return (attachPoint.position - grabbable.rb.position);
        }
    }
    public Quaternion grabRotationOffset;   // The rotation of the grab relative to grabbable.rb.rotation
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

        this.attachPoint = new GameObject("_attachpoint").transform;
        attachPoint.transform.position = this.grabber.actionPoint.position;
        attachPoint.transform.parent = grabbable.transform;

        // Set the location of the grab initiation relative to the grabbable rigidbody position
        this.grabRotationOffset = Quaternion.Inverse(grabber.actionPoint.rotation) * grabbable.rb.rotation;
        // http://answers.unity3d.com/questions/35541/problem-finding-relative-rotation-from-one-quatern.html
        // Need to figure out this math! Don't touch pls

        inited = true;
    }

    void OnDestroy()
    {
        Destroy(attachPoint.gameObject);
        if (OnDestroyInstance != null) OnDestroyInstance(this, System.EventArgs.Empty);
    }

    void GrabButtonReleased(object sender, System.EventArgs e)
    {
        if (OnGrabButtonReleased != null) OnGrabButtonReleased(this, System.EventArgs.Empty);
    }
}
