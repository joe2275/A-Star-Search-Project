using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointGenerator : MonoBehaviour {

    private WayPointManager m_wayPointManager;
    [SerializeField]
    private ELayerFlags[] m_obstacleLayers;
    private int m_obstacleLayer;

    [SerializeField]
    private Transform m_leftDownTransform;
    private Vector2 m_leftDownPosition;
    private Vector2Int m_leftDownKey;
    [SerializeField]
    private Transform m_rightUpTransform;
    private Vector2 m_rightUpPosition;
    private Vector2Int m_rightUpKey;

    private Vector2 m_centerPosition;
    private Vector2Int m_centerKey;

    private Vector2 m_gapSize;

    private void OnDrawGizmos()
    {
        if(m_leftDownTransform != null && m_rightUpTransform != null)
        {
            Gizmos.color = new Color(0.5f, 0.0f, 0.5f, 0.1f);

            Vector2 leftDownPosition = m_leftDownTransform.position;
            Vector2 rightUpPosition = m_rightUpTransform.position;
            Gizmos.DrawCube(new Vector3((leftDownPosition.x + rightUpPosition.x) * 0.5f, (leftDownPosition.y + rightUpPosition.y) * 0.5f, 0.0f)
                , new Vector3(rightUpPosition.x - leftDownPosition.x, rightUpPosition.y - leftDownPosition.y, 0.0f));
        }
    }

    private void Awake()
    {
        for(int i=0; i< m_obstacleLayers.Length; i++)
        {
            m_obstacleLayer = m_obstacleLayer | 1 << (int)m_obstacleLayers[i];
        }

        m_wayPointManager = WayPointManager.Instance;
        m_leftDownPosition = m_leftDownTransform.position;
        m_rightUpPosition = m_rightUpTransform.position;
        m_leftDownKey = m_wayPointManager.ConvertPositionToKey(m_leftDownPosition);
        m_rightUpKey = m_wayPointManager.ConvertPositionToKey(m_rightUpPosition);
        m_centerPosition = new Vector2((m_leftDownPosition.x + m_rightUpPosition.x) * 0.5f, (m_leftDownPosition.y + m_rightUpPosition.y) * 0.5f);
        m_centerKey = m_wayPointManager.ConvertPositionToKey(m_centerPosition);
        m_gapSize = m_wayPointManager.GapSize;
        m_gapSize = new Vector2(m_gapSize.x * 0.9f, m_gapSize.y * 0.9f);
        GenerateWayPoint();
        Debug.Log("Gen");
    }

    public void GenerateWayPoint()
    {
        Queue<WayPoint> wayPointQueue = new Queue<WayPoint>();
        Vector2Int[] nextKeys = new Vector2Int[4];
        Vector2 nextPosition;
        WayPoint nextWayPoint;

        Vector2 curPosition = m_centerPosition;
        Vector2Int curKey = m_centerKey;

        WayPoint curWayPoint = new WayPoint(curKey);

        m_wayPointManager.AddWayPoint(curWayPoint);
        wayPointQueue.Enqueue(curWayPoint);
        
        // BFS Algorithm
        while (wayPointQueue.Count != 0)
        {
            curWayPoint = wayPointQueue.Dequeue();
            curKey = curWayPoint.Key;
            // Keys of Right, Up, Left, Down Side
            nextKeys[0] = new Vector2Int(curKey.x + 1, curKey.y);
            nextKeys[1] = new Vector2Int(curKey.x, curKey.y + 1);
            nextKeys[2] = new Vector2Int(curKey.x - 1, curKey.y);
            nextKeys[3] = new Vector2Int(curKey.x, curKey.y - 1);

            for(int i=0; i<4; i++)
            {
                // if nextKeys were not out of boundary
                if(((nextKeys[i].x >= m_leftDownKey.x && nextKeys[i].y >= m_leftDownKey.y) &&
                    (nextKeys[i].x <= m_rightUpKey.x && nextKeys[i].y <= m_rightUpKey.y)))
                {
                    nextPosition = m_wayPointManager.ConvertKeyToPosition(nextKeys[i]);
                    Collider2D obstacle = Physics2D.OverlapBox(nextPosition, m_gapSize, 0.0f, m_obstacleLayer);
                    if(obstacle == null)
                    {
                        nextWayPoint = new WayPoint(nextKeys[i]);
                        Vector2 position = m_wayPointManager.ConvertKeyToPosition(nextKeys[i]);
                        if (m_wayPointManager.AddWayPoint(nextWayPoint))
                        {
                            wayPointQueue.Enqueue(nextWayPoint);
                        }
                    }
                }
            }
        }
    }
}
