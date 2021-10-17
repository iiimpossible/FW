using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
    
//用一个数据结构来存储节点的状态
public class AIBrickState
{
    public bool isAccess { get; private set; }
    public bool isFound { get; private set; }
    public bool isColorVariable { get; private set; }
    public bool isObstacle { get; private set; }
    public float distance { get; private set; }//砖块到源节点的距离
    public Vector2Int pos { get; set; }
    public Color color { get; private set; }
    public GameObject self { get; private set; }
    public AIBrickState parentState { get; private set; }

    public AIBrickState(Vector2Int pos, GameObject self, AIBrickState parent = null)
    {
        this.color = Color.white;
        this.pos = pos;
        this.distance = 0;
        this.isAccess = false;
        this.isObstacle = false;
        this.parentState = null;
        this.isFound = false;
        this.self = self;
        this.isColorVariable = true;
    } 

    public AIBrickState SetAccess()
    {
        this.isAccess = true;
        this.SetColor(Color.yellow);
        return this;
    }

    public AIBrickState SetFound()
    {
        this.isFound = true;
        if (isObstacle)
            return this;
        this.SetColor(Color.grey);
        return this;
    }

    public AIBrickState SetObstacle(bool isObstacle = true)
    {
        this.isObstacle = isObstacle;         
        this.SetColor(Color.black);
        return this;
    }

    public AIBrickState SetColor(Color color)
    {
        if (!isColorVariable) return this;
        this.color = color;
        self.GetComponent<SpriteRenderer>().color = color;
        return this;
    }

    public AIBrickState SetParentState(AIBrickState parent,float distance)
    {
        if (isAccess) return this;
        this.parentState = parent;
        this.distance += distance + 1;
        return this;
    }

    public AIBrickState SetDistance(int dis)
    {
        this.distance += dis;
        return this;
    }


    public AIBrickState SetColorVariable(bool isVariable)
    {
        this.isColorVariable = isVariable;
        return this;
    }
}

 