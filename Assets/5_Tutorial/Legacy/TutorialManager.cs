using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    void Awake()
    {
        gameObject.AddComponent<Tutorial_Basic>();
        gameObject.AddComponent<Tutorial_OtherPlayer>();
        gameObject.AddComponent<Tutorial_Tower>();
        gameObject.AddComponent<Tutorial_Boss>();
    }
}
