using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(MiningSublevelConfig))]
public class SubLevelConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 1. Dibujar todas las propiedades normales EXCEPTO 'objectives'
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;
        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (prop.name != "objectives") // evitamos dibujar objectives dos veces
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Objectives", EditorStyles.boldLabel);

        SerializedProperty objectivesProp = serializedObject.FindProperty("objectives");

        // 2. Mostrar lista de objetivos
        for (int i = 0; i < objectivesProp.arraySize; i++)
        {
            SerializedProperty element = objectivesProp.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(element, new GUIContent($"Objective {i}"), true);

            if (GUILayout.Button("Remove"))
            {
                objectivesProp.DeleteArrayElementAtIndex(i);
            }
        }

        EditorGUILayout.Space();

        // 3. Botón para añadir objetivo nuevo
        if (GUILayout.Button("Add Objective"))
        {
            GenericMenu menu = new GenericMenu();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(ISublevelObjective).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in types)
            {
                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    objectivesProp.arraySize++;
                    var newElement = objectivesProp.GetArrayElementAtIndex(objectivesProp.arraySize - 1);
                    newElement.managedReferenceValue = Activator.CreateInstance(type);
                    serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }

        serializedObject.ApplyModifiedProperties();
    }
}