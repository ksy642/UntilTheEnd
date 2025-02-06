using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private MonsterPool _monsterPool;
    [SerializeField] private MonsterType[] _monsterTypes =
        {
        MonsterType.Orc,
        MonsterType.Goblin
    };

    private MonsterFactory _monsterFactory;
    [SerializeField] private int _activeMonsterCount = 0; //✅ 현재 활성화된 몬스터 수
    [SerializeField] private int _maxMonsters = 5; // 최대 몬스터 개수

    private void Start()
    {
        _monsterFactory = new MonsterFactory(_monsterPool);
        InvokeRepeating(nameof(_SpawnMonster), 2f, 5f);
    }

    private void _SpawnMonster()
    {
        if (_activeMonsterCount >= _maxMonsters)
        {
            Debug.LogWarning($" 몬스터 최대 개수({_maxMonsters}) 도달! 스폰 중지!");
            CancelInvoke(nameof(_SpawnMonster));
            return;
        }

        // 랜덤하게 enum 타입을 선택
        MonsterType monsterType = _monsterTypes[Random.Range(0, _monsterTypes.Length)];


        // 포지션도 내가 직접 설정해줘야됨 이따가!!
        Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));



        Monster monster = _monsterFactory.CreateMonster(monsterType, spawnPosition);

        if (monster != null)
        {
            _activeMonsterCount++;
            Debug.Log("활성화된 몬스터 수 증가 : " + _activeMonsterCount);
        }
        else
        {
            Debug.Log($"{monsterType} 풀에 남은 객체가 없어서 소환을 멈춥니다!");
        }
    }
}
