using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch (settings.filterType)
        {
            case NoiseSettings.FilterType.Smooth:
                return new SmoothNoiseFilter(settings.smoothNoiseSettings);
            case NoiseSettings.FilterType.Ridge:
                return new RigidNoiseFilter(settings.ridgeNoiseSettings);
            default:
                return new SmoothNoiseFilter(settings.smoothNoiseSettings);
        }
    }
}
