using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter: INoiseFilter
{
    Noise noise = new Noise();
    private NoiseSettings.RidgeNoiseSettings settings;

    public RigidNoiseFilter(NoiseSettings.RidgeNoiseSettings settings)
    {
        this.settings = settings;
    }
    public float Evaluate(Vector3 pos)
    {
        float noiseValue = 0;
        float frequency = settings.baseFrequency;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.octaves; ++i)
        {
            float v = 1- Mathf.Abs(noise.Evaluate(pos * frequency + settings.center));
            v = Mathf.Pow(v, settings.power);
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);
            
            noiseValue += v * amplitude;
            frequency *= settings.frequency;
            amplitude *= settings.persistence;
        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue*settings.strength;
    }
}
