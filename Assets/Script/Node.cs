using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    /// <summary>
    /// 是否可以通过此路径
    /// </summary>
    public bool _canWalk;
    /// <summary>
    /// 保存节点位置
    /// </summary>
    public Vector3 _worldPos;
    /// <summary>
    /// 整个网格的索引
    /// </summary>
    public int _gridX, _gridY;

    public int gCost;
    public int hCost;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node parent;

    public Node(bool _canWalk, Vector3 _worldPos, int _gridX, int _gridY)
    {
        this._canWalk = _canWalk;
        this._worldPos = _worldPos;
        this._gridX = _gridX;
        this._gridY = _gridY;
    }
}