using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private List<NodeInfo> m_openNodeList;
    private List<NodeInfo> m_closeNodeList;

    [SerializeField]
    private Vector2Int m_startPoint;
    [SerializeField]
    private Vector2Int m_endPoint;

    public Vector2Int[] FindPath()
    {
        return null;
    }
}
