using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelPainter : EditorWindow
{
    public ColorPalette palette;

    private int gridSize = 21;
    private int pixelSize = 30;
    private int[,] gridData;
    private int selectedColorIndex = 0;
    private Vector2 scrollPos;

    [MenuItem("Tools/Level Painter")]
    public static void ShowWindow()
    {
        GetWindow<LevelPainter>("Level Painter");
    }

    private void OnEnable()
    {
        if (palette == null)
        {
            // Intenta cargar una paleta por defecto desde Resources (opcional)
            palette = Resources.Load<ColorPalette>("DefaultColorPalette");
        }

        if (palette == null || palette.colors == null || palette.colors.Length == 0)
        {
            Debug.LogWarning("No ColorPalette assigned or found. Please assign one.");
        }

        gridData = new int[gridSize, gridSize];
    }

    private void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("Palette", EditorStyles.boldLabel);

        if (palette == null)
        {
            EditorGUILayout.HelpBox("No ColorPalette assigned! Assign in inspector.", MessageType.Warning);
            palette = (ColorPalette)EditorGUILayout.ObjectField("Palette", palette, typeof(ColorPalette), false);
            return;
        }
        else
        {
            palette = (ColorPalette)EditorGUILayout.ObjectField("Palette", palette, typeof(ColorPalette), false);
        }

        GUILayout.BeginHorizontal();
        for (int i = 0; i < palette.colors.Length; i++)
        {
            GUI.backgroundColor = palette.colors[i].color;
            if (GUILayout.Button(new GUIContent(" ", palette.colors[i].blockString), GUILayout.Width(30), GUILayout.Height(30)))
            {
                selectedColorIndex = i;
            }
        }
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;

        GUILayout.Space(10);
        GUILayout.Label("Grid", EditorStyles.boldLabel);

        var rect = GUILayoutUtility.GetRect(gridSize * pixelSize, gridSize * pixelSize);
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Rect cellRect = new Rect(rect.x + x * pixelSize, rect.y + y * pixelSize, pixelSize, pixelSize);
                EditorGUI.DrawRect(cellRect, palette.colors[gridData[x, y]].color);

                if (Event.current.type == EventType.MouseDown && cellRect.Contains(Event.current.mousePosition))
                {
                    gridData[x, y] = selectedColorIndex;
                    Repaint();
                }
            }
        }

        GUILayout.Space(gridSize * pixelSize + 10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Export PNG"))
            ExportToPNG();
        if (GUILayout.Button("Import PNG"))
            ImportFromPNG();
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    private void ExportToPNG()
    {
        Texture2D texture = new Texture2D(gridSize, gridSize);
        for (int x = 0; x < gridSize; x++)
            for (int y = 0; y < gridSize; y++)
                texture.SetPixel(x, y, palette.colors[gridData[x, y]].color);
        texture.Apply();

        string path = EditorUtility.SaveFilePanel("Save Level Texture", "", "level.png", "png");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, texture.EncodeToPNG());
            Debug.Log("Saved to: " + path);
        }
    }

    private void ImportFromPNG()
    {
        string path = EditorUtility.OpenFilePanel("Load PNG", "", "png");
        if (string.IsNullOrEmpty(path)) return;

        byte[] data = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(data);

        if (texture.width != gridSize || texture.height != gridSize)
        {
            Debug.LogError("Image must be " + gridSize + "x" + gridSize);
            return;
        }

        for (int x = 0; x < gridSize; x++)
            for (int y = 0; y < gridSize; y++)
                gridData[x, y] = FindClosestColorIndex(texture.GetPixel(x, y));

        Repaint();
    }

    private int FindClosestColorIndex(Color color)
    {
        for (int i = 0; i < palette.colors.Length; i++)
        {
            if (ColorsAreEqual(color, palette.colors[i].color))
                return i;
        }
        Debug.LogWarning($"Unrecognized color {color}, using index 0.");
        return 0;
    }

    private bool ColorsAreEqual(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }
}
