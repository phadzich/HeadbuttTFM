using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelPainter : EditorWindow
{
    public ColorPalette palette;

    private int gridSize = 25;
    private int pixelSize = 23;
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

    private void ClearGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                gridData[x, y] = 0; // Color base, usualmente blanco
            }
        }
        Repaint();
    }

    private void PaintCenterCross()
    {
        int center = gridSize / 2;
        for (int i = 0; i < gridSize; i++)
        {
            gridData[center, i] = selectedColorIndex; // Vertical
            gridData[i, center] = selectedColorIndex; // Horizontal
        }
        Repaint();
    }

    private void OnGUI()
    {
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

        float canvasWidth = gridSize * pixelSize + 40; // espacio extra para coordenadas
        float canvasHeight = gridSize * pixelSize + 20;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(600));
        Rect rect = GUILayoutUtility.GetRect(canvasWidth, canvasHeight);

        // Dibuja las celdas
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Rect cellRect = new Rect(rect.x + x * pixelSize, rect.y + y * pixelSize, pixelSize, pixelSize);
                EditorGUI.DrawRect(cellRect, palette.colors[gridData[x, y]].color);

                if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) && cellRect.Contains(Event.current.mousePosition))
                {
                    gridData[x, y] = selectedColorIndex;
                    Event.current.Use();
                    Repaint();
                }
            }
        }

        // Dibuja grilla
        Handles.BeginGUI();
        Handles.color = new Color(0, 0, 0, 0.3f);
        for (int x = 0; x <= gridSize; x++)
        {
            float xPos = rect.x + x * pixelSize;
            Handles.DrawLine(new Vector2(xPos, rect.y), new Vector2(xPos, rect.y + gridSize * pixelSize));
        }
        for (int y = 0; y <= gridSize; y++)
        {
            float yPos = rect.y + y * pixelSize;
            Handles.DrawLine(new Vector2(rect.x, yPos), new Vector2(rect.x + gridSize * pixelSize, yPos));
        }
        Handles.EndGUI();

        // Dibuja coordenadas
        GUIStyle coordStyle = new GUIStyle(EditorStyles.label);
        coordStyle.fontSize = 10;
        coordStyle.normal.textColor = Color.black;

        for (int x = 0; x < gridSize; x++)
        {
            float xPos = rect.x + x * pixelSize + pixelSize / 2f - 8;
            GUI.Label(new Rect(xPos, rect.y - 20, 30, 20), x.ToString(), coordStyle);
        }

        for (int y = 0; y < gridSize; y++)
        {
            float yPos = rect.y + y * pixelSize + pixelSize / 2f - 8;
            float xRight = rect.x + gridSize * pixelSize + 5;
            GUI.Label(new Rect(xRight, yPos, 30, 20), y.ToString(), coordStyle);
        }

        // Muestra coordenadas del mouse
        Vector2 localMouse = Event.current.mousePosition;
        int hoveredX = (int)((localMouse.x - rect.x) / pixelSize);
        int hoveredY = (int)((localMouse.y - rect.y) / pixelSize);
        if (hoveredX >= 0 && hoveredX < gridSize && hoveredY >= 0 && hoveredY < gridSize)
        {
            GUI.Label(new Rect(localMouse.x + 10, localMouse.y, 100, 20), $"({hoveredX}, {hoveredY})", coordStyle);
        }

        EditorGUILayout.EndScrollView();

        // Botones fuera del scroll
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Export PNG")) ExportToPNG();
        if (GUILayout.Button("Import PNG")) ImportFromPNG();
        if (GUILayout.Button("Clear All")) ClearGrid();
        if (GUILayout.Button("Paint Center Cross")) PaintCenterCross();
        GUILayout.EndHorizontal();
    }


    private void ExportToPNG()
    {
        Texture2D texture = new Texture2D(gridSize, gridSize);
        for (int x = 0; x < gridSize; x++)
            for (int y = 0; y < gridSize; y++)
                texture.SetPixel(x, y, palette.colors[gridData[x, gridSize - 1 - y]].color);
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
                gridData[x, gridSize - 1 - y] = FindClosestColorIndex(texture.GetPixel(x, y));

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
