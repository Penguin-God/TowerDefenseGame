using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPathLocationFinder
{
    public MonsterPathLocationFinder(Vector3[] waypoints) => Waypoints = waypoints;

    readonly Vector3[] Waypoints; // 웨이포인트 위치를 저장하는 리스트

    public Vector3 GetTrapSpawnLocation()
    {
        // 먼저, 랜덤한 웨이포인트를 하나 선택합니다.
        int index = Random.Range(0, Waypoints.Length);
        Vector3 pointA = Waypoints[index];

        // 선택한 웨이포인트와 연결된 다음 웨이포인트를 찾습니다. 
        // 웨이포인트가 리스트의 끝에 있다면 첫 번째 웨이포인트를 선택합니다.
        Vector3 pointB = Waypoints[(index + 1) % Waypoints.Length];

        // 두 웨이포인트 사이의 랜덤한 위치를 선택합니다.
        Vector3 spawnPosition = Vector3.Lerp(pointA, pointB, Random.value);
        return spawnPosition;
    }
}
