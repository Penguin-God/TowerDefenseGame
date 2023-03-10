using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class UnitSpanwer
    {
        readonly IInstantiater _instantiater;
        public UnitSpanwer(IInstantiater instantiater) => _instantiater = instantiater;

        protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();
        public event System.Action<Multi_TeamSoldier> OnSpawn = null;

        public Multi_TeamSoldier Spawn(int colorNum, int classNum) => Spawn(new UnitFlags(colorNum, classNum));
        public Multi_TeamSoldier Spawn(UnitColor color, UnitClass unitClass) => Spawn(new UnitFlags(color, unitClass));
        public Multi_TeamSoldier Spawn(UnitFlags flag)
        {
            var unit = _instantiater.Instantiate(PathBuilder.BuildUnitPath(flag)).GetComponent<Multi_TeamSoldier>();
            OnSpawn?.Invoke(unit);
            return unit;
        }
    }
}
