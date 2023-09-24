using System.Collections;
using System.Collections.Generic;

public struct ObjectSpot
{
    public readonly byte WorldId;
    public readonly bool IsInDefenseWolrd;

    public ObjectSpot(byte worldId, bool isInDefenseWolrd)
    {
        WorldId = worldId;
        IsInDefenseWolrd = isInDefenseWolrd;
    }

    public ObjectSpot ChangeWorldId() => new ObjectSpot((byte)(WorldId == 0 ? 1 : 0), IsInDefenseWolrd);
    public ObjectSpot ChangeWorldType() => new ObjectSpot(WorldId, !IsInDefenseWolrd);
}
