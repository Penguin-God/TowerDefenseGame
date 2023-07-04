using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPathLocationFinder
{
    public MonsterPathLocationFinder(Vector3[] waypoints) => Waypoints = waypoints;

    readonly Vector3[] Waypoints; // ��������Ʈ ��ġ�� �����ϴ� ����Ʈ

    public Vector3 GetTrapSpawnLocation()
    {
        // ����, ������ ��������Ʈ�� �ϳ� �����մϴ�.
        int index = Random.Range(0, Waypoints.Length);
        Vector3 pointA = Waypoints[index];

        // ������ ��������Ʈ�� ����� ���� ��������Ʈ�� ã���ϴ�. 
        // ��������Ʈ�� ����Ʈ�� ���� �ִٸ� ù ��° ��������Ʈ�� �����մϴ�.
        Vector3 pointB = Waypoints[(index + 1) % Waypoints.Length];

        // �� ��������Ʈ ������ ������ ��ġ�� �����մϴ�.
        Vector3 spawnPosition = Vector3.Lerp(pointA, pointB, Random.value);
        return spawnPosition;
    }
}
