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
  public ConnectionPoint FirstPoint { get; }
  public ConnectionPoint SecondPoint { get; }

  private const int BUTTON_SIZE = 4;
  private const int BUTTON_PICK_SIZE = 8;
  private const float BEZIER_WIDTH = 2f;

  private Vector2 _centerPoint;
  private readonly Color _connectionColor = Color.black;
  private readonly Action<Connection> _onClickRemoveConnection;
  private readonly ConnectionType _type;

  public Connection(List<ConnectionPoint> selectedPoints, ConnectionType type, Action<Connection> onClickRemoveConnection)
  {
    FirstPoint = selectedPoints[0];
    SecondPoint = selectedPoints[1];
    _type = type;
    _onClickRemoveConnection = onClickRemoveConnection;
  }

  public void Draw()
  {
    _centerPoint = (FirstPoint.Rect.center + SecondPoint.Rect.center) / 2f;
    Handles.color = _connectionColor;

    switch (_type)
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
        FirstPoint.Rect.center,
        SecondPoint.Rect.center,
        new Vector2(_centerPoint.x, FirstPoint.Rect.center.y),
        new Vector2(_centerPoint.x, SecondPoint.Rect.center.y),
        _connectionColor,
        null,
        BEZIER_WIDTH
    );
  }

  private void DrawLine()
  {
    Handles.DrawLine(FirstPoint.Rect.center, SecondPoint.Rect.center);
  }

  private void DrawRemoveButton()
  {
    if (!FirstPoint.Node.IsSelected() && !SecondPoint.Node.IsSelected())
    {
      return;
    }
    if (Handles.Button(_centerPoint, Quaternion.identity, BUTTON_SIZE, BUTTON_PICK_SIZE, Handles.RectangleHandleCap))
    {
      _onClickRemoveConnection?.Invoke(this);
    }
  }
}
