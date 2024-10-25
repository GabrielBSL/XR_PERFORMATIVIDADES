using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubicBezier))]
public class CubicStudyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CubicBezier instance = (CubicBezier) target;

        EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Save Rotation"))
            {
                instance.SaveRotation();
            }

            if(GUILayout.Button("Load Rotation"))
            {
                //instance.LoadRotation();
            }

        EditorGUILayout.EndHorizontal();
        
        base.OnInspectorGUI();
    }
}