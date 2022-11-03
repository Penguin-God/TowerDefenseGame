using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EffectType
{
    GameObject,
    Particle,
    Material,
}

public class EffectData
{
    EffectType _effectType;
    string _name;
    string _path;

    public EffectType EffectType => _effectType;
    public string Name => _name;
    public string Path => _path;
}

public class EffectManager
{
    Dictionary<string, string> _nameByPath = new Dictionary<string, string>();
    // 풀링하기
    Dictionary<string, GameObject> _nameByObject = new Dictionary<string, GameObject>();
    Dictionary<string, ParticleSystem> _nameByParticle = new Dictionary<string, ParticleSystem>();
    Dictionary<string, Material> _nameByMaterial = new Dictionary<string, Material>();

    public void Init()
    {
        foreach (var data in CsvUtility.CsvToArray<EffectData>(Multi_Managers.Resources.Load<TextAsset>("Data/EffectData").text))
        {
            _nameByPath.Add(data.Name, data.Path);
            switch (data.EffectType)
            {
                case EffectType.GameObject:
                    _nameByObject.Add(data.Name, Resources.Load<GameObject>(data.Path));
                    break;
                case EffectType.Particle:
                    _nameByParticle.Add(data.Name, Resources.Load<GameObject>(data.Path).GetComponent<ParticleSystem>());
                    break;
                case EffectType.Material:
                    _nameByMaterial.Add(data.Name, Resources.Load<Material>(data.Path));
                    break;
            }
        }
    }

    public void ChaseToTarget(string name, Transform target, Vector3 offset)
    {
        GameObject chaser = Multi_Managers.Resources.Load<GameObject>(_nameByPath[name]);
        // chaser.AddComponent<>().SetInfo(target, offset) 추적 컴포넌트 만들기
    }

    public void PlayParticle(string name, Vector3 pos)
    {
        ParticleSystem particle = _nameByParticle[name];
        particle.gameObject.transform.position = pos;
        particle.Play();
    }

    public void ChangeMaterial(string name, MeshRenderer mesh)
        => mesh.material = _nameByMaterial[name];

    public void ChangeAllMaterial(string name, Transform transform)
        => transform.GetComponentInChildren<MeshRenderer>().material = _nameByMaterial[name];

    public void ChangeColor(byte r, byte g, byte b, Transform transform)
        => transform.GetComponentInChildren<MeshRenderer>().material.color = new Color32(r, g, b, 255);
}
