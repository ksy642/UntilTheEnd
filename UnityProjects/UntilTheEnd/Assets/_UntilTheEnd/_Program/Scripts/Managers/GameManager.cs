using UnityEngine;

/// <summary>
/// 제일 처음에 소환돼서 모든 매니저들 관리하게 해주려함
/// 그냥 불러도 애들이 clone으로 튀어나올텐데 그것보단 그냥 직접 소환시켜서 쓰도록 하자
/// </summary>
public class GameManager : DontDestroySingleton<GameManager>
{
    [Header("Prefab")]
    public GameObject dreamManagerPrefab;
    public GameObject uiManagerPrefab;

    [Header("소환된 Clone")]
    [SerializeField] private GameObject spawnedDreamManager;
    [SerializeField] private GameObject spawnedUIManager;


    // 매니저들이 있으면 삭제시키고 재소환 혹은 대기
    // 없으면 소환시키고

    private void Start()
    {
        Debug.Log("동작 할까요?");

        // UIManager가 존재하지 않으면 새로 생성
        if (spawnedUIManager == null)
        {
            Debug.Log("동작 할까요?22");
            spawnedUIManager = Instantiate(uiManagerPrefab);
        }







    }
}
