using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// 일단 목적은 지하철역에서 방활할 때 한쪽길로만 쭉 따라서 가는 경우 뒤돌려보내는 느낌으로 만들껀데...
/// 뭐 대충 계단으로 치면 5층에서 4층내려간 후 4층에서 3층갔는데 4층으로 되돌아오는 느낌이랄까
/// </summary>
public class TeleportSpot : MonoBehaviour
{
    public Transform spotA;
    public Transform spotB;
    public CinemachineCamera cinemachineCamera; // 시네머신 카메라

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 텔레포트를 시킬 때 캐릭터 컨트롤러를 꺼줘야함!
            CharacterController characterController = other.GetComponent<CharacterController>();
            characterController.enabled = false;

            other.transform.position = spotB.position;

            if (cinemachineCamera != null)
            {
                cinemachineCamera.ForceCameraPosition(spotB.position, Quaternion.identity);
            }

            // 이동 다 시켰으니 다시 켜주고
            characterController.enabled = true;
        }
    }
}