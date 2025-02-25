using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 컷씬 테스트용으로 설치해둔거
/// </summary>
public class TimelineCutScene : MonoBehaviour
{
    public PlayableDirector playableDirector;

    //public GameObject playerPrefab;
    //public Transform spawnPoint;

    //public GameObject cutsceneCanvas; // 대사창 같은거

    private void Start()
    {
        // 컷씬 끝날 때 이벤트 설정
        playableDirector.stopped += _OnCutsceneEnd;

        Debug.Log("컷씬이 시작합니다.");
        playableDirector.Play();
    }

    private void _OnCutsceneEnd(PlayableDirector pd)
    {
        Debug.Log("컷씬이 끝났습니다.");

       // cutsceneCanvas.SetActive(false);
       // _SpawnPlayer();
    }

    //public void SkipCutscene()
    //{
    //    playableDirector.Stop();
    //    cutsceneCanvas.SetActive(false);
    //    _SpawnPlayer();
    //}

    //private void _SpawnPlayer()
    //{
    //    Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    //}
}
