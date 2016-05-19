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

    void grabber_GrabInstanceDestroyed(object sender, GrabInstance gi)
    {
        if (grabInstance.Equals(gi)) grabInstance = null;

        // return ball to default position
        transform.position = grabber.actionPoint.position;
        transform.parent = grabber.transform;

        Destroy(lineRenderer);
    }

    void grabber_GrabInstanceCreated(object sender, GrabInstance gi)
    {
        // Parent the ball to the grabbed object so it follows it.
        grabInstance = gi;
        transform.parent = grabInstance.grabbable.transform;

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
}
