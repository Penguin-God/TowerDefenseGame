using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerDataManager
{
    public UserInfo UserInfo { get; private set; }
    public PlayerDataManager(UserInfo userInfo)
    {
        UserInfo = userInfo;
    }
}
