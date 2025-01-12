using UnityEngine;

/// <summary>
/// 제일 처음에 소환돼서 모든 매니저들 관리하게 해주려함
/// 그냥 불러도 애들이 clone으로 튀어나올텐데 그것보단 그냥 직접 소환시켜서 쓰도록 하자
/// </summary>
public class GameManager : DontDestroySingleton<GameManager>
{
    public GameObject fadeManager;
    public GameObject dreamManager;
    public GameObject uiManager;












}
