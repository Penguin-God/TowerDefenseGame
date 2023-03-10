using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class UnitSpanwer
    {
        readonly IInstantiater _instantiater;
        readonly ICollection<Multi_TeamSoldier> _unitCollection;
        public UnitSpanwer(IInstantiater instantiater, ICollection<Multi_TeamSoldier> unitCollection)
        {
             _instantiater = instantiater;
            _unitCollection = unitCollection;
        }

        protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();

        public Multi_TeamSoldier Spawn(int colorNum, int classNum) => Spawn(new UnitFlags(colorNum, classNum));
        public Multi_TeamSoldier Spawn(UnitColor color, UnitClass unitClass) => Spawn(new UnitFlags(color, unitClass));
        public Multi_TeamSoldier Spawn(UnitFlags flag)
        {
            var unit = _instantiater.Instantiate(PathBuilder.BuildUnitPath(flag)).GetComponent<Multi_TeamSoldier>();
            _unitCollection.Add(unit);
            return unit;
        }
    }
}
