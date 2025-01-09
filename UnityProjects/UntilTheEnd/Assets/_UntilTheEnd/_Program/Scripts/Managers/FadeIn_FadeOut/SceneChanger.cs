using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string _targetSceneName = "MainTest";
    [SerializeField] private string _targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            Debug.Log($"'{_targetTag}'가 트리거를 밟았습니다. 페이드 효과와 함께 씬 '{_targetSceneName}'으로 이동합니다.");
            FadeManager.instance.FadeToScene(_targetSceneName);
        }
    }
}
