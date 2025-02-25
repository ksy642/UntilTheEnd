using System.Collections;
using UnityEngine;

public class DreamManager : DontDestroySingleton<DreamManager>
{
    public bool isDreaming = false; // 꿈 관련된 중요한 bool 변수

    [Header("----Timer----")]
    public float timer = 10; // 제한 시간 ..테스트용으로 10초라 해둠
    [SerializeField] private float skyBox_RotationSpeed = 1.3f; // 회전 속도 (초당 각도)

    [Header("안개 세팅")]
    [SerializeField] public Material skybox_Awake;
    [SerializeField] public Material skybox_Dream;
    [SerializeField] private float _fogTime = 4.0f;    // 안개 차오르는 시간
    [SerializeField] private float _fogDensity = 0.013f; // 안개 밀도

    private void Update()
    {
        _SkyboxRotation();

        timer -= Time.deltaTime;

        // 로그 추가: 현재 타이머 상태
        //Debug.Log($"[DreamManager] Timer: {timer}");

        if (timer <= 0)
        {
            timer = 0;
            //Debug.Log("[DreamManager] Timer reached 0. Starting DreamLayer...");
            DreamLayer();
        }
    }

    private void _SkyboxRotation()
    {
        float degree = Time.time * skyBox_RotationSpeed; // 현재 시간에 따른 회전 각도 계산
        degree %= 360; // 각도를 360도 범위 내로 유지
        RenderSettings.skybox.SetFloat("_Rotation", degree); // Skybox의 _Rotation 속성 설정
    }



    #region 꿈 속
    public void DreamLayer()
    {
        isDreaming = true;
        Dreaming();
    }

    public void Dreaming()
    {
        StartCoroutine(_FogOn(_fogDensity));
    }

    private IEnumerator _FogOn(float fogDensity)
    {
        float theTime = 0f;
        float startDensity = RenderSettings.fogDensity;

        if (isDreaming)
        {
            while (theTime < _fogTime)
            {
                theTime += Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Lerp(startDensity, fogDensity, theTime / _fogTime);
                yield return null;
            }

            RenderSettings.fog = true;
            // 전환 완료 후 안개 밀도를 최종 목표 값으로 설정
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.skybox = skybox_Dream;
        }
        else
        {
            Debug.LogWarning("[DreamManager] Dreaming aborted. 'dreaming' state is false.");
        }
    }
    #endregion

    #region 일어나


    public void Awaking()
    {
        if (!isDreaming)
        {
            Debug.Log("[DreamManager] Starting Awaking sequence...");
            StartCoroutine(_FogOff(0.001f));
        }
    }

    private IEnumerator _FogOff(float fogDensity)
    {
        Debug.Log($"[DreamManager] FogOff Coroutine started. Target fog density: {fogDensity}");
        yield return new WaitForSeconds(_fogTime);

        RenderSettings.fog = false;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.skybox = skybox_Awake;
        Debug.Log("[DreamManager] Awaking complete. Fog density set and skybox changed.");
    }



    public void AwakeLayer()
    {
        isDreaming = false;
        Awaking();
    }

    public void ForAdrenaline()
    {
        AwakeLayer();
        timer = 10;
    }

    #endregion
}
