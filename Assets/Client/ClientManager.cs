using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClientManager : MonoBehaviour
{
    public Text IronText;
    public Text WoodText;
    public Text HammerText;

    int ClientWood;
    int ClientIron;
    int ClientHammer;

    public AudioSource ClientClickAudioSource;

    void Start()
    {
        ClientIron = Managers.ClientData.MoneyByType[MoneyType.Iron].Amount;
        ClientWood = Managers.ClientData.MoneyByType[MoneyType.Wood].Amount;
        ClientHammer = Managers.ClientData.MoneyByType[MoneyType.Hammer].Amount;

        UpdateWoodText(ClientWood);
        UpdateIronText(ClientIron);
        UpdateHammerText(ClientHammer);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // a 누르면 돈복사
        {
            ClientIron += 10000;
            ClientWood += 10000;
            ClientHammer += 10000;
            InitMoney();
            UpdateMoney();
        }

        if (Input.GetKeyDown(KeyCode.S)) // 돈복사 후 모든 스킬 구매
        {
            ClientIron += 10000;
            ClientWood += 10000;
            ClientHammer += 10000;
            InitMoney();
            UpdateMoney();

            foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
                new UserSkillShopUseCase().GetSkillExp(type, 1);
        }
    }

    // 모바일을 위한 버튼 클릭 시 스킬 구매
    public void MobileSkill()
    {
        ClientIron += 10000;
        ClientWood += 10000;
        ClientHammer += 10000;
        InitMoney();
        UpdateMoney();

        foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
            new UserSkillShopUseCase().GetSkillExp(type, 1);
    }

    // 버튼에서 사용 중
    public void ShowSkillEquipUI() => Managers.UI.ShowPopupUI<UI_SkillManagementWindow>().RefreshUI();
    public void ShowSkillShopUI() => Managers.UI.ShowPopupUI<UI_SkillShop>("UI_LobbyShop").RefreshUI();

    #region update Money
    public void UpdateMoney()
    {
        UpdateIronText(ClientIron);
        UpdateWoodText(ClientWood);
        UpdateHammerText(ClientHammer);
    }

    public void UpdateIronText(int Iron)
    {
        IronText.text = "" + Iron;
    }

    public void UpdateWoodText(int Wood)
    {
        WoodText.text = "" + Wood;
    }

    public void UpdateHammerText(int Hammer)
    {
        HammerText.text = "" + Hammer;
    }
    #endregion

    public void ClientClickSound()
    {
        ClientClickAudioSource.Play();
    }

    private void InitMoney()
    {
        Managers.ClientData.MoneyByType[MoneyType.Iron].SetAmount(ClientIron);
        Managers.ClientData.MoneyByType[MoneyType.Wood].SetAmount(ClientWood);
        Managers.ClientData.MoneyByType[MoneyType.Hammer].SetAmount(ClientHammer);

        ClientIron = Managers.ClientData.MoneyByType[MoneyType.Iron].Amount;
        ClientWood = Managers.ClientData.MoneyByType[MoneyType.Wood].Amount;
        ClientHammer = Managers.ClientData.MoneyByType[MoneyType.Hammer].Amount;
    }
}
