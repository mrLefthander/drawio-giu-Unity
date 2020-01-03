using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizePoint
{
    public Rect rect;
    public Node node;

    public ResizePoint(Node node)
    {
        this.node = node;
        rect = new Rect(0, 0, 10f, 10f);
    }

    public void Draw()
    {
        rect.position = new Vector2(node.rect.x + node.rect.width - 10f,
                    node.rect.y + node.rect.height - 10f);
        GUI.Box(rect, "∆");
    }
}
