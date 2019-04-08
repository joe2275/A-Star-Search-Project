using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector2Int[] m_path;
    // Start is called before the first frame update
    void Start()
    {
        PathFinder finder = new PathFinder();
        m_path = finder.FindPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
