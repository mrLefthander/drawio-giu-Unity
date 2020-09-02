using System.Collections.Generic;
using UnityEngine;
using System;

public class Node
{

  public Rect Rect;
  public List<ConnectionPoint> ConnectionPoints = new List<ConnectionPoint>();
  public ResizePoint ResizePoint;

  private const int DOUBLE_CLICK = 2;
  private State state;
  private string _title = string.Empty;
  private string _textArea = string.Empty;
  private readonly GUIStyle _textAreaStyle = new GUIStyle("TextArea");
  private readonly GUIStyle _labelStyle = new GUIStyle("Label");
  private readonly Vector2 _nodeMinSize = new Vector2(50, 50);
  private enum State
  {
    Idle,
    Selected,
    Editing,
    Hovered,
    Resizing
  }

  public Node(Rect nodeRect, Action<ConnectionPoint> onClickConnectionPoint)
  {
    Rect = nodeRect;

    _textAreaStyle.alignment = TextAnchor.MiddleCenter;
    _labelStyle.alignment = TextAnchor.MiddleCenter;

    ConnectionPoints.Add(new ConnectionPoint(this, ConnectionPointPosition.Top, onClickConnectionPoint));
    ConnectionPoints.Add(new ConnectionPoint(this, ConnectionPointPosition.Bottom, onClickConnectionPoint));
    ConnectionPoints.Add(new ConnectionPoint(this, ConnectionPointPosition.Left, onClickConnectionPoint));
    ConnectionPoints.Add(new ConnectionPoint(this, ConnectionPointPosition.Right, onClickConnectionPoint));

    ResizePoint = new ResizePoint(this);
    state = State.Idle;
  }

  private void Drag(Vector2 delta)
  {
    Rect.position += delta;
  }

  private void Resize(Vector2 delta)
  {
    if (Rect.size.x + delta.x < _nodeMinSize.x || Rect.size.y + delta.y < _nodeMinSize.y)
    {
      return;
    }

    Rect.size += delta;
  }

  public void Draw()
  {
    GUI.Box(Rect, _title);
    EditAndShowNodeText();
    DrawConnectionPoints();
  }

  private void DrawConnectionPoints()
  {
    if (!IsSelected())
    {
      return;
    }  

    foreach (ConnectionPoint point in ConnectionPoints)
    {
      point.Draw();
    }

    ResizePoint.Draw();
  }

  private void EditAndShowNodeText()
  {
    if (state != State.Editing)
    {
      GUI.Label(Rect, _textArea, _labelStyle);
    }
    else
    {
      _textArea = GUI.TextArea(Rect, _textArea, _textAreaStyle);
    }
  }

  public void ProcessEvents(Event e)
  {
    ProcessLeftMouseButtonClicks(e);
    ProcessMouseHover(e);
  }

  private void ProcessMouseHover(Event e)
  {
    foreach (ConnectionPoint point in ConnectionPoints)
    {
      bool mouseHoverOnNode = point.Rect.Contains(e.mousePosition) || Rect.Contains(e.mousePosition);
      if (mouseHoverOnNode && state == State.Idle)
      {
        state = State.Hovered;
      }
      else if (!mouseHoverOnNode && state == State.Hovered)
      {
        state = State.Idle;
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
          ProcessMouseDown(e);
          break;

        case EventType.MouseDrag:
          ProcessMouseDrag(e);
          break;
      }
    }
  }

  private void ProcessMouseDrag(Event e)
  {
    if (state == State.Selected)
    {
      Drag(e.delta);
      e.Use();
    }
    else if (state == State.Resizing)
    {
      Resize(e.delta);
      e.Use();
    }
  }

  private void ProcessMouseDown(Event e)
  {
    if (!Rect.Contains(e.mousePosition))
    {
      state = State.Idle;
    }
    else if (ResizePoint.Rect.Contains(e.mousePosition))
    {
      state = State.Resizing;
    }
    else if (Rect.Contains(e.mousePosition))
    {
      if (e.clickCount == DOUBLE_CLICK)
      {
        state = State.Editing;
      }
      else if (!ResizePoint.Rect.Contains(e.mousePosition))
      {
        state = State.Selected;
      }
    }
  }

  public bool IsSelected()
  {
    return state != State.Idle;
  }

}

