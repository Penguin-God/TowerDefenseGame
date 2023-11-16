using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombineMultiController : MonoBehaviourPun
{
    UnitCombineSystem _combineSystem;
    UnitManagerController _unitManager;
    Multi_NormalUnitSpawner _spawner;
    BattleEventDispatcher _dispatcher;
    UnitCombineNotifier _combineResultNotifier;
    public void DependencyInject
        (UnitCombineSystem combineSystem, UnitManagerController unitManager, Multi_NormalUnitSpawner spawner, BattleEventDispatcher dispatcher, TextShowAndHideController textController)
    {
        _unitManager = unitManager;
        _spawner = spawner;
        _combineSystem = combineSystem;
        _dispatcher = dispatcher;
        _combineResultNotifier = new UnitCombineNotifier(textController);
    }

    public bool TryCombine(UnitFlags targetFlag) => TryCombine(targetFlag, PlayerIdManager.Id);
    public bool TryCombine(UnitFlags targetFlag, byte id)
    {
        if (CanCombine(targetFlag, id))
        {
            photonView.RPC(nameof(Combine), RpcTarget.MasterClient, targetFlag, id);
            // 클라에서 이벤트 호출하는게 과연 보안상 괜찮은지는 미지수
            _dispatcher.NotifyUnitCombine(targetFlag);
            _combineResultNotifier.ShowCombineSuccessText(targetFlag);
            return true;
        }
        else
        {
            _combineResultNotifier.ShowCombineFaliedText();
            return false;
        }
    }

    public bool CanCombine(UnitFlags targetFlag, byte id) => _combineSystem.CheckCombineable(targetFlag, _unitManager.WorldUnitManager.GetUnitFlags(id));

    [PunRPC]
    void Combine(UnitFlags targetFlag, byte id)
    {
        if (CanCombine(targetFlag, id) == false) return;

        foreach (var needFlag in _combineSystem.GetNeedFlags(targetFlag))
            _unitManager.GetUnit(id, needFlag).Dead();

        _spawner.Spawn(targetFlag, id);
    }
}
