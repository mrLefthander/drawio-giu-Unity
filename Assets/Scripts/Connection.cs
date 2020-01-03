using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ConnectionType
{
    Bezier,
    Line,
}
public class Connection
{
    public Action<Connection> OnClickRemoveConnection;
    private ConnectionType type;
    public ConnectionPoint FirstPoint { get; }
    public ConnectionPoint SecondPoint { get; }
    private Vector2 centerPoint;

    public Connection(List<ConnectionPoint> selectedPoints, ConnectionType type, Action<Connection> OnClickRemoveConnection)
    {
        FirstPoint = selectedPoints[0];
        SecondPoint = selectedPoints[1];
        this.type = type;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {
        centerPoint = (FirstPoint.rect.center + SecondPoint.rect.center) / 2f;
        Handles.color = Color.black;

        switch (type)
        {
            case ConnectionType.Bezier:
                DrawBezier();
                break;
            case ConnectionType.Line:
                DrawLine();
                break;
        }

        DrawRemoveButton();
    }

    private void DrawBezier()
    {
        Handles.DrawBezier(
            FirstPoint.rect.center,
            SecondPoint.rect.center,
            new Vector2(centerPoint.x, FirstPoint.rect.center.y),
            new Vector2(centerPoint.x, SecondPoint.rect.center.y),
            Color.black,
            null,
            2f
        );
    }

    private void DrawLine()
    {
        Handles.DrawLine(FirstPoint.rect.center, SecondPoint.rect.center);
    }

    private void DrawRemoveButton()
    {
        if (FirstPoint.node.IsSelected || SecondPoint.node.IsSelected)
        {
            if (Handles.Button(centerPoint, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                OnClickRemoveConnection?.Invoke(this);
            }
        }
    }
}
