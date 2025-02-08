using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MonsterPool : MonoBehaviour
{
    [System.Serializable]
    public class MonsterPrefab
    {
        public MonsterType type;
        public Monster monsterPrefab;
    }

    [SerializeField] private List<MonsterPrefab> monsterPrefabs = new List<MonsterPrefab>();
    private Dictionary<MonsterType, ObjectPool<Monster>> _monsterPools = new Dictionary<MonsterType, ObjectPool<Monster>>();

    [Header("현재 활성화된 몬스터 수와 최대 몬스터 수를 나타냄\n  (_ActiveMonsterCount는 건드리지말것!!)")]
    [SerializeField] private int _activeMonsterCount = 0; //  현재 활성화된 몬스터 수
    [SerializeField] private int _maxMonsters = 5;       // 최대 몬스터 개수

    // 타입을 다른 스크립트에서도 쉽게 확인할 수 있게 하는 프로퍼티
    public MonsterType[] AvailableMonsterTypes
    {
        get
        {
            return _monsterPools.Keys.ToArray();
        }
    }

    // 현재 몬스터 개수를 체크해서 스폰 가능 여부를 알려준다.
    public bool CanSpawnMonster()
    {
        return _activeMonsterCount < _maxMonsters;
    }

    private void Start()
    {
        foreach (var monsterData in monsterPrefabs)
        {
            if (!_monsterPools.ContainsKey(monsterData.type))
            {
                _monsterPools[monsterData.type] = new ObjectPool<Monster>(monsterData.monsterPrefab, _maxMonsters);
            }
        }
    }

    public Monster GetMonster(MonsterType type, Vector3 position)
    {
        if (_activeMonsterCount >= _maxMonsters)
        {
            Debug.LogWarning($"[MonsterPool] 최대 몬스터 개수({_maxMonsters}) 도달! 스폰 불가");
            return null;
        }


        if (_monsterPools.ContainsKey(type))
        {
            Monster monster = _monsterPools[type].GetObject(position);

            if (monster != null)
            {
                _activeMonsterCount++;
                Debug.Log($"[MonsterPool] 활성화된 몬스터 수: {_activeMonsterCount}");
            }

            return monster;
        }

        Debug.LogWarning($"[MonsterPool] {type} 몬스터 타입을 찾을 수 없습니다!");
        return null;
    }

    public void ReturnMonster(MonsterType type, Monster monster)
    {
        if (_monsterPools.ContainsKey(type))
        {
            _monsterPools[type].ReturnObject(monster);
            _activeMonsterCount--;
        }
        else
        {
            Debug.LogWarning($"[MonsterPool] 풀에서 {type} 몬스터를 찾을 수 없습니다!");
        }
    }
}