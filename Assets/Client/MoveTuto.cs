using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTuto : MonoBehaviour
{
    public void Tuto() => Managers.Scene.LoadScene(SceneTyep.Tutorial);
}
