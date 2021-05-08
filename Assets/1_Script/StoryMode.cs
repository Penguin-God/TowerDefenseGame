using UnityEngine;

public class StoryMode : MonoBehaviour
{
    public SoldiersTags soldiersTags;
    public CombineSoldier combineSoldier;
    public CreateDefenser createDefenser;
    
    public void TranslateUnit()
    {
        Transform hitUnit = GameManager.instance.hitSolider.transform.parent;
        if (hitUnit == null) return; // null이면 return
        if (hitUnit.position.x < 300)
        {
            Instantiate(hitUnit.gameObject, SetRandomPosition(460, 540, 20, -20, true), hitUnit.rotation);
            Destroy(hitUnit.gameObject);
        }
        else
        {
            Instantiate(hitUnit.gameObject, SetRandomPosition(10, -10, 10, -10, false), hitUnit.rotation);
            Destroy(hitUnit.gameObject);
        }
    }

    Vector3 SetRandomPosition(float maxX, float minX, float maxZ, float minZ, bool isTower)
    {
        float randomX = 0f;
        if (isTower)
        {
            float randomX_1 = Random.Range(minX, 520);
            float randomX_2 = Random.Range(520, maxX);
            int xArea = Random.Range(0, 2);
            randomX = (xArea == 0) ? randomX_1 : randomX_2;
        }
        else randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, 0, randomZ);
    }



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
        for(int i = 0; i <= soldiersTags.RedSwordman.Length; i++)
        {
            if(soldiersTags.RedSwordman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.RedSwordman[i]);
                combineSoldier.SoldierChoose(0, 0, 0, 0);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        } 
    }

    public void BlueSwordmanEnterStoryMode()
    {
        soldiersTags.BlueSwordmanTag();
        for (int i = 0; i <= soldiersTags.BlueSwordman.Length; i++)
        {
            if (soldiersTags.BlueSwordman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.BlueSwordman[i]);
                combineSoldier.SoldierChoose(1, 1, 0, 0);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void YellowSwordmanEnterStoryMode()
    {
        soldiersTags.YellowSwordmanTag();
        for (int i = 0; i <= soldiersTags.YellowSwordman.Length; i++)
        {
            if (soldiersTags.YellowSwordman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.YellowSwordman[i]);
                combineSoldier.SoldierChoose(2, 2, 0, 0);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void GreenSwordmanEnterStoryMode()
    {
        soldiersTags.GreenSwordmanTag();
        for (int i = 0; i <= soldiersTags.GreenSwordman.Length; i++)
        {
            if (soldiersTags.GreenSwordman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.GreenSwordman[i]);
                combineSoldier.SoldierChoose(3, 3, 0, 0);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void OrangeSwordmanEnterStoryMode()
    {
        soldiersTags.OrangeSwordmanTag();
        for (int i = 0; i <= soldiersTags.OrangeSwordman.Length; i++)
        {
            if (soldiersTags.OrangeSwordman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.OrangeSwordman[i]);
                combineSoldier.SoldierChoose(4, 4, 0, 0);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void VioletSwordmanEnterStoryMode()
    {
        soldiersTags.VioletSwordmanTag();
        for (int i = 0; i <= soldiersTags.VioletSwordman.Length; i++)
        {
            if (soldiersTags.VioletSwordman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.VioletSwordman[i]);
                combineSoldier.SoldierChoose(5, 5, 0, 0);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void RedArcherEnterStoryMode()
    {
        soldiersTags.RedArcherTag();
        for (int i = 0; i <= soldiersTags.RedArcher.Length; i++)
        {
            if (soldiersTags.RedArcher[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.RedArcher[i]);
                combineSoldier.SoldierChoose(0, 0, 1, 1);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void BlueArcherEnterStoryMode()
    {
        soldiersTags.BlueArcherTag();
        for (int i = 0; i <= soldiersTags.BlueArcher.Length; i++)
        {
            if (soldiersTags.BlueArcher[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.BlueArcher[i]);
                combineSoldier.SoldierChoose(1, 1, 1, 1);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void YellowArcherEnterStoryMode()
    {
        soldiersTags.YellowArcherTag();
        for (int i = 0; i <= soldiersTags.YellowArcher.Length; i++)
        {
            if (soldiersTags.YellowArcher[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.YellowArcher[i]);
                combineSoldier.SoldierChoose(2, 2, 1, 1);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void GreenArcherEnterStoryMode()
    {
        soldiersTags.GreenArcherTag();
        for (int i = 0; i <= soldiersTags.GreenArcher.Length; i++)
        {
            if (soldiersTags.GreenArcher[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.GreenArcher[i]);
                combineSoldier.SoldierChoose(3, 3, 1, 1);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void OrangeArcherEnterStoryMode()
    {
        soldiersTags.OrangeArcherTag();
        for (int i = 0; i <= soldiersTags.OrangeArcher.Length; i++)
        {
            if (soldiersTags.OrangeArcher[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.OrangeArcher[i]);
                combineSoldier.SoldierChoose(4, 4, 1, 1);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void VioletArcherEnterStoryMode()
    {
        soldiersTags.VioletArcherTag();
        for (int i = 0; i <= soldiersTags.VioletArcher.Length; i++)
        {
            if (soldiersTags.VioletArcher[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.VioletArcher[i]);
                combineSoldier.SoldierChoose(5, 5, 1, 1);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void RedSpearmanEnterStoryMode()
    {
        soldiersTags.RedSpearmanTag();
        for (int i = 0; i <= soldiersTags.RedSpearman.Length; i++)
        {
            if (soldiersTags.RedSpearman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.RedSpearman[i]);
                combineSoldier.SoldierChoose(0, 0, 2, 2);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void BlueSpearmanEnterStoryMode()
    {
        soldiersTags.BlueSpearmanTag();
        for (int i = 0; i <= soldiersTags.BlueSpearman.Length; i++)
        {
            if (soldiersTags.BlueSpearman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.BlueSpearman[i]);
                combineSoldier.SoldierChoose(1, 1, 2, 2);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void YellowSpearmanEnterStoryMode()
    {
        soldiersTags.YellowSpearmanTag();
        for (int i = 0; i <= soldiersTags.YellowSpearman.Length; i++)
        {
            if (soldiersTags.YellowSpearman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.YellowSpearman[i]);
                combineSoldier.SoldierChoose(2, 2, 2, 2);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void GreenSpearmanEnterStoryMode()
    {
        soldiersTags.GreenSpearmanTag();
        for (int i = 0; i <= soldiersTags.GreenSpearman.Length; i++)
        {
            if (soldiersTags.GreenSpearman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.GreenSpearman[i]);
                combineSoldier.SoldierChoose(3, 3, 2, 2);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void OrangeSpearmanEnterStoryMode()
    {
        soldiersTags.OrangeSpearmanTag();
        for (int i = 0; i <= soldiersTags.OrangeSpearman.Length; i++)
        {
            if (soldiersTags.OrangeSpearman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.OrangeSpearman[i]);
                combineSoldier.SoldierChoose(4, 4, 2, 2);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void VioletSpearmanEnterStoryMode()
    {
        soldiersTags.VioletSpearmanTag();
        for (int i = 0; i <= soldiersTags.VioletSpearman.Length; i++)
        {
            if (soldiersTags.VioletSpearman[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.VioletSpearman[i]);
                combineSoldier.SoldierChoose(5, 5, 2, 2);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void RedMageEnterStoryMode()
    {
        soldiersTags.RedMageTag();
        for (int i = 0; i <= soldiersTags.RedMage.Length; i++)
        {
            if (soldiersTags.RedMage[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.RedMage[i]);
                combineSoldier.SoldierChoose(0, 0, 3, 3);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void BlueMageEnterStoryMode()
    {
        soldiersTags.BlueMageTag();
        for (int i = 0; i <= soldiersTags.BlueMage.Length; i++)
        {
            if (soldiersTags.BlueMage[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.BlueMage[i]);
                combineSoldier.SoldierChoose(1, 1, 3, 3);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void YellowMageEnterStoryMode()
    {
        soldiersTags.YellowMageTag();
        for (int i = 0; i <= soldiersTags.YellowMage.Length; i++)
        {
            if (soldiersTags.YellowMage[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.YellowMage[i]);
                combineSoldier.SoldierChoose(2, 2, 3, 3);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void GreenMageEnterStoryMode()
    {
        soldiersTags.GreenMageTag();
        for (int i = 0; i <= soldiersTags.GreenMage.Length; i++)
        {
            if (soldiersTags.GreenMage[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.GreenMage[i]);
                combineSoldier.SoldierChoose(3, 3, 3, 3);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void OrangeMageEnterStoryMode()
    {
        soldiersTags.OrangeMageTag();
        for (int i = 0; i <= soldiersTags.OrangeMage.Length; i++)
        {
            if (soldiersTags.OrangeMage[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.OrangeMage[i]);
                combineSoldier.SoldierChoose(4, 4, 3, 3);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }

    public void VioletMageEnterStoryMode()
    {
        soldiersTags.VioletMageTag();
        for (int i = 0; i <= soldiersTags.VioletMage.Length; i++)
        {
            if (soldiersTags.VioletMage[i].transform.position.x < 300)
            {
                Destroy(soldiersTags.VioletMage[i]);
                combineSoldier.SoldierChoose(5, 5, 3, 3);
                createDefenser.StoryModeCreateSoldier(combineSoldier.Colornumber, combineSoldier.Soldiernumber);

            }
            return;
        }
    }





}
