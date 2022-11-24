using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlug : MonoBehaviour
{
    ParticleSystem _particle;
    void Awake()
    {
        _particle = gameObject.GetOrAddComponent<ParticleSystem>();
    }

    public void PlayParticle()
    {
        _particle.Play();
        StartCoroutine(Co_InAvtive(_particle.main.duration));
    }

    IEnumerator Co_InAvtive(float time)
    {
        yield return new WaitForSeconds(time);
        if (GetComponent<Poolable>() == null)
            Destroy(gameObject);
        else
            Multi_Managers.Pool.Push(gameObject);
    }
}
