using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(FMODAudioManager))]
class FMODAudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FMODAudioManager instance = (FMODAudioManager) target;

        EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Reset"))
            {
                instance.Reset();
            }

            if(GUILayout.Button("Reload Scene"))
            {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }

        EditorGUILayout.EndHorizontal();
        
        base.OnInspectorGUI();
    }
}