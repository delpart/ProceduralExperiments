using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Smooth,
        Ridge
    };
    public FilterType filterType;
    [ConditionalHide("filterType", (int)FilterType.Smooth)]
    public SmoothNoiseSettings smoothNoiseSettings;
    [ConditionalHide("filterType", (int)FilterType.Ridge)]
    public RidgeNoiseSettings ridgeNoiseSettings;
    
    [System.Serializable]
    public class SmoothNoiseSettings
    {
        public float strength = 1;
        [Range(1, 16)]
        public int octaves = 1;

        public float persistence = .5f;
        public float frequency = 2;
        public float baseFrequency = 1;
        public float minValue = 1;
        public Vector3 center;
    }
    
    [System.Serializable]
    public class RidgeNoiseSettings : SmoothNoiseSettings
    {
        public float weightMultiplier = .8f;
        public float power = 2f;
    }


}
