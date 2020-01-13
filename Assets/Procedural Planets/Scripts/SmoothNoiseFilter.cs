using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothNoiseFilter: INoiseFilter
{
    Noise noise = new Noise();
    private NoiseSettings.SmoothNoiseSettings settings;

    public SmoothNoiseFilter(NoiseSettings.SmoothNoiseSettings settings)
    {
        this.settings = settings;
    }
    public float Evaluate(Vector3 pos)
    {
        float noiseValue = 0;
        float frequency = settings.baseFrequency;
        float amplitude = 1;

        for (int i = 0; i < settings.octaves; ++i)
        {
            float v = noise.Evaluate(pos * frequency + settings.center);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.frequency;
            amplitude *= settings.persistence;
        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue*settings.strength;
    }
}
