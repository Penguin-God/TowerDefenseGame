using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

using Random = UnityEngine.Random;

public class Multi_CombineSoldier : MonoBehaviourPun
{
    public int Colornumber;
    public int Soldiernumber;
    [SerializeField] Multi_CreateDefenser createDefenser;
    public SoldiersTags TagSoldier;
    public UnitManageButton unitmanage;
    
    void Start()
    {
        TagSoldier = GetComponent<SoldiersTags>();
    }

    private void CombineSuccessTextDown()
    {
        UIManager.instance.CombineSuccessText.gameObject.SetActive(false);
    }

    private void CombineFailTextDown()
    {
        UIManager.instance.CombineFailText.gameObject.SetActive(false);
    }

    #region Destory Soliders
    void RedSwordman()
    {
        TagSoldier.RedSwordmanTag();
        PhotonNetwork.Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
    }

    void RedArcher()
    {
        TagSoldier.RedArcherTag();
        PhotonNetwork.Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
    }

    void RedSpearman()
    {
        TagSoldier.RedSpearmanTag();
        PhotonNetwork.Destroy(TagSoldier.RedSpearman[0].transform.parent.gameObject);
    }

    void BlueSwordman()
    {
        TagSoldier.BlueSwordmanTag();
        PhotonNetwork.Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
    }

