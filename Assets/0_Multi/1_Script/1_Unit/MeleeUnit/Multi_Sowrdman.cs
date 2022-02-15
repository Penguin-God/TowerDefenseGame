using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Sowrdman : Multi_MeleeUnit
{
    [Header("기사 변수")]
    [SerializeField] GameObject trail;

    [PunRPC]
    public override void NormalAttack()
    {
        StartCoroutine("SwordAttack");
    }

    IEnumerator SwordAttack()
    {
        base.StartAttack();

        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(0.8f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        trail.SetActive(false);

        base.NormalAttack();
    }
}
