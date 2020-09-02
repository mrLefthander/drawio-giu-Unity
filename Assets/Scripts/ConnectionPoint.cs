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
  public Rect Rect;
  public Node Node;

  private const float RESIZE_POINT_SIZE = 10f;

  private readonly Action<ConnectionPoint> _onClickConnectionPoint;
  private ConnectionPointPosition _positionOnNode;
  
  public ConnectionPoint(Node node, ConnectionPointPosition positionOnNode, Action<ConnectionPoint> onClickConnectionPoint)
  {
    Node = node;
    _positionOnNode = positionOnNode;
    _onClickConnectionPoint = onClickConnectionPoint;
    Rect = new Rect(0, 0, RESIZE_POINT_SIZE, RESIZE_POINT_SIZE);
  }

  public void Draw()
  {
    switch (_positionOnNode)
    {
      case ConnectionPointPosition.Top:
        TopPosition();
        break;

      case ConnectionPointPosition.Bottom:
        BottomPosition();
        break;

      case ConnectionPointPosition.Left:
        LeftPosition();
        break;

      case ConnectionPointPosition.Right:
        RightPosition();
        break;
    }

    if (GUI.Button(Rect, "", "textarea"))
    {
      _onClickConnectionPoint?.Invoke(this);
    }
  }

  private void RightPosition()
  {
    Rect.position = new Vector2(Node.Rect.x + Node.Rect.width - Rect.height / 2,
                Node.Rect.y + Node.Rect.height / 2 - Rect.width / 2);
  }

  private void LeftPosition()
  {
    Rect.position = new Vector2(Node.Rect.x - Rect.height / 2,
        Node.Rect.y + Node.Rect.height / 2 - Rect.width / 2);
  }

  private void BottomPosition()
  {
    Rect.position = new Vector2(Node.Rect.x + Node.Rect.width / 2,
                Node.Rect.y + Node.Rect.height - Rect.height / 2);
  }

  private void TopPosition()
  {
    Rect.position = new Vector2(Node.Rect.x + Node.Rect.width / 2,
                Node.Rect.y - Rect.height / 2);
  }
}
