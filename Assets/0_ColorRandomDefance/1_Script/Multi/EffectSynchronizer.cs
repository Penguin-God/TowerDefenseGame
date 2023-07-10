using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EffectSynchronizer : MonoBehaviourPun
{
    public void PlayOneShotEffect(string name, Vector3 pos)
    {
        photonView.RPC(nameof(RPC_PlayOneShotEffect), RpcTarget.All, name, pos);
    }

    [PunRPC]
    void RPC_PlayOneShotEffect(string name, Vector3 pos)
    {
        ParticlePlug particle = LoadEffect(name).GetOrAddComponent<ParticlePlug>();
        particle.gameObject.transform.position = pos;
        particle.PlayParticle();
    }

    GameObject LoadEffect(string name) => Managers.Resources.Instantiate($"Effects/{name}");
}
