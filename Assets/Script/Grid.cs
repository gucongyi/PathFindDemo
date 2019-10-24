using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    Node[,] grid;
    /// <summary>
    /// 保存网格大小
    /// </summary>
    public Vector2 gridSize;
    /// <summary>
    /// 节点半径
    /// </summary>
    public float nodeRadius;
    /// <summary>
    /// 节点直径
    /// </summary>
    float nodeDiameter;
    /// <summary>
    /// 射线图层
    /// </summary>
    public LayerMask WhatLayer;

    public Transform player;

    /// <summary>
    /// 每个方向网格数的个数
    /// </summary>
    public int gridCntX, gridCntY;

    /// <summary>
    /// 保存路径列表
    /// </summary>
    public List<Node> path = new List<Node>();
    // Use this for initialization
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridCntX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridCntY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        grid = new Node[gridCntX, gridCntY];
        CreateGrid();
    }

    private void CreateGrid()
    {
        Vector3 startPoint = transform.position - gridSize.x * 0.5f * Vector3.right
            - gridSize.y * 0.5f * Vector3.forward;
        for (int i = 0; i < gridCntX; i++)
        {
            for (int j = 0; j < gridCntY; j++)
            {
                Vector3 worldPoint = startPoint + Vector3.right * (i * nodeDiameter + nodeRadius)
                    + Vector3.forward * (j * nodeDiameter + nodeRadius);
                //此节点是否可走
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, WhatLayer);
                //i，j是二维数组的索引
                grid[i, j] = new Node(walkable, worldPoint, i, j);
            }
        }
    }

    public Node GetFromPos(Vector3 pos)
    {
        float percentX = (pos.x + gridSize.x * 0.5f) / gridSize.x;
        float percentY = (pos.z + gridSize.y * 0.5f) / gridSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridCntX - 1) * percentX);
        int y = Mathf.RoundToInt((gridCntY - 1) * percentY);

        return grid[x, y];
    }
    void OnDrawGizmos()
    {
        //画出网格边缘
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));
        //画不可走网格
        if (grid == null)
            return;
        Node playerNode = GetFromPos(player.position);
        foreach (var item in grid)
        {
            Gizmos.color = item._canWalk ? Color.white : Color.red;
            Gizmos.DrawCube(item._worldPos, Vector3.one * (nodeDiameter - 0.1f));
        }
        //画路径
        if (path != null)
        {
            foreach (var item in path)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(item._worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
        //画玩家
        if (playerNode != null && playerNode._canWalk)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(playerNode._worldPos, Vector3.one * (nodeDiameter - 0.1f));
        }
    }

    public List<Node> GetNeibourhood(Node node)
    {
        List<Node> neibourhood = new List<Node>();
        //相邻上下左右格子
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int tempX = node._gridX + i;
                int tempY = node._gridY + j;

                if (tempX < gridCntX && tempX > 0 && tempY > 0 && tempY < gridCntY)
                {
                    neibourhood.Add(grid[tempX, tempY]);
                }
            }
        }
        return neibourhood;
    }
}
