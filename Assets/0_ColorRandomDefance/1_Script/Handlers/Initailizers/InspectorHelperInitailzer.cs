using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorHelperInitailzer : MonoBehaviour
{
    T Get<T>() => GetComponentInChildren<T>();

    public void Set(BattleDIContainer container)
    {
        container.Inject(Get<PrefabSpawner>());
        Get<UserSkillTestButtons>().DependencyInject(container);
    }
}
