
using UnityEngine;
using Cinemachine;
using System;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] [Range(0f,10f)] private float defalut_camera = 6f;
    [SerializeField] [Range(0f,10f)] private float default_distance_min_camera = 1f;
    [SerializeField] [Range(0f,10f)] private float defautl_distance_max_camera = 6f;

    [SerializeField][Range(0f, 10f)] private float smothing = 4f; // Facem miscarile mai lente pentru nu a simit colturile

    [SerializeField][Range(0f, 10f)] private float zoom_sensivity = 1f;

    private CinemachineFramingTransposer framing_transposer;
    private CinemachineInputProvider input_provider;

    private float current_target_distance;
    private void Awake()
    {
        framing_transposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        input_provider = GetComponent<CinemachineInputProvider>();

        current_target_distance = defalut_camera;
    }


    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        float zoom_value = input_provider.GetAxisValue(2) * zoom_sensivity;
        current_target_distance = Mathf.Clamp(current_target_distance + zoom_value , default_distance_min_camera, defautl_distance_max_camera);

        float currentDistance = framing_transposer.m_CameraDistance;

        if (currentDistance == current_target_distance)
        {
            return;
        }

        //ca sa scadem

        float lerped_zoom_value = Mathf.Lerp(currentDistance, current_target_distance, smothing * Time.deltaTime);

        framing_transposer.m_CameraDistance = lerped_zoom_value;


    }
}