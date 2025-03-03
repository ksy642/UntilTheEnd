using System.Collections;
using UnityEngine;

namespace UntilTheEnd
{
    public class MainSceneScript : MonoBehaviour
    {
        [Header("코루틴 : 처음에 캐릭터 못움직이게 하는 시간")]
        public float testTime = 0.0f;

        private void Start()
        {
            // 컷씬이 들어올 자리인데 일단 테스트용으로 코루틴으로 딜레이 줘보자
            StartCoroutine(CutSceneTest());
        }

        private IEnumerator CutSceneTest()
        {
            Debug.LogWarning ("곧 코루틴이 동작합니다. 3초 카운트 + 캐릭터 안움직여질껄?");
            yield return new WaitForSeconds(testTime);
            Debug.LogWarning("코루틴이 끝났습니다. 캐릭터 움직여보실?");

            GameManager.instance.playerCanMove = true;
            GameManager.instance.isLobby = false;
        }
    }
}