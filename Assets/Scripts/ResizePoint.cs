using UnityEngine;

public class ResizePoint
{
  public Rect Rect;

  private const float RESIZE_POINT_SIZE = 10f;
  private const string RESIZE_POINT_BOX_TEXT = "∆";

  private readonly Node _node;

  public ResizePoint(Node node)
  {
    _node = node;
    Rect = new Rect(0, 0, RESIZE_POINT_SIZE, RESIZE_POINT_SIZE);
  }

  public void Draw()
  {
    Rect.position = new Vector2(_node.Rect.x + _node.Rect.width - RESIZE_POINT_SIZE,
                    _node.Rect.y + _node.Rect.height - RESIZE_POINT_SIZE);
    GUI.Box(Rect, RESIZE_POINT_BOX_TEXT);
  }
}
