using UnityEngine;

public class Goblin : Monster // 테스트용 파란색
{
    public float health = 100f;

    private void Awake()
    {
        SetMonsterType(MonsterType.Goblin);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Goblin 맞음! 남은 체력: {health}");

        if (health <= 0)
        {
            Die();
        }
    }



    public override void Attack()
    {
        Debug.Log("고블린이 공격했다!");
    }

    public override void Die()
    {
        base.Die();  // 부모의 Die() 로직 실행
        Debug.Log("고블린이 포효하며 쓰러진다...!");
        this.gameObject.SetActive(false);
    }
}