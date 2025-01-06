using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class PlayerSystem : MonoBehaviour
{
    [Header("Spawn Player")]
    public Transform spawnPoint;
    [SerializeField] private GameObject _playerPrefab;
    //public CharacterNetworkMovement characterNetworkMovement;

    [Header("Camera")]
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Vector3 targetAddOffset;
}
