using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;  // 웨이브에서 스폰할 적 프리팹 배열
    public int enemyCount = 10;        // 웨이브당 적의 수
    public float spawnInterval = 0.5f; // 적 스폰 간격
    public bool isBossWave = false;
}