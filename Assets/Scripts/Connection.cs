using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Connection
{
    public Action<Connection> OnClickRemoveConnection;

    public ConnectionPoint FirstPoint { get; }
    public ConnectionPoint SecondPoint { get; }

    public Connection(List<ConnectionPoint> selectedPoints, Action<Connection> OnClickRemoveConnection)
    {
        FirstPoint = selectedPoints[0];
        SecondPoint = selectedPoints[1];

        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {
        Vector2 centerPoint = (FirstPoint.rect.center + SecondPoint.rect.center) / 2f;

        Handles.DrawBezier(
            FirstPoint.rect.center,
            SecondPoint.rect.center,
            new Vector2(centerPoint.x, FirstPoint.rect.center.y),
            new Vector2(centerPoint.x, SecondPoint.rect.center.y),
            Color.black,
            null,
            2f
        );

        if (Handles.Button(centerPoint, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            OnClickRemoveConnection?.Invoke(this);
        }
    }
}
