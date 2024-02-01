using UnityEngine;

public class BoolWrapper
{
    public bool value { get; private set; }
    private float delayTimeSec;
    private float deactivateTime;

    public BoolWrapper(float delay)
    {
        this.delayTimeSec = delay;
    }

    public void Start()
    {
        value = true;
        deactivateTime = Time.time + delayTimeSec;
    }

    public void Update()
    {
        if (value && Time.time >= deactivateTime)
        {
            value = false;
        }
    }
}