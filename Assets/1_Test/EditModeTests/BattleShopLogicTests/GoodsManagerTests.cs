using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ShopLogicTests
{
    public class GoodsManagerTests
    {
        readonly IReadOnlyList<string> _initialGoods = new List<string>
        {
            "Apple",
            "Banana",
            "Cherry"
        };

        public GoodsManager<string> CreateGoodsManager() => new GoodsManager<string>(new HashSet<string>(_initialGoods));

        [Test]
        public void ������_��ǰ��_�̾ƾ�_�ϸ�_�Ȱ���_��ǰ��_�ٽ�_������_��_��()
        {
            var sut = CreateGoodsManager();

            var result = sut.GetRandomGoods();

            CollectionAssert.Contains(_initialGoods, result);
            CollectionAssert.DoesNotContain(sut.GetRandomGoods(), result);
            CollectionAssert.DoesNotContain(sut.GetRandomGoods(), result);
        }
    }
}
