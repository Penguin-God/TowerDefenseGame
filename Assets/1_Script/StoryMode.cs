using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryMode : MonoBehaviour
{
    public SoldiersTags soldiersTags;

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

    public void RedSwordmanEnterStoryMode()
    {
        soldiersTags.RedSwordmanTag();
        for(int i = 0; i < soldiersTags.RedSwordman.Length; i++)
        {
            if(soldiersTags.RedSwordman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.RedSwordman[i]);

                return;
            }
        } 
    }

    public void BlueSwordmanEnterStoryMode()
    {

    }

    public void YellowSwordmanEnterStoryMode()
    {

    }

    public void GreenSwordmanEnterStoryMode()
    {

    }

    public void OrangeSwordmanEnterStoryMode()
    {

    }

    public void VioletSwordmanEnterStoryMode()
    {

    }

    public void RedArcherEnterStoryMode()
    {

    }

    public void BlueArcherEnterStoryMode()
    {

    }

    public void YellowArcherEnterStoryMode()
    {

    }

    public void GreenArcherEnterStoryMode()
    {

    }

    public void OrangeArcherEnterStoryMode()
    {

    }

    public void VioletArcherEnterStoryMode()
    {

    }

    public void RedSpearmanEnterStoryMode()
    {

    }

    public void BlueSpearmanEnterStoryMode()
    {

    }

    public void YellowSpearmanEnterStoryMode()
    {

    }

    public void GreenSpearmanEnterStoryMode()
    {

    }

    public void OrangeSpearmanEnterStoryMode()
    {

    }

    public void VioletSpearmanEnterStoryMode()
    {

    }

    public void RedMageEnterStoryMode()
    {

    }

    public void BlueMageEnterStoryMode()
    {

    }

    public void YellowMageEnterStoryMode()
    {

    }

    public void GreenMageEnterStoryMode()
    {

    }

    public void OrangeMageEnterStoryMode()
    {

    }

    public void VioletMageEnterStoryMode()
    {

    }





}
