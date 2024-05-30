using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    public float shakeIntensity = 1f;
    public float shakeDuration = 0.2f;

    public float timer;
    public bool isShaking = false;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    // Start is called before the first frame update
    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopShake();
    }


    void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        cbmcp.m_AmplitudeGain = shakeIntensity;
        
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            StopShake();
        }
    }

    void StopShake()
    {
        isShaking = false;
        cbmcp.m_AmplitudeGain = 0f;
        timer = 0f;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            ShakeCamera();
        }
    }
}
