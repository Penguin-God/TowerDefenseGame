using System.Collections;
using System.Collections.Generic;

public class SpeedManager
{
    public float OriginSpeed { get; private set; }
    public float CurrentSpeed { get; private set; }

    public SpeedManager(float originSpeed) => ChangeOriginSpeed(originSpeed);

    public void ChangeOriginSpeed(float originSpeed)
    {
        OriginSpeed = originSpeed;
        CurrentSpeed = originSpeed;
    }
    public void ChangeSpeed(float speed)
    {
        if(speed > OriginSpeed)
            speed = OriginSpeed;
        CurrentSpeed = speed;
    }
    public void RestoreSpeed() => ChangeSpeed(OriginSpeed);
}
