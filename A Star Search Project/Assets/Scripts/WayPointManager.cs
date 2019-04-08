using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * WayPoint 클래스
 * 해당 WayPoint가 존재하는 곳에 어떤 Target이 존재하는지 저장하는 클래스이다.
 */
public class WayPoint
{
    private Vector2Int m_key;
    public Vector2Int Key
    {
        get { return m_key; }
    }
    private Dictionary<EWayPointFlags, List<Transform>> m_targetDict;

    public WayPoint(Vector2Int key)
    {
        m_targetDict = new Dictionary<EWayPointFlags, List<Transform>>();

        for (EWayPointFlags e = EWayPointFlags.EMPTY; e < EWayPointFlags.COUNT; e++)
        {
            m_targetDict.Add(e, new List<Transform>());
        }

        m_key = key;
    }

    public void AddTarget(EWayPointFlags wayPointFlag, Transform targetTransform)
    {
        m_targetDict[wayPointFlag].Add(targetTransform);
    }

    public void RemoveTarget(EWayPointFlags wayPointFlag, Transform targetTransform)
    {
        m_targetDict[wayPointFlag].Remove(targetTransform);
    }

    public List<Transform>.Enumerator GetTarget(EWayPointFlags wayPointFlag)
    {
        return m_targetDict[wayPointFlag].GetEnumerator();
    }
}

/*
 * EWayPointFlags 열거형
 * WayPoint에 기록할 대상이다.
 */
public enum EWayPointFlags
{
    EMPTY = 0, PLAYER, TRAP, ARRIVAL, COUNT
}

/*
 * WayPointManager 클래스
 * 한 Scene에 존재하는 모든 WayPoint를 관리하는 클래스로 Singleton Pattern으로 구현되었다.
 */
public class WayPointManager
{
    private static WayPointManager m_instance = null;

    public static WayPointManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new WayPointManager();
            }

            return m_instance;
        }
    }

    private Dictionary<Vector2Int, WayPoint> m_wayPointDict;

    private Vector2 m_gapSize = new Vector2(2.0f, 2.0f);
    public Vector2 GapSize { get { return m_gapSize; } }

    public WayPointManager()
    {
        m_wayPointDict = new Dictionary<Vector2Int, WayPoint>();
    }

    public Vector2Int ConvertPositionToKey(Vector2 position)
    {
        return new Vector2Int((int)(position.x / m_gapSize.x), (int)(position.y / m_gapSize.y));
    }

    public Vector2 ConvertKeyToPosition(Vector2Int key)
    {
        return new Vector2(key.x * m_gapSize.x + m_gapSize.x * 0.5f, key.y * m_gapSize.y + m_gapSize.y * 0.5f);
    }

    public bool AddWayPoint(WayPoint wayPoint)
    {
        Vector2Int key = wayPoint.Key;
        if (!m_wayPointDict.ContainsKey(key))
        {
            m_wayPointDict.Add(key, wayPoint);
            return true;
        }

        return false;
    }

    public bool RemoveWayPoint(Vector2Int key)
    {
        if (m_wayPointDict.ContainsKey(key))
        {
            m_wayPointDict.Remove(key);
            return true;
        }
        return false;
    }

    public WayPoint GetWayPoint(Vector2 position)
    {
        Vector2Int key = ConvertPositionToKey(position);

        if (m_wayPointDict.ContainsKey(key))
        {
            return m_wayPointDict[key];
        }
        return null;
    }

    public WayPoint GetWayPoint(Vector2Int key)
    {

        if(m_wayPointDict.ContainsKey(key))
        {
            return m_wayPointDict[key];
        }

        return null;
    }

    public void Print()
    {
        foreach (KeyValuePair<Vector2Int, WayPoint> pair in m_wayPointDict)
        {
            Debug.Log(pair.Key);
        }
    }

    /// <summary>
    /// 활성화된 씬이 변할때 호출되는 이벤트 메소드
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        m_instance = new WayPointManager();
    }

    public void Destroy()
    {
        m_instance = null;
    }
}
