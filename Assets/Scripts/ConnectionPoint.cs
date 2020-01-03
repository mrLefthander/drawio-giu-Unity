using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ConnectionPointPosition
{
    Top,
    Bottom,
    Left,
    Right,
}
public class ConnectionPoint
{
    public Rect rect;
    private ConnectionPointPosition positionOnNode;
    public Node node;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(Node node, ConnectionPointPosition positionOnNode, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.positionOnNode = positionOnNode;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 10f);
    }

    public void Draw()
    {
        switch (positionOnNode)
        {
            case ConnectionPointPosition.Top:
                rect.position = new Vector2(node.rect.x + node.rect.width / 2, 
                    node.rect.y - rect.height / 2);
                break;

            case ConnectionPointPosition.Bottom:
                rect.position = new Vector2(node.rect.x + node.rect.width / 2, 
                    node.rect.y + node.rect.height - rect.height / 2);
                break;

            case ConnectionPointPosition.Left:
                rect.position = new Vector2(node.rect.x - rect.height / 2, 
                    node.rect.y + node.rect.height / 2 - rect.width / 2);
                break;

            case ConnectionPointPosition.Right:
                rect.position = new Vector2(node.rect.x + node.rect.width - rect.height / 2, 
                    node.rect.y + node.rect.height / 2 - rect.width / 2);
                break;
        }

        if (GUI.Button(rect, "", "textarea"))
        {
            OnClickConnectionPoint?.Invoke(this);
        }
    }
}
