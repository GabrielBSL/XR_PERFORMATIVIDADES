using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FMODAudioManager))]
class FMODAudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FMODAudioManager test = (FMODAudioManager) target;

        EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Teste1"))
            {
                //test.DoThing("Teste1");
            }

            if(GUILayout.Button("Teste2"))
            {
                //test.DoThing("Teste2");
            }

            if(GUILayout.Button("Teste3"))
            {
                //test.DoThing("Teste3");
            }

        EditorGUILayout.EndHorizontal();
        
        base.OnInspectorGUI();
    }
}