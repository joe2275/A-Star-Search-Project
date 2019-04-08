using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Stack<Vector2Int> m_path;

    private bool m_isStarted = false;
    private bool isArrived = true;

    [SerializeField]
    private Transform playerTransform;

    private Vector2 nextPosition;
    private Vector3 goalPosition = new Vector3(21,-19,0);

    // Update is called once per frame
    void Update()
    {
        if (!m_isStarted)
        {
            m_isStarted = true;
            PathFinder finder = new PathFinder();
            m_path = finder.FindPath();
        } else
        {
            if (playerTransform.localPosition != goalPosition)
            {
                if (isArrived)
                {
                    nextPosition = WayPointManager.Instance.ConvertKeyToPosition(m_path.Pop());
                    Debug.Log(nextPosition);
                    isArrived = false;
                }
                else
                {
                    if (playerTransform.localPosition.x == nextPosition.x && playerTransform.localPosition.y == nextPosition.y)
                    {
                        isArrived = true;
                    }
                    playerTransform.position = Vector2.MoveTowards(playerTransform.localPosition, nextPosition, 20.0f * Time.deltaTime);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            WayPointManager.Instance.Destroy();
            SceneManager.LoadScene(0);
        }
    }
}
