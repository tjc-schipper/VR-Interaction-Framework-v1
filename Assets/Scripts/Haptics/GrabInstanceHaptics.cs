using UnityEngine;
using System.Collections;

public class GrabInstanceHaptics : MonoBehaviour
{

    GrabInstance grabInstance;
    GrabberHaptics haptics;
    bool inited = false;

    int pulseStrength = 200;
    readonly int PULSE_MIN_STRENGTH = 100;

    int updateCounter;
    readonly int PULSE_UPDATE_INTERVAL = 1;
    
    int strength;

    void Start()
    {
        updateCounter = 0;
    }
    
    public void Init(GrabInstance gi)
    {
        grabInstance = gi;
        haptics = grabInstance.grabber.GetComponent<GrabberHaptics>();
        if (haptics == null) return;
        inited = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inited) return;
        strength = (int)(Mathf.Clamp01(grabInstance.stretchDistance / grabInstance.MAX_STRETCH) * pulseStrength);
        
        updateCounter++;
        if (updateCounter >= PULSE_UPDATE_INTERVAL)
        {
            if (strength > PULSE_MIN_STRENGTH)
            {
                haptics.DoPulse(strength);
            }
            updateCounter = 0;
        }
    }
}