    void BlueArcher()
    {
        TagSoldier.BlueArcherTag();
        PhotonNetwork.Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);
    }

    void BlueSpearman()
    {
        TagSoldier.BlueSpearmanTag();
        PhotonNetwork.Destroy(TagSoldier.BlueSpearman[0].transform.parent.gameObject);
    }

    void YellowSwordman()
    {
        TagSoldier.YellowSwordmanTag();
        PhotonNetwork.Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
    }

    void YellowArcher()
    {
        TagSoldier.RedArcherTag();
        PhotonNetwork.Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);
    }

    void YellowSpearman()
    {
        TagSoldier.YellowSpearmanTag();
        PhotonNetwork.Destroy(TagSoldier.YellowSpearman[0].transform.parent.gameObject);
    }

    void GreenSwordman()
    {
        TagSoldier.GreenSwordmanTag();
        PhotonNetwork.Destroy(TagSoldier.GreenSwordman[0].transform.parent.gameObject);
    }

    void GreenArcher()
    {
        TagSoldier.RedArcherTag();
        PhotonNetwork.Destroy(TagSoldier.GreenArcher[0].transform.parent.gameObject);
    }

    void GreenSpearman()
    {
        TagSoldier.RedSpearmanTag();
        PhotonNetwork.Destroy(TagSoldier.GreenSpearman[0].transform.parent.gameObject);
    }

    void OrangeSwordman()
    {
        TagSoldier.RedSwordmanTag();
        PhotonNetwork.Destroy(TagSoldier.OrangeSwordman[0].transform.parent.gameObject);
    }

    void OrangeArcher()
    {
        TagSoldier.RedArcherTag();
        PhotonNetwork.Destroy(TagSoldier.OrangeArcher[0].transform.parent.gameObject);
    }

    void OrangeSpearman()
    {
        TagSoldier.RedSpearmanTag();
        PhotonNetwork.Destroy(TagSoldier.OrangeSpearman[0].transform.parent.gameObject);
    }

    void VioletSwordman()
    {
        TagSoldier.RedSwordmanTag();
        PhotonNetwork.Destroy(TagSoldier.VioletSwordman[0].transform.parent.gameObject);
    }

    void VioletArcher()
    {
        TagSoldier.RedArcherTag();
        PhotonNetwork.Destroy(TagSoldier.VioletArcher[0].transform.parent.gameObject);
    }

    void VioletSpearman()
    {
        TagSoldier.RedSpearmanTag();
        PhotonNetwork.Destroy(TagSoldier.VioletSpearman[0].transform.parent.gameObject);
    }
    #endregion

    public event Action<CombineData> OnCombine = null;

    public void Combine()
    {
        // TODO : 구현 해줘
        //OnCombine?.Invoke(null);
    }

    public void CombineRedArcher()
    {
        if (!photonView.IsMine)
        {
            Debug.Log("막힘");
            return;
        }
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 3)
        {

            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[2].transform.parent.gameObject);

            //Multi_SoldierPoolingManager.ReturnObject(Multi_SoldierPoolingManager.GetCurrentSoldiers("RedSwordman")[0],"RedSwordman");
            //Multi_SoldierPoolingManager.ReturnObject(Multi_SoldierPoolingManager.GetCurrentSoldiers("RedSwordman")[1], "RedSwordman");
            //Multi_SoldierPoolingManager.ReturnObject(Multi_SoldierPoolingManager.GetCurrentSoldiers("RedSwordman")[2], "RedSwordman");

            //Multi_TeamSoldier _RedArcher = Multi_SoldierPoolingManager.GetSoldier("RedArcher", 0, 1);
            

            SoldierChoose(0, 0, 1, 1);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("빨간 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
        UIManager.instance.CreateButtonAuido.Play();
    }

    public void CombineRedSpearman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.RedSwordmanTag();
        TagSoldier.RedArcherTag();
        if (TagSoldier.RedSwordman.Length >= 2 && TagSoldier.RedArcher.Length >= 3)
        {
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedArcher[2].transform.parent.gameObject);

            RedSwordman();
            RedSwordman();
            RedArcher();
            RedArcher();
            RedArcher();


            SoldierChoose(0, 0, 2, 2);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("빨간 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineRedMage()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.RedSpearmanTag();
        TagSoldier.RedArcherTag();
        if (TagSoldier.RedSpearman.Length >= 3 && TagSoldier.RedArcher.Length >= 2)
        {
            //PhotonNetwork.Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSpearman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSpearman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSpearman[2].transform.parent.gameObject);

            RedArcher();
            RedArcher();
            RedSpearman();
            RedSpearman();
            RedSpearman();

            SoldierChoose(0, 0, 3, 3);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("빨간 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineBlueArcher()
    {
        if (!photonView.IsMine)
        {
            Debug.Log("막힘");
            return;
        }
        TagSoldier.BlueSwordmanTag();
        if (TagSoldier.BlueSwordman.Length >= 3)
        {

            //PhotonNetwork.Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueSwordman[2].transform.parent.gameObject);

            BlueSwordman();
            BlueSwordman();
            BlueSwordman();


            SoldierChoose(1, 1, 1, 1);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("파란 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineBlueSpearman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.BlueSwordmanTag();
        TagSoldier.BlueArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 2 && TagSoldier.BlueArcher.Length >= 3)
        {

            //PhotonNetwork.Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueArcher[2].transform.parent.gameObject);

            BlueSwordman();
            BlueSwordman();
            BlueArcher();
            BlueArcher();
            BlueArcher();


            SoldierChoose(1, 1, 2, 2);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("파란 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineBlueMage()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.BlueSpearmanTag();
        TagSoldier.BlueArcherTag();
        if (TagSoldier.BlueSpearman.Length >= 3 && TagSoldier.BlueArcher.Length >= 2)
        {
            //PhotonNetwork.Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueSpearman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueSpearman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueSpearman[2].transform.parent.gameObject);

            BlueArcher();
            BlueArcher();
            BlueSpearman();
            BlueSpearman();
            BlueSpearman();


            SoldierChoose(1, 1, 3, 3);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("파란 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineYellowArcher()
    {
        if (!photonView.IsMine)
        {
            Debug.Log("막힘");
            return;
        }
        TagSoldier.YellowSwordmanTag();
        if (TagSoldier.YellowSwordman.Length >= 3)
        {
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[2].transform.parent.gameObject);

            YellowSwordman();
            YellowSwordman();
            YellowSwordman();

            GameManager.instance.Gold += 2;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);


            SoldierChoose(2, 2, 1, 1);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("노란 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineYellowSpearman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.YellowSwordmanTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.YellowSwordman.Length >= 2 && TagSoldier.YellowArcher.Length >= 3)
        {
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowArcher[2].transform.parent.gameObject);

            YellowSwordman();
            YellowSwordman();
            YellowArcher();
            YellowArcher();
            YellowArcher();

            GameManager.instance.Gold += 2;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(2, 2, 2, 2);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("노란 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineYellowMage()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.YellowSpearmanTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.YellowSpearman.Length >= 3 && TagSoldier.YellowArcher.Length >= 2)
        {
            //PhotonNetwork.Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowSpearman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowSpearman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowSpearman[2].transform.parent.gameObject);

            YellowArcher();
            YellowArcher();
            YellowSpearman();
            YellowSpearman();
            YellowSpearman();

            SoldierChoose(2, 2, 3, 3);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("노란 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineGreenArcher()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.GreenSwordmanTag();
        if (TagSoldier.GreenSwordman.Length >= 3)
        {

            //PhotonNetwork.Destroy(TagSoldier.GreenSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.GreenSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.GreenSwordman[2].transform.parent.gameObject);

            GreenSwordman();
            GreenSwordman();
            GreenSwordman();


            SoldierChoose(3, 3, 1, 1);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("초록 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineGreenSpearman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.BlueSwordmanTag();
        TagSoldier.YellowSwordmanTag();
        TagSoldier.GreenArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.GreenArcher.Length >= 3)
        {
            //PhotonNetwork.Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.GreenArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.GreenArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.GreenArcher[2].transform.parent.gameObject);

            BlueSwordman();
            YellowSwordman();
            GreenArcher();
            GreenArcher();
            GreenArcher();

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(3, 3, 2, 2);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("초록 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineGreenMage()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.GreenSpearmanTag();
        TagSoldier.BlueArcherTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.GreenSpearman.Length >= 3 && TagSoldier.BlueArcher.Length >= 1 && TagSoldier.YellowArcher.Length >= 1)
        {

            //PhotonNetwork.Destroy(TagSoldier.GreenSpearman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.GreenSpearman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.GreenSpearman[2].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);

            GreenSpearman();
            GreenSpearman();
            GreenSpearman();
            BlueArcher();
            YellowArcher();


            SoldierChoose(3, 3, 3, 3);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("초록 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);

    }

    public void CombineOrangeArcher()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.OrangeSwordmanTag();
        if (TagSoldier.OrangeSwordman.Length >= 3)
        {

            //PhotonNetwork.Destroy(TagSoldier.OrangeSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.OrangeSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.OrangeSwordman[2].transform.parent.gameObject);

            OrangeSwordman();
            OrangeSwordman();
            OrangeSwordman();


            SoldierChoose(4, 4, 1, 1);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("주황 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineOrangeSpearman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.RedSwordmanTag();
        TagSoldier.YellowSwordmanTag();
        TagSoldier.OrangeArcherTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.OrangeArcher.Length >= 3)
        {
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.OrangeArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.OrangeArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.OrangeArcher[2].transform.parent.gameObject);

            RedSwordman();
            YellowSwordman();
            OrangeArcher();
            OrangeArcher();
            OrangeArcher();

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(4, 4, 2, 2);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("주황 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineOrangeMage()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.OrangeSpearmanTag();
        TagSoldier.RedArcherTag();
        TagSoldier.YellowArcherTag();
        if (TagSoldier.OrangeSpearman.Length >= 3 && TagSoldier.RedArcher.Length >= 1 && TagSoldier.YellowArcher.Length >= 1)
        {

            //PhotonNetwork.Destroy(TagSoldier.OrangeSpearman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.OrangeSpearman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.OrangeSpearman[2].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.YellowArcher[0].transform.parent.gameObject);

            OrangeSpearman();
            OrangeSpearman();
            OrangeSpearman();
            RedArcher();
            YellowArcher();

            SoldierChoose(4, 4, 3, 3);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("주황 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineVioletArcher()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.VioletSwordmanTag();
        if (TagSoldier.VioletSwordman.Length >= 3)
        {

            //PhotonNetwork.Destroy(TagSoldier.VioletSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.VioletSwordman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.VioletSwordman[2].transform.parent.gameObject);

            VioletSwordman();
            VioletSwordman();
            VioletSwordman();

            SoldierChoose(5, 5, 1, 1);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("보라 궁수 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineVioletSpearman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.BlueSwordmanTag();
        TagSoldier.RedSwordmanTag();
        TagSoldier.VioletArcherTag();
        if (TagSoldier.BlueSwordman.Length >= 1 && TagSoldier.RedSwordman.Length >= 1 && TagSoldier.VioletArcher.Length >= 3)
        {
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.VioletArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.VioletArcher[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.VioletArcher[2].transform.parent.gameObject);

            RedSwordman();
            RedSwordman();
            VioletArcher();
            VioletArcher();
            VioletArcher();

            SoldierChoose(5, 5, 2, 2);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("보라 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineVioletMage()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.VioletSpearmanTag();
        TagSoldier.BlueArcherTag();
        TagSoldier.RedArcherTag();
        if (TagSoldier.VioletSpearman.Length >= 3 && TagSoldier.BlueArcher.Length >= 1 && TagSoldier.RedArcher.Length >= 1)
        {

            //PhotonNetwork.Destroy(TagSoldier.VioletSpearman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.VioletSpearman[1].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.VioletSpearman[2].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedArcher[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueArcher[0].transform.parent.gameObject);

            VioletSpearman();
            VioletSpearman();
            VioletSpearman();
            RedArcher();
            BlueArcher();


            SoldierChoose(5, 5, 3, 3);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("보라 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }



    public void CombineGreenSwordman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.YellowSwordmanTag();
        TagSoldier.BlueSwordmanTag();
        if (TagSoldier.YellowSwordman.Length >= 1 && TagSoldier.BlueSwordman.Length >= 1)
        {
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);

            YellowSwordman();
            BlueSwordman();

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(3, 3, 0, 0);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("초록 기사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineOrangeSwordman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.YellowSwordmanTag();
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.YellowSwordman.Length >= 1)
        {
            //PhotonNetwork.Destroy(TagSoldier.YellowSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);

            YellowSwordman();
            RedSwordman();

            GameManager.instance.Gold += 1;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            SoldierChoose(4,4,0,0);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("주황 기사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);
        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void CombineVioletSwordman()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        TagSoldier.BlueSwordmanTag();
        TagSoldier.RedSwordmanTag();
        if (TagSoldier.RedSwordman.Length >= 1 && TagSoldier.BlueSwordman.Length >= 1)
        {
            //PhotonNetwork.Destroy(TagSoldier.BlueSwordman[0].transform.parent.gameObject);
            //PhotonNetwork.Destroy(TagSoldier.RedSwordman[0].transform.parent.gameObject);

            BlueSwordman();
            RedSwordman();

            SoldierChoose(5, 5, 0, 0);
            Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);

            UIManager.instance.UpdateCombineSuccessText("보라 기사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        UIManager.instance.CreateButtonAuido.Play();
        unitmanage.UnitManagementButton.gameObject.SetActive(true);
    }

    public void SoldierChoose(int Colornumber1, int Colornumber2, int Soldiernumber1, int Soldiernumber2)
    {
        Colornumber = Random.Range(Colornumber1, Colornumber2);
        Soldiernumber = Random.Range(Soldiernumber1, Soldiernumber2);
    }


    public void Sommon()
    {
        SoldierChoose(0, 3, 0, 0);
        if (PhotonNetwork.IsMasterClient) createDefenser.DrawSoldier(Colornumber, Soldiernumber);
        else
        {
            if (Multi_GameManager.instance.Gold >= 5)
            {
                createDefenser.photonView.RPC("CreateSoldier", RpcTarget.MasterClient, 
                    Colornumber, Soldiernumber, Multi_WorldPosUtility.Instance.GetUnitSpawnPositon());
                Multi_GameManager.instance.AddGold(-5);
            }
        }
    }
}