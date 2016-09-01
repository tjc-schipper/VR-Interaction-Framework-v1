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
    public Vector3 AttachPoint
    {
        get
        {
            return attachPoint.position;
        }
    }

    GrabInstanceHaptics haptics;

    public Vector3 stretch
    {
        get
        {
            if (inited) return (grabber.actionPoint.position - attachPoint.position);
            else return Vector3.zero;
        }
    }

    public readonly float MAX_STRETCH = 0.5f;
    public float stretchDistance
    {
        get
        {
            return stretch.magnitude;
        }
    }

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

    void Update()
    {
        if (Mathf.Abs(stretchDistance) >= MAX_STRETCH)
        {
            // Break the GrabInstance
            grabbable.DestroyGrabInstance(this);
        }
    }

    public void Init(Grabbable grabbable, Grabber grabber, GrabZone grabZone)
    {
        this.grabbable = grabbable;
        this.grabber = grabber;
        this.grabZone = grabZone;

        CalcGrabOrientationOffset(resetAttachPoint: true);

        // Add haptics component so we can feel the stretch of our grabinstances
        haptics = gameObject.AddComponent<GrabInstanceHaptics>();
        haptics.Init(this);

        inited = true;
    }

    /// <summary>
    /// Calculate the difference between Grabbable rot and Grabber rot, to use for offset position setting
    /// </summary>
    /// <param name="resetAttachPoint"></param>
    public void CalcGrabOrientationOffset(bool resetAttachPoint = false)
    {
        if (resetAttachPoint)
        {
            if (attachPoint != null) Destroy(attachPoint);

            // Store the location of the grab initiation relative to the grabbable rigidbody position            
            attachPoint = new GameObject(grabber.gameObject.name + "_attachpoint").transform;
            attachPoint.position = grabber.actionPoint.position;
            attachPoint.parent = this.transform;
        }

        // Store the initial controller rotation relative to the grabbable rigidbody rotation
        this.grabRotationOffset = Quaternion.Inverse(grabber.actionPoint.rotation) * grabbable.rb.rotation;
        
        /* http://answers.unity3d.com/questions/35541/problem-finding-relative-rotation-from-one-quatern.html
         * Need to figure out this math! Don't touch pls
         */
    }

    public void Uninit()
    {
        inited = false;
    }

    void OnDestroy()
    {
        Destroy(haptics);
        Destroy(attachPoint.gameObject);
        if (OnDestroyInstance != null) OnDestroyInstance(this, System.EventArgs.Empty);
    }

    void GrabButtonReleased(object sender, System.EventArgs e)
    {
        if (OnGrabButtonReleased != null) OnGrabButtonReleased(this, System.EventArgs.Empty);
    }
}
