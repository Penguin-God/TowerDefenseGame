﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace Tests
{
    public class ResourcesPathBuilderTests
    {
        ResourcesPathBuilder _pathBuilder = new ResourcesPathBuilder();

        [Test]
        public void BuildUnitPath()
        {
            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                    AssertResourcesLoad(_pathBuilder.BuildUnitPath(new UnitFlags(color, unitClass)));
            }
        }

        [Test]
        public void BuildNormalMonstersPath()
        {
            var monsterNames = new string[] { "Archer", "Mage", "Spearman", "Swordman" };
            for (int i = 0; i < monsterNames.Length; i++)
                AssertResourcesLoad(_pathBuilder.BuildMonsterPath(i));
        }

        [Test]
        public void BuildBossMonstersPath()
        {
            var monsterNames = new string[] { "Archer", "Mage", "Spearman", "Swordman" };

            for (int i = 0; i < monsterNames.Length; i++)
                AssertResourcesLoad(_pathBuilder.BuildBossMonsterPath(i));
        }

        [Test]
        public void BuildTowersPath()
        {
            const int MAX_TOWER_LEVEL = 6;
            for (int i = 1; i < MAX_TOWER_LEVEL + 1; i++)
                AssertResourcesLoad(_pathBuilder.BuildEnemyTowerPath(i));
        }

        [Test]
        public void BuildProjectilePath()
        {
            var throwableUnits = new UnitClass[] { UnitClass.Archer, UnitClass.Spearman, UnitClass.Mage };

            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in throwableUnits)
                    AssertResourcesLoad(_pathBuilder.BuildUnitWeaponPath(new UnitFlags(color, unitClass)));
            }
        }

        [Test]
        public void BuildEffetPathTests()
        {
            foreach (SkillEffectType type in Enum.GetValues(typeof(SkillEffectType)))
                AssertResourcesLoad(_pathBuilder.BuildEffectPath(type));
        }

        void AssertResourcesLoad(string path) => Assert.NotNull(Resources.Load<GameObject>(GetPrefabPath(path)));
        string GetPrefabPath(string path) => path.Contains("Prefabs/") ? path : $"Prefabs/{path}";
    }
}
