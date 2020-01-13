using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;

    private int resolution;

    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;
    private ShapeGenerator shapeGenerator;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution*resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        Vector2[] uv = (mesh.uv.Length == vertices.Length)?mesh.uv:new Vector2[vertices.Length];
        
        for (int y = 0; y < resolution; ++y)
        {
            for (int x = 0; x < resolution; ++x)
            {
                Vector2 ratio = new Vector2(x, y)/(resolution-1);
                Vector3 pontOnUnitCube = localUp + (ratio.x - .5f) * 2 * axisA + (ratio.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pontOnUnitCube.normalized;
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[x + y * resolution] = shapeGenerator.GetScaledElevation(unscaledElevation)*pointOnUnitSphere;
                uv[x + y * resolution].y = unscaledElevation;
                if (x < resolution - 1 && y < resolution - 1)
                {
                    triangles[triIndex++] = x + y * resolution;
                    triangles[triIndex++] = x + y * resolution + resolution + 1;
                    triangles[triIndex++] = x + y * resolution + resolution;
                    
                    triangles[triIndex++] = x + y * resolution;
                    triangles[triIndex++] = x + y * resolution + 1;
                    triangles[triIndex++] = x + y * resolution + resolution + 1;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uv;
    }

    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = mesh.uv;
        for (int y = 0; y < resolution; ++y)
        {
            for (int x = 0; x < resolution; ++x)
            {
                Vector2 ratio = new Vector2(x, y) / (resolution - 1);
                Vector3 pontOnUnitCube = localUp + (ratio.x - .5f) * 2 * axisA + (ratio.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pontOnUnitCube.normalized;
                
                uv[x + y*resolution].x = colorGenerator.BiomeRatioFromPosition(pointOnUnitSphere);
            }
        }

        mesh.uv = uv;
    }
}
