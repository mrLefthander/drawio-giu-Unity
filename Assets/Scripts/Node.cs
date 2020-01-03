using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node
{
    public Rect rect;
    private string title = string.Empty;
    private string textArea = string.Empty;
    private bool isDragged = false;
    private bool isEditing = false;
    private bool isHovered = false;
    private bool isResizing = false;

    public bool IsSelected { get; private set; } = false;
    GUIStyle textAreaStyle = new GUIStyle("TextArea");
    GUIStyle labelStyle = new GUIStyle("Label");


    public List<ConnectionPoint> connectionPoints = new List<ConnectionPoint>();
    public ResizePoint resizePoint;

    public Node(float x, float y, float width, float height, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        rect = new Rect(x, y, width, height);

        textAreaStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        connectionPoints.Add(new ConnectionPoint(this, ConnectionPointPosition.Top, OnClickConnectionPoint));
        connectionPoints.Add(new ConnectionPoint(this, ConnectionPointPosition.Bottom, OnClickConnectionPoint));
        connectionPoints.Add(new ConnectionPoint(this, ConnectionPointPosition.Left, OnClickConnectionPoint));
        connectionPoints.Add(new ConnectionPoint(this, ConnectionPointPosition.Right, OnClickConnectionPoint));

        resizePoint = new ResizePoint(this);
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    private void Resize(Vector2 delta)
    {
        Vector2 minSize = new Vector2(50, 50);
        if(rect.size.x + delta.x >= minSize.x && rect.size.y + delta.y >= minSize.y)
        {
            rect.size += delta;
        }
        
    }

    public void Draw()
    {
        GUI.Box(rect, title);
        EditAndShowNodeText();
        DrawConnectionPoints();

    }

    private void DrawConnectionPoints()
    {
        if (isHovered || IsSelected)
        {
            resizePoint.Draw();
            foreach (ConnectionPoint point in connectionPoints)
            {
                point.Draw();
            }
        }
    }

    private void EditAndShowNodeText()
    {
        if (!isEditing)
        {
            GUI.Label(rect, textArea, labelStyle);
        }
        else
        {
            textArea = GUI.TextArea(rect, textArea, textAreaStyle);
        }
    }

    public void ProcessEvents(Event e)
    {
        ProcessLeftMouseButtonClicks(e);
        ProcessMouseHover(e);
    }

    private void ProcessMouseHover(Event e)
    {
        foreach(ConnectionPoint point in connectionPoints)
        {
            if (point.rect.Contains(e.mousePosition) || rect.Contains(e.mousePosition))
            {
                isHovered = true;
            }
            else
            {
                isHovered = false;
            }
        }        
    }

    private void ProcessLeftMouseButtonClicks(Event e)
    {
        if (e.button == 0)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (rect.Contains(e.mousePosition) && e.clickCount == 2)
                    {
                        isEditing = true;
                    }
                    else if (rect.Contains(e.mousePosition) && !resizePoint.rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        IsSelected = true;
                    }
                    else if (resizePoint.rect.Contains(e.mousePosition))
                    {
                        isResizing = true;
                        IsSelected = true;
                    }
                    else if (!rect.Contains(e.mousePosition))
                    {
                        isEditing = false;
                        IsSelected = false;
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    isResizing = false;
                    break;

                case EventType.MouseDrag:
                    if (isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                    }
                    else if (isResizing)
                    {
                        Resize(e.delta);
                        e.Use();
                    }
                    break;
            }
        }
    }


}

