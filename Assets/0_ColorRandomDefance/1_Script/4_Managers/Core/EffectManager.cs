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
    public string Path => _path;
}

public class EffectManager
{
    Dictionary<TargetTracker, Transform> _trackersByTarget = new Dictionary<TargetTracker, Transform>();
    public TargetTracker TrackingTarget(string name, Transform target, Vector3 offset)
    {
        TargetTracker tracker = LoadObject(name).GetOrAddComponent<TargetTracker>();
        tracker.SetInfo(target, offset);
        _trackersByTarget.Add(tracker, target);
        return tracker;
    }
    public void CancleTracking(TargetTracker tracker)
    {
        if (_trackersByTarget.ContainsKey(tracker) == false) return;
        Managers.Resources.Destroy(tracker.gameObject);
        _trackersByTarget.Remove(tracker);
    }

    public void PlayOneShotEffect(string name, Vector3 pos)
    {
        ParticlePlug particle = LoadParticle(name);
        if (particle == null) return;
        particle.gameObject.SetActive(true);
        particle.gameObject.transform.position = pos;
        particle.PlayParticle();
    }

    public void ChangeAllMaterial(string name, Transform transform) => transform.GetComponentInChildren<MeshRenderer>().material = LoadMaterial(name);

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
