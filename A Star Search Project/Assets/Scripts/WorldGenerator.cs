using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform m_leftDownTransform;
    private Vector2 m_leftDownPosition;
    [SerializeField]
    private Transform m_rightUpTransform;
    private Vector2 m_rightUpPosition;

    [SerializeField]
    private Vector2 m_gapSize;

    [SerializeField]
    private GameObject m_trapPrefab;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_possibility;

    private int m_horizontalMin;
    private int m_horizontalMax;
    private int m_verticalMin;
    private int m_verticalMax;

    private Transform m_trapHolder;

    private void OnDrawGizmos()
    {
        if(m_leftDownTransform != null && m_rightUpTransform != null)
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.1f);
            m_leftDownPosition = m_leftDownTransform.position;
            m_rightUpPosition = m_rightUpTransform.position;
            Gizmos.DrawCube(new Vector3((m_leftDownPosition.x + m_rightUpPosition.x) * 0.5f, (m_leftDownPosition.y + m_rightUpPosition.y) * 0.5f, 0.0f), new Vector3(m_rightUpPosition.x - m_leftDownPosition.x, m_rightUpPosition.y - m_leftDownPosition.y, 0.0f));
        }
    }

    private void Awake()
    {
        m_trapHolder = new GameObject("Trap_Holder").transform;

        float gap_x = m_gapSize.x * 0.5f;
        float gap_y = m_gapSize.y * 0.5f;
        m_leftDownPosition = m_leftDownTransform.position;
        m_rightUpPosition = m_rightUpTransform.position;

        m_horizontalMin = (int)((m_leftDownPosition.x + gap_x) / m_gapSize.x);
        m_horizontalMax = (int)((m_rightUpPosition.x - gap_x) / m_gapSize.x);
        m_verticalMin = (int)((m_leftDownPosition.y) / m_gapSize.y);
        m_verticalMax = (int)((m_rightUpPosition.y - gap_y) / m_gapSize.y) - 1;
        for (int i = m_horizontalMin +1; i <= m_horizontalMax -1; i++)
        {
            for (int j = m_verticalMin; j <= m_verticalMax; j++)
            {
                if (!(i == m_horizontalMin && j == m_verticalMax) && !(i == m_horizontalMax && j == m_verticalMin))
                {
                    float possibilty = Random.Range(0.0f, 1.0f);
                    if (possibilty < m_possibility)
                    {
                        Instantiate(m_trapPrefab, new Vector3((float)i * m_gapSize.x + gap_x, (float)j * m_gapSize.y + gap_y, 0.0f), Quaternion.identity, m_trapHolder);
                    }
                }
            }
        }

    }
}
