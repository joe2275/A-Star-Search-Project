using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo
{
    public Vector2Int NodeId { get; set; }
    public float ScoreF { get; set; }
    public float ScoreG { get; set; }
    public float ScoreH { get; set; }
    public Vector2Int ParentId { get; set; }
}
