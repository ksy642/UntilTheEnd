using System.Collections;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public MonsterType MonsterType { get; private set; } // 몬스터 타입 저장

    private static MonsterPool _monsterPool;
    private bool _isDead = false;


    // 모든 자식 클래스는 반드시 Attack() 메서드를 구현해야한다.
    public abstract void Attack();

    public void SetMonsterType(MonsterType monsterType)
    {
        // 몬스터 타입 설정 (Orc, Goblin 등)
        MonsterType = monsterType;
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        _isDead = false;
    }


    public virtual void Die()
    {
        Debug.LogWarning("Die동작");

        if (!_isDead)
        {
            _isDead = true;
            Debug.LogWarning($"{gameObject.name}이(가) 죽었다!");

            // 애니메이션 실행
            //if (animator != null)
            //{
            //    animator.SetTrigger("Die");
            //}

            // 일정 시간 후 풀로 반환 (부드러운 삭제)
            StartCoroutine(_ReturnToPool());
        }
    }

    private IEnumerator _ReturnToPool()
    {
        // 2초 후 풀에 반환
        yield return new WaitForSeconds(2f);

        if (_monsterPool != null)
        {
            // MonsterType을 사용하여 풀에 반환
            _monsterPool.ReturnMonster(MonsterType, this);
        }
        else
        {
            Debug.LogError("MonsterPool을 찾을 수 없습니다!");
        }
    }
}