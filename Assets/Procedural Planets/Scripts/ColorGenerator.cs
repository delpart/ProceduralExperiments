using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class ColorGenerator
{
    ColorSettings settings;
    private Texture2D texture;
    private const int textureResolution = 50;
    private INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution*2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max, 0 , 0));
    }

    public float BiomeRatioFromPosition(Vector3 pos)
    {
        float heightRatio = (pos.y + 1) / 2f;
        heightRatio += (biomeNoiseFilter.Evaluate(pos) - settings.biomeColorSettings.noiseOffset)*settings.biomeColorSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendAmount / 2f + 0.0001f;
        
        for (int i = 0; i < numBiomes; ++i)
        {
            float distance = heightRatio - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
            
        }

        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }
    
    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width*texture.height];
        int j = 0;
        foreach (var biome in settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < textureResolution*2; ++i)
            {
                Color grad;
                if (i < textureResolution)
                {
                    grad = settings.oceanColor.Evaluate(i / (textureResolution - 1f));
                }
                else
                {
                    grad = biome.gradient.Evaluate((i-textureResolution) / (textureResolution - 1f));
                }
                Color tint = biome.tint;
                colors[j++] = grad * (1 - biome.tintRatio) + tint * biome.tintRatio;
            }
        }
       
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}