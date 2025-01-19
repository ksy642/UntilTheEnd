using UnityEngine;

namespace UntilTheEnd
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private string _targetSceneName = "MainTest";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(StringValues.Tag.player))
            {
                Debug.Log($"'{StringValues.Tag.player}'가 트리거를 밟았습니다. 페이드 효과와 함께 씬 '{_targetSceneName}'으로 이동합니다.");
                UIManager.instance.FadeToScene(_targetSceneName);
            }
        }
    }
}