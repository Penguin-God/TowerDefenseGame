using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Dictionary<Transform, TargetTracker> _targetByTrackers = new Dictionary<Transform, TargetTracker>();
    public IReadOnlyDictionary<Transform, TargetTracker> TargetByTrackers => _targetByTrackers;
    public TargetTracker TrackingToTarget(string name, Transform target, Vector3 offset)
    {
        TargetTracker tracker = LoadObject(name).GetOrAddComponent<TargetTracker>();
        tracker.SetInfo(target, offset);
        _targetByTrackers.Add(target, tracker);
        return tracker;
    }

    public void StopTargetTracking(Transform target)
    {
        if (_targetByTrackers.TryGetValue(target, out TargetTracker tracker) == false)
            return;
        Managers.Resources.Destroy(tracker.gameObject);
        _targetByTrackers.Remove(target);
    }
    
    public void PlayOneShotEffect(string name, Vector3 pos)
    {
        ParticlePlug particle = LoadParticle(name);
        if (particle == null) return;
        particle.gameObject.SetActive(true);
        particle.gameObject.transform.position = pos;
        particle.PlayParticle();
    }

    public void ChangeAllMaterial(string name, Transform transform)
        => transform.GetComponentInChildren<MeshRenderer>().material = LoadMaterial(name);

    GameObject LoadObject(string name) => Managers.Resources.Instantiate($"Effects/{name}");
    ParticlePlug LoadParticle(string name) => LoadObject(name).GetOrAddComponent<ParticlePlug>();

    Dictionary<string, Material> _nameByMaterial = new Dictionary<string, Material>();
    Material LoadMaterial(string name)
    {
        if (_nameByMaterial.ContainsKey(name) == false)
            _nameByMaterial.Add(name, Managers.Resources.Load<Material>($"Materials/{name}"));
        return _nameByMaterial[name];
    }
}
