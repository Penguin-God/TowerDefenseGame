using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSoldier : MonoBehaviour
{
    private int Colornumber;
    private int Soldiernumber;
    public CreateDefenser createdefenser;
    public SoldiersTags TagSoldier;

    void Start()
    {
        TagSoldier = GetComponent<SoldiersTags>();
    }

    public void Combine() // 태그로 바꾸기
    {
        if (GameManager.instance.hitSolider.gameObject.tag == "RedSwordman" )
        {
            TagSoldier.RedSwordmanTag();
            if (TagSoldier.RedSwordman.Length >= 2)
            {
                
                Destroy(TagSoldier.RedSwordman[0]);
                Destroy(TagSoldier.RedSwordman[1]);


                SoldierChoose(0, 0, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "BlueSwordman") // 태그로 바꿔듬
        {
            TagSoldier.BlueSwordmanTag();
            if (TagSoldier.BlueSwordman.Length >= 2)
            {

                Destroy(TagSoldier.BlueSwordman[0]);
                Destroy(TagSoldier.BlueSwordman[1]);


                SoldierChoose(1, 1, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "YellowSwordman")
        {
            TagSoldier.YellowSwordmanTag();
            if (TagSoldier.YellowSwordman.Length >= 2)
            {

                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.YellowSwordman[1]);


                SoldierChoose(2, 2, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "GreenSwordman")
        {
            TagSoldier.GreenSwordmanTag();
            if (TagSoldier.GreenSwordman.Length >= 2)
            {

                Destroy(TagSoldier.GreenSwordman[0]);
                Destroy(TagSoldier.GreenSwordman[1]);


                SoldierChoose(3, 3, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "OrangeSwordman")
        {
            TagSoldier.OrangeSwordmanTag();
            if (TagSoldier.OrangeSwordman.Length >= 2)
            {

                Destroy(TagSoldier.OrangeSwordman[0]);
                Destroy(TagSoldier.OrangeSwordman[1]);


                SoldierChoose(4, 4, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "VioletSwordman")
        {
            TagSoldier.VioletSwordmanTag();
            if (TagSoldier.VioletSwordman.Length >= 2)
            {

                Destroy(TagSoldier.VioletSwordman[0]);
                Destroy(TagSoldier.VioletSwordman[1]);


                SoldierChoose(5, 5, 1, 1);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "RedArcher") 
        {
            TagSoldier.RedSwordmanTag();
            TagSoldier.RedArcherTag();
            if (TagSoldier.RedSwordman.Length >= 2 && TagSoldier.RedArcher.Length >=2)
            {

                Destroy(TagSoldier.RedSwordman[0]);
                Destroy(TagSoldier.RedSwordman[1]);
                Destroy(TagSoldier.RedArcher[0]);
                Destroy(TagSoldier.RedArcher[1]);


                SoldierChoose(0, 0, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "BlueArcher")
        {
            TagSoldier.BlueSwordmanTag();
            TagSoldier.BlueArcherTag();
            if (TagSoldier.BlueSwordman.Length >= 2 && TagSoldier.BlueArcher.Length >= 2)
            {

                Destroy(TagSoldier.BlueSwordman[0]);
                Destroy(TagSoldier.BlueSwordman[1]);
                Destroy(TagSoldier.BlueArcher[0]);
                Destroy(TagSoldier.BlueArcher[1]);


                SoldierChoose(1, 1, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "YellowArcher")
        {
            TagSoldier.YellowSwordmanTag();
            TagSoldier.YellowArcherTag();
            if (TagSoldier.YellowSwordman.Length >= 2 && TagSoldier.YellowArcher.Length >= 2)
            {

                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.YellowSwordman[1]);
                Destroy(TagSoldier.YellowArcher[0]);
                Destroy(TagSoldier.YellowArcher[1]);


                SoldierChoose(2, 2, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "GreenArcher")
        {
            TagSoldier.BlueSwordmanTag();
            TagSoldier.YellowSwordmanTag();
            TagSoldier.GreenArcherTag();
            if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.GreenArcher.Length >= 2)
            {

                Destroy(TagSoldier.BlueSwordman[0]);
                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.GreenArcher[0]);
                Destroy(TagSoldier.GreenArcher[1]);


                SoldierChoose(3, 3, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "OrangeArcher")
        {
            TagSoldier.RedSwordmanTag();
            TagSoldier.YellowSwordmanTag();
            TagSoldier.OrangeArcherTag();
            if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.OrangeArcher.Length >= 2)
            {

                Destroy(TagSoldier.RedSwordman[0]);
                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.OrangeArcher[0]);
                Destroy(TagSoldier.OrangeArcher[1]);


                SoldierChoose(4, 4, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "VioletArcher")
        {
            TagSoldier.BlueSwordmanTag();
            TagSoldier.RedSwordmanTag();
            TagSoldier.VioletArcherTag();
            if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.RedSwordman.Length >= 1 && TagSoldier.VioletArcher.Length >= 2)
            {

                Destroy(TagSoldier.RedSwordman[0]);
                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.VioletArcher[0]);
                Destroy(TagSoldier.VioletArcher[1]);


                SoldierChoose(5, 5, 2, 2);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "RedSpearman")
        {
            TagSoldier.RedSpearmanTag();
            if (TagSoldier.RedSpearman.Length >= 2)
            {

                Destroy(TagSoldier.RedSpearman[0]);
                Destroy(TagSoldier.RedSpearman[1]);


                SoldierChoose(0, 0, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "BlueSpearman")
        {
            TagSoldier.BlueSpearmanTag();
            if (TagSoldier.BlueSpearman.Length >= 2)
            {

                Destroy(TagSoldier.BlueSpearman[0]);
                Destroy(TagSoldier.BlueSpearman[1]);


                SoldierChoose(1, 1, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "YellowSpearman")
        {
            TagSoldier.YellowSpearmanTag();
            if (TagSoldier.YellowSpearman.Length >= 2)
            {

                Destroy(TagSoldier.YellowSpearman[0]);
                Destroy(TagSoldier.YellowSpearman[1]);


                SoldierChoose(2, 2, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "GreenSpearman")
        {
            TagSoldier.GreenSpearmanTag();
            TagSoldier.BlueArcherTag();
            TagSoldier.YellowArcherTag();
            if (TagSoldier.GreenSpearman.Length >= 2 && TagSoldier.BlueArcher.Length >= 1 && TagSoldier.YellowArcher.Length >= 1)
            {

                Destroy(TagSoldier.GreenSpearman[0]);
                Destroy(TagSoldier.GreenSpearman[1]);
                Destroy(TagSoldier.BlueArcher[0]);
                Destroy(TagSoldier.YellowArcher[0]);


                SoldierChoose(3, 3, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "OrangeSpearman")
        {
            TagSoldier.OrangeSpearmanTag();
            TagSoldier.RedArcherTag();
            TagSoldier.YellowArcherTag();
            if (TagSoldier.OrangeSpearman.Length >= 2 && TagSoldier.RedArcher.Length >= 1 && TagSoldier.YellowArcher.Length >= 1)
            {

                Destroy(TagSoldier.OrangeSpearman[0]);
                Destroy(TagSoldier.OrangeSpearman[1]);
                Destroy(TagSoldier.RedArcher[0]);
                Destroy(TagSoldier.YellowArcher[0]);


                SoldierChoose(4, 4, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "VioletSpearman")
        {
            TagSoldier.VioletSpearmanTag();
            TagSoldier.BlueArcherTag();
            TagSoldier.RedArcherTag();
            if (TagSoldier.VioletSpearman.Length >= 2 && TagSoldier.BlueArcher.Length >= 1 && TagSoldier.RedArcher.Length >= 1)
            {

                Destroy(TagSoldier.VioletSpearman[0]);
                Destroy(TagSoldier.VioletSpearman[1]);
                Destroy(TagSoldier.RedArcher[0]);
                Destroy(TagSoldier.YellowArcher[0]);


                SoldierChoose(5, 5, 3, 3);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

    }

    public void otherCombine()
    {
        if (GameManager.instance.hitSolider.gameObject.tag == "RedSwordman")
        {
            TagSoldier.RedSwordmanTag();
            TagSoldier.BlueSwordmanTag();
            if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.BlueSwordman.Length >= 1)
            {

                Destroy(TagSoldier.RedSwordman[0]);
                Destroy(TagSoldier.BlueSwordman[0]);


                SoldierChoose(5, 5, 0, 0);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "BlueSwordman")
        {
            TagSoldier.YellowSwordmanTag();
            TagSoldier.BlueSwordmanTag();
            if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1)
            {

                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.BlueSwordman[0]);


                SoldierChoose(3, 3, 0, 0);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }

        if (GameManager.instance.hitSolider.gameObject.tag == "YellowSwordman")
        {
            TagSoldier.YellowSwordmanTag();
            TagSoldier.RedSwordmanTag();
            if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1)
            {

                Destroy(TagSoldier.YellowSwordman[0]);
                Destroy(TagSoldier.RedSwordman[0]);


                SoldierChoose(4, 4, 0, 0);
                createdefenser.CreateSoldier(Colornumber, Soldiernumber);
                UIManager.instance.ButtonDown();
            }
        }
        //UIManager.instance.ButtonDown();
    }


    private void SoldierChoose(int Colornumber1, int Colornumber2, int Soldiernumber1,int Soldiernumber2)
    {
        Colornumber = Random.Range(Colornumber1, Colornumber2);
        Soldiernumber = Random.Range(Soldiernumber1, Soldiernumber2);
        
    }
    
    public void ButtonOn() // 박준 메모 - Dictionary 자료구조랑 swich문 사용해서 바꿀 예정
    {

        //GameManager.instance.Chilk();
        UIManager.instance.ButtonDown();
        if (GameManager.instance.hitSolider.transform.gameObject.tag == "RedSwordman")
        {
            UIManager.instance.SetActiveButton(true, 0, 0);
            UIManager.instance.SetActiveButton2(true, 0, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "BlueSwordman")
        {
            UIManager.instance.SetActiveButton(true, 1, 0);
            UIManager.instance.SetActiveButton2(true, 1, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "YellowSwordman")
        {
            UIManager.instance.SetActiveButton(true, 2, 0);
            UIManager.instance.SetActiveButton2(true, 2, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "GreenSwordman")
        {
            UIManager.instance.SetActiveButton(true, 3, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "OrangeSwordman")
        {
            UIManager.instance.SetActiveButton(true, 4, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "VioletSwordman")
        {
            UIManager.instance.SetActiveButton(true, 5, 0);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "RedArcher")
        {
            UIManager.instance.SetActiveButton(true, 0, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "BlueArcher")
        {
            UIManager.instance.SetActiveButton(true, 1, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "YellowArcher")
        {
            UIManager.instance.SetActiveButton(true, 2, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "GreenArcher")
        {
            UIManager.instance.SetActiveButton(true, 3, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "OrangeArcher")
        {
            UIManager.instance.SetActiveButton(true, 4, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "VioletArcher")
        {
            UIManager.instance.SetActiveButton(true, 5, 1);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "RedSpearman")
        {
            UIManager.instance.SetActiveButton(true, 0, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "BlueSpearman")
        {
            UIManager.instance.SetActiveButton(true, 1, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "YellowSpearman")
        {
            UIManager.instance.SetActiveButton(true, 2, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "GreenSpearman")
        {
            UIManager.instance.SetActiveButton(true, 3, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "OrangeSpearman")
        {
            UIManager.instance.SetActiveButton(true, 4, 2);
        }

        if (GameManager.instance.hitSolider.transform.gameObject.tag == "VioletSpearman")
        {
            UIManager.instance.SetActiveButton(true, 5, 2);
        }

    }

    public void Sommon()
    {
        SoldierChoose(0, 3, 0, 0);
        createdefenser.DrawSoldier(Colornumber, Soldiernumber);
        // createdefenser.CreateSoldier(Colornumber, Soldiernumber);
        // createdefenser.ExpenditureGold();
    }
}
