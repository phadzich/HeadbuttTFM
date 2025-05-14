using UnityEditor;
using UnityEngine;

public class ResourceDataAutoLinker : EditorWindow
{
    [MenuItem("Tools/Auto-Link Resource Prefabs")]
    static void AutoLinkPrefabs()
    {
        string prefabFolder = "Assets/Prefabs/ResourceDrops"; // 🔁 adjust as needed
        string[] prefabGuids = AssetDatabase.FindAssets("t:GameObject", new[] { prefabFolder });
        string[] dataGuids = AssetDatabase.FindAssets("t:ResourceData");

        int linked = 0;

        foreach (string dataGuid in dataGuids)
        {
            string dataPath = AssetDatabase.GUIDToAssetPath(dataGuid);
            ResourceData data = AssetDatabase.LoadAssetAtPath<ResourceData>(dataPath);
            if (data == null) continue;

            string resourceName = data.name.Replace("ResourceData", "").ToLower();

            foreach (string prefabGuid in prefabGuids)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null) continue;

                string prefabName = prefab.name.ToLower();

                if (prefabName.Contains(resourceName))
                {
                    Undo.RecordObject(data, "Auto-Link Prefab");
                    data.resourceDropPrefab = prefab;
                    EditorUtility.SetDirty(data);
                    linked++;
                    break;
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Auto-linked {linked} resource prefabs to ResourceData assets.");
    }
}
