using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 멀티 확장성을 고려해서 인터페이스로 만듬 나중에 쓸일 없으면 폐기될지도
public interface IGoodsSeleter
{
    GameObject GetGoods();
}