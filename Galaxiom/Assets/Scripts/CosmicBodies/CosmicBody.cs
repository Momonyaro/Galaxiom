using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicBody : MonoBehaviour
{
    [Range(2, 255)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colorSettingsFoldout;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private void Init()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");    
                meshObj.transform.parent = transform;   

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();    
                meshFilters[i].sharedMesh = new Mesh(); 
            }
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);    
        }
    }

    public void GeneratePlanet()
    {
        Init();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (!autoUpdate) return;
        Init();
        GenerateMesh();
    }
    
    public void OnColorSettingsUpdates()
    {
        if (!autoUpdate) return;
        Init();
        GenerateColors();
    }

    private void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    private void GenerateColors()
    {
        foreach (MeshFilter m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.color;
        }
    }
}
