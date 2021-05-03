using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMode : MonoBehaviour
{
    public void EnterStoryMode()
    {
        if(Camera.main.gameObject.transform.position.x == 500)
        {
            Camera.main.gameObject.transform.position = new Vector3(0, 100, -30);
        }
        else
        {
            Camera.main.gameObject.transform.position = new Vector3(500, 100, -30);
        }
        
    }

    public void SoldiersEnterStoryMode()
    {

    }



}
