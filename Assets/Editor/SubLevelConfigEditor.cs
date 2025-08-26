using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;

[CustomEditor(typeof(MiningSublevelConfig))]
public class SubLevelConfigEditor : Editor
{
    private ReorderableList objectivesList;
    private ReorderableList gateReqList;
    private ReorderableList chestsReqList;
    private ReorderableList chestsLootList;

    private void OnEnable()
    {
        objectivesList = CreateManagedRefList<ISublevelObjective>("objectives", "Objectives");
        gateReqList = CreateManagedRefList<IRequirement>("gateRequirements", "Gate Requirements");
        chestsReqList = CreateManagedRefList<IRequirement>("chestsRequirements", "Chests Requirements");

        // NUEVO: lista de LootBase
        chestsLootList = CreateManagedRefList<LootBase>("chestsLoot", "Chests Loot");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Dibujar las propiedades normales excepto las listas
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;
        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (prop.name != "objectives" && prop.name != "gateRequirements" && prop.name != "chestsRequirements" && prop.name != "chestsLoot")
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }

        EditorGUILayout.Space();
        objectivesList.DoLayoutList();

        EditorGUILayout.Space();
        gateReqList.DoLayoutList();

        EditorGUILayout.Space();
        chestsReqList.DoLayoutList();

        EditorGUILayout.Space();
        chestsLootList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private ReorderableList CreateManagedRefList<T>(string propertyName, string label)
    {
        SerializedProperty prop = serializedObject.FindProperty(propertyName);
        var list = new ReorderableList(serializedObject, prop, true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);
        };

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = prop.GetArrayElementAtIndex(index);

            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element, true)),
                element,
                new GUIContent($"{label} {index}"),
                true
            );
        };

        list.elementHeightCallback = (int index) =>
        {
            var element = prop.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(element, true) + 6;
        };

        list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
        {
            GenericMenu menu = new GenericMenu();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in types.OrderBy(t => t.Name))
            {
                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    prop.arraySize++;
                    var newElement = prop.GetArrayElementAtIndex(prop.arraySize - 1);
                    newElement.managedReferenceValue = Activator.CreateInstance(type);
                    serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        };

        return list;
    }
}
