using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance = null;

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin perlin;

    private List<Perlin> perlins = new();

    private void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        Instance = this;

        cam = GetComponent<CinemachineVirtualCamera>();
        perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        perlin.m_AmplitudeGain = 0;
        perlin.m_FrequencyGain = 0;

        for(int i = 0; i < perlins.Count; ++i)
        {
            Perlin value = perlins[i];

            if(perlin.m_AmplitudeGain < value.amplitude)
            {
                perlin.m_AmplitudeGain = value.amplitude;
                perlin.m_FrequencyGain = value.frequency;
            }

            value.time -= Time.deltaTime;

            if (value.time < 0) perlins.RemoveAt(i--);
        }
    }

    public void AddPerlin(float amplitude, float frequency, float time)
    {
        Perlin value = new Perlin(amplitude, frequency, time);
        perlins.Add(value);
    }

    public void AddPerlin(Perlin perlin)
    {
        perlins.Add(perlin);
    }
}

public class Perlin
{
    public float amplitude;
    public float frequency;
    public float time;

    public Perlin(float amplitude, float frequency, float time)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.time = time;
    }
}
