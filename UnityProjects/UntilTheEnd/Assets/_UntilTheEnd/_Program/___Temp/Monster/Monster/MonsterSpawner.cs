using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private MonsterPool _monsterPool;
    private MonsterFactory _monsterFactory;

    private void Start()
    {
        _monsterFactory = new MonsterFactory(_monsterPool);

        InvokeRepeating(nameof(SpawnMonster), 2f, 5f); // 2초 후 시작, 5초마다 반복
    }

    private void SpawnMonster()
    {
        // 50% 확률로 몬스터 생성
        string monsterType = Random.value > 0.5f ? "Orc" : "Goblin";

        Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        Monster monster = _monsterFactory.CreateMonster(monsterType, spawnPosition);

        Debug.Log($"{monsterType} 스폰됨!");
    }   
}
