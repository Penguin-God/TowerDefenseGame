using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyTestHelper : MonoBehaviour
{
    BattleDIContainer _container;
    PlayerDataManager _playerData;
    public void SetContainer(BattleDIContainer container)
    {
        _container = container;
        _playerData = container.GetService<PlayerDataManager>();
    }

    void Update()
    {
        ShowMeTheMoney();
        SetScreen();
        AddAllSkill();
    }

    void ShowMeTheMoney()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _playerData.AddGold(1000);
            _playerData.AddGem(500);
        }
    }

    bool _isFullScreen;
    void SetScreen()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isFullScreen)
            {
                Screen.SetResolution(800, 480, false);
                _isFullScreen = false;
            }
            else
            {
                Screen.SetResolution(1920, 1080, true);
                _isFullScreen = true;
            }
        }
    }

    void AddAllSkill()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (SkillType skillType in System.Enum.GetValues(typeof(SkillType)).Cast<SkillType>().Where(x => x != SkillType.None))
                _playerData.SkillInventroy.AddSkill(new SkillAmountData(skillType, 10));
        }
    }
}
