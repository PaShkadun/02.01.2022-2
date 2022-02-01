using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class CustomWindow : EditorWindow
{
    private Texture2D aTexture;
    public GameObject cube;
    public Color[] colors = new Color[aSize * aSize];
    private Color first;
    private Color second;

    private const int aSize = 12;
    private const int oSize = 12;
    
    [MenuItem("My super mega/hyper menu New Menu", false, -1000)]
    public static void  ShowWindow () {
        GetWindow(typeof(CustomWindow));
    }

    void Awake()
    {
        for (var i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }

        first = Color.white;
        second = Color.white;
        
        aTexture = new Texture2D(12, 12);
    }

    private void ChangeTexture()
    {
        var newText = new Texture2D(12, 12);
        for (var i = 0; i < aSize; i++)
        {
            for (var j = 0; j < aSize; j++)
            {
                newText.SetPixel(i, j, colors[i * aSize + j]);
            }
        }
        
        newText.Apply();
        cube.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = newText;
    }

    private void OneColor()
    {
        for (var i = 0; i < aSize; i++)
        {
            for (var j = 0; j < aSize; j++)
            {
                colors[i * aSize + j] = first;
            }
        }
    }

    void OnGUI()
    {
        var current = Event.current;
        
        first = EditorGUILayout.ColorField("Color 1", first);
        second = EditorGUILayout.ColorField("Color 2", second);

        for (var i = 0; i < aSize; i++)
        {
            for (var j = 0; j < aSize; j++)
            {
                var position = new Rect(oSize + (i * oSize) + (i * 2), (oSize * 5) + (j * oSize + oSize * 2) + (j * 2), oSize, oSize);

                if (current.type == EventType.MouseDown)
                {
                    if (current.button == 0 && position.Contains(current.mousePosition))
                    {
                        colors[i * aSize + j] = first;
                        current.Use();
                        Debug.Log("left");
                    }
                    
                    if (current.button == 1 && position.Contains(current.mousePosition))
                    {
                        colors[i * aSize + j] = second;
                        current.Use();
                        Debug.Log("right");
                    }
                }

                GUIStyle style = new GUIStyle();

                style.padding = new RectOffset(0,0,0,0);
                style.border = new RectOffset(0,0,0,0);
                style.margin = new RectOffset(0, 0, 0, 0);
                style.alignment = TextAnchor.MiddleCenter;
                style.imagePosition = ImagePosition.ImageOnly;
                
                GUI.color = colors[i * aSize + j];
                GUI.Box(position, aTexture, style);
            }
        }

        GUI.color = Color.white;

        var dropPos = new Rect(20, 420, 325, 30);
        
        GUI.Box(dropPos, "Drag some GameObject here");

        if (current.type == EventType.DragPerform || current.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (dropPos.Contains(current.mousePosition) && current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                cube = (GameObject) DragAndDrop.objectReferences[0];
                
                current.Use();
                
                Debug.Log("OK!");
            }
        }

        if (GUILayout.Button("save"))
        {
            if (cube)
            {
                ChangeTexture();
            }
        }
        
        var oneColorPos = new Rect(20, 460, 325, 30);
        
        GUI.Box(oneColorPos, "One color");

        if (oneColorPos.Contains(current.mousePosition) && current.type == EventType.MouseDown)
        {
            OneColor();
            current.Use();
        }
    }
}
