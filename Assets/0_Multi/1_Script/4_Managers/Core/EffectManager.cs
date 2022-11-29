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
    [SerializeField] EffectType _effectType;
    [SerializeField] string _name;
    [SerializeField] string _path;

    public EffectType EffectType => _effectType;
    public string Name => _name;
    public string Path => _path;
}

public class EffectManager
{
    Dictionary<string, string> _nameByPath = new Dictionary<string, string>();

    public void Init(IInstantiater instantiater = null)
    {
        foreach (var data in CsvUtility.CsvToArray<EffectData>(Multi_Managers.Resources.Load<TextAsset>("Data/EffectData").text))
        {
            _nameByPath.Add(data.Name, data.Path);
            switch (data.EffectType)
            {
                case EffectType.GameObject:
                    Multi_Managers.Pool.CreatePool_InGroup(data.Path, 3, "Effects", instantiater);
                    break;
            }
        }
    }

    public TargetTracker ChaseToTarget(string name, Transform target, Vector3 offset)
    {
        TargetTracker tracker = LoadObject(name).GetOrAddComponent<TargetTracker>();
        tracker.SetInfo(target, offset);
        return tracker;
    }

    // 이걸 호출하는 쪽에서 All이나 Other로 튕기면 됨. 대신 그때 서로가 풀링이 되어 있어야 함.
    public void PlayParticle(string name, Vector3 pos)
    {
        ParticlePlug particle = LoadParticle(name);
        if (particle == null) return;
        particle.gameObject.SetActive(true);
        particle.gameObject.transform.position = pos;
        particle.PlayParticle();
    }

    public void ChangeMaterial(string name, MeshRenderer mesh)
        => mesh.material = LoadMaterial(name);

    public void ChangeAllMaterial(string name, Transform transform)
        => transform.GetComponentInChildren<MeshRenderer>().material = LoadMaterial(name);

    public void ChangeColor(byte r, byte g, byte b, Transform transform)
        => transform.GetComponentInChildren<MeshRenderer>().material.color = new Color32(r, g, b, 255);

    GameObject LoadObject(string name) => Multi_Managers.Resources.PhotonInsantiate(_nameByPath[name], Vector3.zero);
    ParticlePlug LoadParticle(string name) => LoadObject(name).GetOrAddComponent<ParticlePlug>();

    Dictionary<string, Material> _nameByMaterial = new Dictionary<string, Material>();
    Material LoadMaterial(string name)
    {
        if (_nameByMaterial.ContainsKey(name) == false)
            _nameByMaterial.Add(name, Multi_Managers.Resources.Load<Material>(_nameByPath[name]));
        return _nameByMaterial[name];
    }
}
