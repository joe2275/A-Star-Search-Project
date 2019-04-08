using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionFlags
{
    RIGHT = 0, LEFT = 1, DOWN = 2, UP = 3, COUNT
}

public class PathFinder
{
    private WayPointManager m_wayPointManager;

    private List<NodeInfo> m_openNodeList;
    private List<NodeInfo> m_closeNodeList;

    private Vector2Int m_startPoint = new Vector2Int(0, 0);
    private Vector2Int m_endPoint = new Vector2Int(10, -10);

    public PathFinder()
    {
        m_openNodeList = new List<NodeInfo>();
        m_closeNodeList = new List<NodeInfo>();
        m_wayPointManager = WayPointManager.Instance;
    }

    public Stack<Vector2Int> FindPath()
    {
        NodeInfo curNode = new NodeInfo();
        NodeInfo[] nextNodes = new NodeInfo[(int)DirectionFlags.COUNT];
        List<NodeInfo>.Enumerator enumerator;

        curNode.NodeId = m_startPoint;

        m_closeNodeList.Add(curNode);


        while (curNode.NodeId != m_endPoint)
        {
            for (DirectionFlags e = DirectionFlags.RIGHT; e < DirectionFlags.COUNT; e++)
            {
                nextNodes[(int)e] = new NodeInfo();
                nextNodes[(int)e].ParentId = curNode.NodeId;
                switch (e)
                {
                    case DirectionFlags.RIGHT:
                        nextNodes[(int)DirectionFlags.RIGHT].NodeId = new Vector2Int(curNode.NodeId.x + 1, curNode.NodeId.y);
                        break;
                    case DirectionFlags.LEFT:
                        nextNodes[(int)DirectionFlags.LEFT].NodeId = new Vector2Int(curNode.NodeId.x - 1, curNode.NodeId.y);
                        break;
                    case DirectionFlags.DOWN:
                        nextNodes[(int)DirectionFlags.DOWN].NodeId = new Vector2Int(curNode.NodeId.x, curNode.NodeId.y - 1);
                        break;
                    case DirectionFlags.UP:
                        nextNodes[(int)DirectionFlags.UP].NodeId = new Vector2Int(curNode.NodeId.x, curNode.NodeId.y + 1);
                        break;
                }

                enumerator = m_closeNodeList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.NodeId == nextNodes[(int)e].NodeId)
                    {
                        continue;
                    }
                }

                if (m_wayPointManager.GetWayPoint(nextNodes[(int)e].NodeId) != null)
                {
                    nextNodes[(int)e].ScoreG += curNode.ScoreG + 1;
                    nextNodes[(int)e].ScoreH = Vector2.Distance(curNode.NodeId, m_endPoint);

                    enumerator = m_openNodeList.GetEnumerator();
                    bool isExist = false;

                    while (enumerator.MoveNext())
                    {
                        NodeInfo openNode = enumerator.Current;
                        if (openNode.NodeId == nextNodes[(int)e].NodeId
                            && openNode.ScoreF > nextNodes[(int)e].ScoreF)
                        {
                            openNode.ScoreG = nextNodes[(int)e].ScoreG;
                            openNode.ScoreH = nextNodes[(int)e].ScoreH;
                            openNode.ParentId = nextNodes[(int)e].ParentId;
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        m_openNodeList.Add(nextNodes[(int)e]);
                    }
                }
                else
                {
                    continue;
                }
            }//for


            enumerator = m_openNodeList.GetEnumerator();
            enumerator.MoveNext();
            curNode = enumerator.Current;
            
            while (enumerator.MoveNext())
            {
                NodeInfo nextNode = enumerator.Current;
                if (curNode.ScoreF > nextNode.ScoreF)
                {
                    curNode = nextNode;
                }
            }

            m_openNodeList.Remove(curNode);
            m_closeNodeList.Add(curNode);
        }

        NodeInfo[] closeNodes = m_closeNodeList.ToArray();
        Stack<Vector2Int> pathStack = new Stack<Vector2Int>();
        pathStack.Push(closeNodes[closeNodes.Length - 1].NodeId);
        pathStack.Push(closeNodes[closeNodes.Length - 1].ParentId);

        for(int i=closeNodes.Length - 2; i >0; i--)
        {
            if(closeNodes[i].NodeId == pathStack.Peek())
            {
                pathStack.Push(closeNodes[i].ParentId);
            }
        }

        return pathStack;
    }
}
