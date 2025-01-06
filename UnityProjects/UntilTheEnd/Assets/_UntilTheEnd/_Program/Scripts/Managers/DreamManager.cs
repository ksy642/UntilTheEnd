using System.Collections;
using UnityEngine;

public class DreamManager : DontDestroySingleton<DreamManager>
{
    public bool dreaming = false; // �� ���õ� �߿��� bool ����
    public Material skybox_Awake;
    public Material skybox_Dream;

    private float _fogTime = 4.0f;    // �Ȱ� �������� �ð�
    private float _fogDensity = 0.013f; // �Ȱ� �е�

    [Header("----Timer----")]
    public float timer = 10; // ���� �ð� ..�׽�Ʈ������ 10�ʶ� �ص�

    public void DreamTest1()
    {
        Debug.Log("�׽�Ʈ1");
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        // �α� �߰�: ���� Ÿ�̸� ����
        Debug.Log($"[DreamManager] Timer: {timer}");

        if (timer <= 0)
        {
            timer = 0;
            Debug.Log("[DreamManager] Timer reached 0. Starting DreamLayer...");
            DreamLayer();
        }
    }

    #region �� ��
    public void DreamLayer()
    {
        dreaming = true;
        Debug.Log("[DreamManager] Entering DreamLayer. Dreaming state set to true.");
        Dreaming();
    }

    public void Dreaming()
    {
        Debug.Log("��������?");
        StartCoroutine(_FogOn(_fogDensity));
    }

    private IEnumerator _FogOn(float fogDensity)
    {
        Debug.Log($"�Ȱ� Ȯ���� : {fogDensity}");
        float theTime = 0f;
        float startDensity = RenderSettings.fogDensity;

        if (dreaming)
        {
            Debug.Log("��?");

            while (theTime < _fogTime)
            {
                theTime += Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Lerp(startDensity, fogDensity, theTime / _fogTime);
                yield return null;
            }

            RenderSettings.fog = true;
            // ��ȯ �Ϸ� �� �Ȱ� �е��� ���� ��ǥ ������ ����
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.skybox = skybox_Dream;
        }
        else
        {
            Debug.LogWarning("[DreamManager] Dreaming aborted. 'dreaming' state is false.");
        }
    }
    #endregion

    #region �Ͼ


    public void Awaking()
    {
        if (!dreaming)
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
        dreaming = false;
        Awaking();
    }

    public void ForAdrenaline()
    {
        AwakeLayer();
        timer = 10;
    }

    #endregion
}
