using UnityEngine;
using System.Collections;

public class WayPointDetector : MonoBehaviour
{
    private WayPointManager m_wayPointManager;
    private WayPoint m_curWayPoint;
    public WayPoint CurrentWayPoint { get { return m_curWayPoint; } }
    [SerializeField]
    private EWayPointFlags m_wayPointFlag;

    private void Awake()
    {
        m_wayPointManager = WayPointManager.Instance;
        m_curWayPoint = null;
    }

    private void Update()
    {
        detectWayPoint();
    }

    private void detectWayPoint()
    {
        WayPoint curWayPoint = m_wayPointManager.GetWayPoint(transform.position);
        if (curWayPoint != null)
        {
            if (m_curWayPoint != null)
            {
                m_curWayPoint.RemoveTarget(m_wayPointFlag, transform);
            }
            m_curWayPoint = curWayPoint;
            m_curWayPoint.AddTarget(m_wayPointFlag, transform);
        }
        else
        {
            if (m_curWayPoint != null)
            {
                m_curWayPoint.RemoveTarget(m_wayPointFlag, transform);
            }
            m_curWayPoint = null;
        }
    }
}
