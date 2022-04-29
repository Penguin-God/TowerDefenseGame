using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_Managers : MonoBehaviour
{
    private static Multi_Managers instance;
    private static Multi_Managers Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Multi_Managers>();
                if (instance == null)
                    instance = new GameObject("Multi_Managers").AddComponent<Multi_Managers>();
            }

            return instance;
        }
    }

    Multi_ResourcesManager _resources = new Multi_ResourcesManager();

    public static Multi_ResourcesManager Resources => Instance._resources;
}
