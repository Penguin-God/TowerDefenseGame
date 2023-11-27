using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_Meteor : MonoBehaviour
{
    [SerializeField] float _speed = 50;

    readonly string ExpolsionPath = new ResourcesPathBuilder().BuildEffectPath(SkillEffectType.MeteorExplosion);
    Action<Multi_Enemy> _explosionAction = null;
    Rigidbody _rigid;
    
    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    WorldAudioPlayer _audioPlayer;
    ObjectSpot _spot;
    public void DependencyInject(Action<Multi_Enemy> hitAction, WorldAudioPlayer audioPlayer, ObjectSpot spot)
    {
        _explosionAction = hitAction;
        _audioPlayer = audioPlayer;
        _spot = spot;
    }
    public void ShotMeteor(Multi_Enemy target) => StartCoroutine(Co_ShotMeteor(target));
    IEnumerator Co_ShotMeteor(Multi_Enemy target)
    {
        //Managers.Sound.PlayEffect(EffectSoundType.RedMageSkill);
        _audioPlayer.PlayObjectEffectSound(_spot, EffectSoundType.RedMageSkill);
        Vector3 tempPos = target.transform.position;
        yield return new WaitForSeconds(1f);
        Shot(CalculateShotPath(target, tempPos));
    }
    void Shot(Vector3 dir)
    {
        _rigid.velocity = dir.normalized * _speed;
        Quaternion lookDir = Quaternion.LookRotation(dir);
        transform.rotation = lookDir;
    }

    Vector3 CalculateShotPath(Multi_Enemy enemy, Vector3 tempPos)
    {
        Vector3 targetPoint;
        if (enemy == null || enemy.enemyType == EnemyType.Tower || enemy.IsDead) targetPoint = tempPos;
        else targetPoint = enemy.transform.position + enemy.dir.normalized * (enemy as Multi_NormalEnemy).Speed;
        return (targetPoint - transform.position).normalized;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "World")
            MeteorExplosion();
    }

    void MeteorExplosion() // 메테오 폭발
    {
        if (_explosionAction != null)
        {
            _audioPlayer.PlayObjectEffectSound(_spot, EffectSoundType.MeteorExplosion);
            Managers.Resources.Instantiate(ExpolsionPath, transform.position).GetComponent<Multi_HitSkill>().SetHitActoin(_explosionAction);
            _explosionAction = null;
        }

        _rigid.velocity = Vector3.zero;
        Managers.Resources.Destroy(gameObject);
    }
}
