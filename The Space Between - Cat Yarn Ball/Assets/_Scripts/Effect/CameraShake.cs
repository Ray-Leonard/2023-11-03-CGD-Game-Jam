using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private float shakeIntensity = 2f;

    private CinemachineBasicMultiChannelPerlin noise;

    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;

        StatManager.Instance.OnTakeDamage += ShakeCamera;
    }

    private void OnDestroy()
    {
        StatManager.Instance.OnTakeDamage -= ShakeCamera;
    }


    private async void ShakeCamera()
    {
        noise.m_AmplitudeGain = shakeIntensity;
        noise.m_FrequencyGain = shakeIntensity;

        await Task.Delay(Mathf.RoundToInt(1000f * shakeDuration));

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
