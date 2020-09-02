using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChartDrawer : MonoBehaviour
{
  private const int LINE_SELECTION = 1;
  private List<Node> _nodes = new List<Node>();
  private List<ConnectionPoint> _selectedPoints = new List<ConnectionPoint>();
  private List<Connection> _connections = new List<Connection>();
  private int _selectionGridInt = 0;
  private readonly string[] _selectionStrings = { "Bezier", "Line" };
  private readonly Rect _labelRect = new Rect(15, 50, 60, 30);
  private readonly Rect _selectionGridRect = new Rect(15, 70, 70, 40);
  private readonly int _numOfColumns = 1;
  private readonly Rect _nodeInitialRect = new Rect(100, 100, 150, 100);
  private readonly Rect _addNodeButtonRect = new Rect(15, 15, 70, 20);
  private readonly Color _connectionColor = Color.black;

  private void OnGUI()
  {
    Event e = Event.current;
    AddNodeButton();

    GUI.Label(_labelRect, "Connection:", GUIStyle.none);
    _selectionGridInt = GUI.SelectionGrid(_selectionGridRect, _selectionGridInt, _selectionStrings, _numOfColumns);

    DrawNodes();
    DrawConnections();
    DrawConnectionLine(e);
    ProcessNodesEvents(e);
    ProcessNodeDeletion(e);
  }

  private void ProcessNodeDeletion(Event e)
  {
    if (e.type != EventType.KeyDown || e.keyCode != KeyCode.Delete)
    {
      return;
    }

    for (int i = 0; i < _nodes.Count; i++)
    {
      if (_nodes[i].IsSelected())
      {
        DeleteNode(_nodes[i]);
      }
    }
  }

  private void DeleteNode(Node node)
  {
    if (_connections.Count <= 0)
    {
    }
    else
    {
      RemoveNodeConnections(node);
    }

    _nodes.Remove(node);
  }

  private void RemoveNodeConnections(Node node)
  {
    List<Connection> connectionsToRemove = new List<Connection>();

    GetNodeConnectionsList(node, connectionsToRemove);
    RemoveNodeConnections(connectionsToRemove);
  }

  private void GetNodeConnectionsList(Node node, List<Connection> connectionsList)
  {
    for (int i = 0; i < _connections.Count; i++)
    {
      if (node.ConnectionPoints.Contains(_connections[i].FirstPoint) ||
        node.ConnectionPoints.Contains(_connections[i].SecondPoint))
      {
        connectionsList.Add(_connections[i]);
      }
    }
  }

  private void RemoveNodeConnections(List<Connection> connectionsToRemove)
  {
    for (int i = 0; i < connectionsToRemove.Count; i++)
    {
      _connections.Remove(connectionsToRemove[i]);
    }
  }

  private void ProcessNodesEvents(Event e)
  {
    foreach (Node node in _nodes)
    {
      node.ProcessEvents(e);
    }
  }

  private void AddNodeButton()
  {
    if (GUI.Button(_addNodeButtonRect, "Add Node"))
    {
      _nodes.Add(new Node(_nodeInitialRect, OnPointClick));
    }
  }

  private void DrawNodes()
  {
    foreach (Node node in _nodes)
    {
      node.Draw();
    }
  }

  private void DrawConnections()
  {
    for (int i = 0; i < _connections.Count; i++)
    {
      _connections[i].Draw();
    }
  }

  private void DrawConnectionLine(Event e)
  {
    if (_selectedPoints.Count != 1)
    {
      return;
    }

    Vector2 firstPoint = _selectedPoints[0].Rect.center;
    Vector2 secondPoint = e.mousePosition;
    Vector2 centerPoint = (firstPoint + secondPoint) / 2f;
    DrawOnePointLine(firstPoint, secondPoint, centerPoint);
  }

  private void DrawOnePointLine(Vector2 firstPoint, Vector2 secondPoint, Vector2 centerPoint)
  {
    if (_selectionGridInt == LINE_SELECTION)
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
             _connectionColor,
             null,
             2f
         );
    }
  }

  private void OnPointClick(ConnectionPoint nodePoint)
  {
    _selectedPoints.Add(nodePoint);
    if (_selectedPoints.Count == 2)
    {
      if (_selectedPoints[0].Node == _selectedPoints[1].Node)
      {
        _selectedPoints.Clear();
      }
      else
      {
        CreateConnection();
        _selectedPoints.Clear();
      }
    }
  }

  private void CreateConnection()
  {
    ConnectionType type;
    type = _selectionGridInt == 1 ? ConnectionType.Line : ConnectionType.Bezier;
    _connections.Add(new Connection(_selectedPoints, type, OnClickRemoveConnection));
  }

  private void OnClickRemoveConnection(Connection connection)
  {
    _connections.Remove(connection);
  }
}
