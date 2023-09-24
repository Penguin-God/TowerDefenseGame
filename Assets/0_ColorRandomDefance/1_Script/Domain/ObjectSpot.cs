using System.Collections;
using System.Collections.Generic;

public struct ObjectSpot
{
    public readonly byte WorldId;
    public readonly bool IsInDefenseWorld;

    public ObjectSpot(byte worldId, bool isInDefenseWolrd)
    {
        WorldId = worldId;
        IsInDefenseWorld = isInDefenseWolrd;
    }

    public ObjectSpot ChangeWorldId() => new ObjectSpot((byte)(WorldId == 0 ? 1 : 0), IsInDefenseWorld);
    public ObjectSpot ChangeWorldType() => new ObjectSpot(WorldId, !IsInDefenseWorld);

    public static bool operator ==(ObjectSpot a, ObjectSpot b) => a.WorldId == b.WorldId && a.IsInDefenseWorld == b.IsInDefenseWorld;

    public static bool operator !=(ObjectSpot a, ObjectSpot b) => !(a == b);

    public override bool Equals(object obj)
    {
        if (obj is ObjectSpot other)
            return this == other;
        return false;
    }

    public override int GetHashCode() => WorldId.GetHashCode() ^ IsInDefenseWorld.GetHashCode();
}
