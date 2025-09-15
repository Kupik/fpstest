using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{



    public static CameraManager instance;


    [SerializeField] private CinemachineVirtualCamera virtualCamera;


    public void Awake()
    {
      
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            if (virtualCamera == null)
            {
                Debug.LogWarning("CameraManager: nu a fost gasita cinemachine virtual camera");
            }
        }
    
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
    }

    public void SetCameraToFollowPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
    }
}