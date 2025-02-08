using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private MonsterPool _monsterPool;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private bool _isSpawning = false;

    private void Start()
    {
        // 2초 뒤 시작, 5초마다 생성
        InvokeRepeating(nameof(_SpawnMonster), 2f, 2f);
    }

    private void Update()
    {
        // 몬스터 개수 초과 시 스폰 중지 (한 번만 실행되도록 처리)
        if (!_monsterPool.CanSpawnMonster() && _isSpawning)
        {
            Debug.Log("MonsterSpawner 몬스터 개수 제한 도달! 스폰 중지");
            CancelInvoke(nameof(_SpawnMonster));
            _isSpawning = false; // 스폰 중지 상태로 변경
        }


        // 몬스터가 반환되면 다시 스폰 시작
        //else if (_monsterPool.CanSpawnMonster() && !_isSpawning)
        //{
        //    Debug.Log("@@MonsterSpawner 몬스터 개수 감소! 스폰 재개");
        //    InvokeRepeating(nameof(_SpawnMonster), 2f, 2f);
        //    _isSpawning = true; // 다시 스폰 가능하도록 변경
        //}
    }

    private void _SpawnMonster()
    {
        if (_spawnPoints.Length == 0)
        {
            Debug.LogWarning("[MonsterSpawner] 스폰 위치가 설정되지 않았습니다!");
            return;
        }

        // 최대 개수 초과 시 스폰 중지
        if (!_monsterPool.CanSpawnMonster())
        {
            Debug.Log("[MonsterSpawner] 몬스터 개수 제한 도달! 스폰 중지");
            CancelInvoke(nameof(_SpawnMonster));
            _isSpawning = false;
            return;
        }

        // MonsterPool에서 등록된 몬스터 타입을 랜덤하게 가져옴
        MonsterType[] availableTypes = _monsterPool.AvailableMonsterTypes;
        MonsterType monsterType = availableTypes[Random.Range(0, availableTypes.Length)];



        // 랜덤하게 스폰 위치 선택 후 해당 위치 주변에서 랜덤하게 생성되게 설정
        Transform selectedSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Vector3 spawnPosition = selectedSpawnPoint.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));

        Monster monster = _monsterPool.GetMonster(monsterType, spawnPosition);

        if (monster == null)
        {
            Debug.Log($"{monsterType} 몬스터를 생성할 수 없음!");
        }
    }
}