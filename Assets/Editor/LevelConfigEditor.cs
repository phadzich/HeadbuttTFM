// Editor para LevelConfig
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Dibuja el inspector normal primero (por si quieres editar cosas del LevelConfig también)
        DrawDefaultInspector();

        LevelConfig level = (LevelConfig)target;

        if (level.subLevels != null && level.subLevels.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Subniveles", EditorStyles.boldLabel);

            foreach (var sublevel in level.subLevels)
            {
                if (sublevel != null)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(sublevel.name, EditorStyles.miniBoldLabel);

                    // Crea un editor para el SublevelConfig y lo dibuja embebido
                    Editor editor = CreateEditor(sublevel);
                    editor.OnInspectorGUI();
                }
                else
                {
                    EditorGUILayout.HelpBox("Subnivel vacío", MessageType.Warning);
                }
            }
        }
    }
}
