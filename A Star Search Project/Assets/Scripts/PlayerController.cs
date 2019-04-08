using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Stack<Vector2Int> m_path;

    private bool m_isStarted = false;

    // Update is called once per frame
    void Update()
    {
        if(!m_isStarted)
        {
            m_isStarted = true;
            PathFinder finder = new PathFinder();
            m_path = finder.FindPath();
            while(m_path.Count != 0)
            {
                Debug.Log(m_path.Pop());
            }
        }
    }
}
