using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChartDrawer : MonoBehaviour
{
    List<Rect> nodes = new List<Rect>();
    GUIStyle style;


    Rect node1 = new Rect(10, 50, 150, 10);
    Rect node2 = new Rect(310, 310, 150, 200);
    private void OnGUI()
    {
        
        Event e = Event.current;
        Debug.Log("Current detected event: " + Event.current);
        GUILayout.BeginArea(new Rect(5, 5, 100, 30));
        if (GUILayout.Button("Add Node"))
        {
            Rect newNode = new Rect(10, 50, 150, 100);
            nodes.Add(newNode);
        }
        GUILayout.EndArea();
        for(int i = 0; i < nodes.Count; i++)
        {
            GUI.Box(nodes[i], "");
        }
       
        DrawCurve(node1, node2);
    }

    void DrawNode(int id)
    {
        GUI.DragWindow();
    }

    void DrawCurve(Rect node1, Rect node2)
    {
        Vector3 startPos = new Vector3(node1.x + node1.width, node1.y + node1.height / 2, 0);
        Debug.Log(startPos);
        Vector3 endPos = new Vector3(node2.x, node2.y + node2.height / 2, 0);
        Debug.Log(endPos);
        Vector3 startTan = startPos + Vector3.right * 100;
        Vector3 endTan = endPos + Vector3.left * 100;
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 3); 
    }
}
