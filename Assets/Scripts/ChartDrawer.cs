using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChartDrawer: MonoBehaviour
{
    private List<Node> nodes = new List<Node>();
    private List<ConnectionPoint> selectedPoints = new List<ConnectionPoint>();
    private List<Connection> connections = new List<Connection>();
    private int selectionGridInt = 0;
    private string[] selectionStrings = { "Bezier", "Line"};

    private void OnGUI()
    {

        Event e = Event.current;
   //     Debug.Log("Current detected event: " + Event.current);
        AddNodeButton();

        GUI.Label(new Rect(15, 50, 60, 30), "Connection:", GUIStyle.none);
        selectionGridInt = GUI.SelectionGrid(new Rect(15, 70, 70, 40), selectionGridInt, selectionStrings, 1);
        DrawNodes();
        DrawConnections();
        DrawConnectionLine(e);
        ProcessNodesEvents(e);
        ProcessNodeDeletion(e);
    }

    private void ProcessNodeDeletion(Event e)
    {
       if(e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].IsSelected)
                {
                    DeleteNode(nodes[i]);
                }
            }
        }
    }

    private void DeleteNode(Node node)
    {
        if(connections.Count > 0)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++)
            {
                if (node.connectionPoints.Contains(connections[i].FirstPoint) || node.connectionPoints.Contains(connections[i].SecondPoint))
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }
        }

        nodes.Remove(node);
    }

    private void ProcessNodesEvents(Event e)
    {
       foreach(Node node in nodes)
        {
            node.ProcessEvents(e);
        }
    }

    private void AddNodeButton()
    {
        if (GUI.Button(new Rect(15, 15, 70, 20), "Add Node"))
        {
            nodes.Add(new Node(100, 100, 150, 100, OnPointClick));
        }
    }

    private void DrawNodes()
    {
        foreach (Node node in nodes)
        {
            node.Draw();
        }
    }

    private void DrawConnections()
    {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedPoints.Count == 1)
        {
            Vector2 firstPoint = selectedPoints[0].rect.center;
            Vector2 secondPoint = e.mousePosition;
            Vector2 centerPoint = (firstPoint + secondPoint) / 2f;
            DrawOnePointLine(firstPoint, secondPoint, centerPoint);
        }

    }

    private void DrawOnePointLine(Vector2 firstPoint, Vector2 secondPoint, Vector2 centerPoint)
    {
        if (selectionGridInt == 1)
        {
            Handles.DrawLine(firstPoint, secondPoint);
        }
        else
        {
            Handles.DrawBezier(
                   firstPoint,
                   secondPoint,
                   new Vector2(centerPoint.x, firstPoint.y),
                   new Vector2(centerPoint.x, secondPoint.y),
                   Color.black,
                   null,
                   2f
               );
        }
    }

    private void OnPointClick(ConnectionPoint nodePoint)
    {
        selectedPoints.Add(nodePoint);
        if(selectedPoints.Count == 2)
        {
            if (selectedPoints[0].node != selectedPoints[1].node)
            {
                CreateConnection();
                selectedPoints.Clear();
            }
            else
            {
                selectedPoints.Clear();
            }
        }
    }

    private void CreateConnection()
    {
        ConnectionType type;
        if (selectionGridInt == 1)
        {
            type = ConnectionType.Line; 
        }
        else
        {
            type = ConnectionType.Bezier;
        }

        connections.Add(new Connection(selectedPoints, type, OnClickRemoveConnection));
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    void DrawCurve(Rect node1, Rect node2)
    {
        Vector3 startPos = new Vector3(node1.x + node1.width, node1.y + node1.height / 2, 0);
        Vector3 endPos = new Vector3(node2.x, node2.y + node2.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 100;
        Vector3 endTan = endPos + Vector3.left * 100;
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 3);
    }
}
