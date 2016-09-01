using UnityEngine;
using System.Collections;

public class GrabberSphere : MonoBehaviour
{

    public Grabber grabber;
    GrabInstance grabInstance;
    LineRenderer lineRenderer;

    static readonly float maxWidth = 0.01f;
    static readonly float minWidth = 0.002f;
    static readonly float minStretchForLine = 0.1f;     // At stretchDistance does the linerenderer appear?
    static readonly float breakWarningPercentage = 0.8f;  // At which percentage of max_stretch does the line turn into a warning color?

    static readonly Color lineColor_light = new Color(0.2f, 1f, 0f, 0.1f);
    static readonly Color lineColor_heavy = new Color(0.2f, 1f, 0.2f, 0.5f);
    static readonly Color lineColor_break = new Color(1f, 0, 0f, 1f);

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
            lineRenderer.enabled = (grabInstance.stretchDistance >= minStretchForLine);

            if (lineRenderer != null)
            {
                // Update positions
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, grabInstance.grabber.actionPoint.position);

                // Update widths
                float startWidth = Mathf.Lerp(minWidth, maxWidth, (Mathf.Abs(grabInstance.stretchDistance / grabInstance.MAX_STRETCH)));
                float endWidth = (startWidth / 2f) * (Mathf.Abs(grabInstance.stretchDistance / grabInstance.MAX_STRETCH));
                lineRenderer.SetWidth(startWidth, endWidth);

                // Update color
                Color lineColor = Color.white;
                if (grabInstance.stretchDistance > (grabInstance.MAX_STRETCH * breakWarningPercentage))
                {
                    lineColor = lineColor_break;
                }
                else
                {
                    lineColor = Color.Lerp(lineColor_light, lineColor_heavy, (grabInstance.stretchDistance / (grabInstance.MAX_STRETCH * breakWarningPercentage)));
                }
                lineRenderer.material.color = lineColor;
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
