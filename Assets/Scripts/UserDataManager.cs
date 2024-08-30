using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    public UserDataOOP UserData;

    public int Level
    {
        get { return UserData.level; }
        set { UserData.level = value; }
    }
}
