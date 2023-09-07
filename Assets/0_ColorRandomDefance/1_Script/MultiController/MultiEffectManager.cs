using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiEffectManager : MonoBehaviourPun
{
    EffectManager _effectManager;
    public void Inject(EffectManager effectManager) => _effectManager = effectManager;
    public void PlayOneShotEffect(string name, Vector3 pos) => photonView.RPC(nameof(PlayParticle), RpcTarget.All, name, pos);
    [PunRPC]
    void PlayParticle(string name, Vector3 pos) => _effectManager.PlayOneShotEffect(name, pos);
}
