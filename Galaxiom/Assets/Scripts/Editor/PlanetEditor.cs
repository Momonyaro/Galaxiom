using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CosmicBody))]
public class PlanetEditor : Editor
{
    CosmicBody planet;
    Editor shapeEditor;
    Editor colorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdates, ref planet.colorSettingsFoldout, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings == null) return;
        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

        using (var check = new EditorGUI.ChangeCheckScope())
        {
            if (foldout)
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();

                if (check.changed)
                {
                    if (onSettingsUpdated != null)
                        onSettingsUpdated();
                }
            }
        }
    }

    void OnEnable()
    {
        planet = (CosmicBody)target;
    }
}
