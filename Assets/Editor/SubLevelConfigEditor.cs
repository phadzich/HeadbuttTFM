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

        // 1. Dibujar todas las propiedades normales EXCEPTO 'objectives' y 'gateRequirements'
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;
        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (prop.name != "objectives" && prop.name != "gateRequirements")
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }

        // ---------- OBJECTIVES ----------
        DrawManagedReferenceList<ISublevelObjective>("objectives", "Objectives");

        // ---------- GATE REQUIREMENTS ----------
        DrawManagedReferenceList<IGateRequirement>("gateRequirements", "Gate Requirements");

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Dibuja una lista de managed references de un tipo genérico (ej: ISublevelObjective, IGateRequirement).
    /// </summary>
    private void DrawManagedReferenceList<T>(string propertyName, string label)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        SerializedProperty listProp = serializedObject.FindProperty(propertyName);
        if (listProp == null) return;

        int removeIndex = -1;

        for (int i = 0; i < listProp.arraySize; i++)
        {
            SerializedProperty element = listProp.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box");

            // Mostrar el tipo concreto de la referencia
            Type objType = element.managedReferenceValue?.GetType();
            if (objType != null)
            {
                EditorGUILayout.LabelField($"Type: {objType.Name}", EditorStyles.miniLabel);
            }

            EditorGUILayout.PropertyField(element, new GUIContent($"{label} {i}"), true);

            if (GUILayout.Button("Remove"))
                removeIndex = i;

            EditorGUILayout.EndVertical();
        }

        if (removeIndex >= 0)
        {
            listProp.DeleteArrayElementAtIndex(removeIndex);
        }

        EditorGUILayout.Space();

        // Botón para añadir nuevo elemento
        if (GUILayout.Button($"Add {label}"))
        {
            GenericMenu menu = new GenericMenu();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in types.OrderBy(t => t.Name))
            {
                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    listProp.arraySize++;
                    var newElement = listProp.GetArrayElementAtIndex(listProp.arraySize - 1);
                    newElement.managedReferenceValue = Activator.CreateInstance(type);
                    serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }
    }
}