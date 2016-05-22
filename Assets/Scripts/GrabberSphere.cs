using UnityEngine;
using System.Collections;

public class GrabberSphere : MonoBehaviour
{

    public Grabber grabber;
    GrabInstance grabInstance;
    LineRenderer lineRenderer;

    static readonly float maxWidth = 0.005f;
    static readonly float minWidth = 0.001f;
    static readonly float minStretchForLine = 0.1f;

    // Use this for initialization
    void Awake()
    {
        if (grabber != null)
        {
            grabber.GrabInstanceCreated += grabber_GrabInstanceCreated;
            grabber.GrabInstanceDestroyed += grabber_GrabInstanceDestroyed;
        }
    }

    void Start()
    {
        ConfigureGrabberSphere(null);   // Reset grabbersphere to controller actionpoint
    }

    void grabber_GrabInstanceDestroyed(object sender, GrabInstance gi)
    {
        if (grabInstance.Equals(gi)) grabInstance = null;
        ConfigureGrabberSphere(null);
        Destroy(lineRenderer);
    }

    void grabber_GrabInstanceCreated(object sender, GrabInstance gi)
    {
        // Parent the ball to the grabbed object so it follows it.
        grabInstance = gi;
        ConfigureGrabberSphere(gi);

        // Create the linerenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        UpdateLineRenderer();
    }

    void OnDestroy()
    {
        //unsub events
        if (grabber != null)
        {
            grabber.GrabInstanceCreated -= grabber_GrabInstanceCreated;
            grabber.GrabInstanceDestroyed -= grabber_GrabInstanceDestroyed;
        }
        // Destroy created components
        Destroy(lineRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Probably do some easing :)
        if (grabInstance != null)
        {
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        if (grabInstance != null)
        {
            // Disable linerenderer if too close
            lineRenderer.enabled = (grabInstance.grabStretch >= minStretchForLine);

            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, grabInstance.grabber.actionPoint.position);
                float width = Mathf.Lerp(minWidth, maxWidth, (grabInstance.grabStretch / grabInstance.MAX_STRETCH));
                lineRenderer.SetWidth(width, width);
            }
        }
    }

    void ConfigureGrabberSphere(GrabInstance gi = null)
    {
        if (gi == null)
        {
            // return to center
            transform.position = grabber.actionPoint.position;
            transform.parent = grabber.actionPoint;
        }
        else
        {
            // parent to grabbable, set to attachPoint
            transform.position = gi.AttachPoint;
            transform.parent = gi.grabbable.transform;
            // TODO: Actually set the grabber sphere to the attachpoint if the grabbable.grabZone has one? For tool snaps and such.
        }
    }
}
