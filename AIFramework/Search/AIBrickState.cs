using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;
using TMPro;
 
//��һ�����ݽṹ���洢�ڵ��״̬
public class AIBrickState :IGetPriority,IBinaryHeapData<AIBrickState,float>
{
    public bool isAccess { get; private set; }
    public bool isFound { get; private set; }
    public bool isColorVariable { get; private set; }
    public bool isObstacle { get; private set; }
    public float distance { get; set; }//ש�鵽Դ�ڵ�ľ���
    public Vector2Int pos { get; set; } 
    public Color color { get; private set; }
    public GameObject self { get; private set; }
    public AIBrickState parentState { get; private set; }

    public float weight {get; set;}

    public int accsessFlag;

    private Vector2Int setNeighborPos = new Vector2Int();
    public AIBrickState(){}
    public AIBrickState(Vector2Int pos = default(Vector2Int), GameObject self = null, AIBrickState parent = null)
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
        this.weight = 0;
    } 

    //���͹��캯�������в�����
    public  void InitBrick(Vector2Int pos = default(Vector2Int), GameObject self = null, AIBrickState parent = null)
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
        this.weight = 0;
    }


    public AIBrickState SetAccess()
    {
        this.isAccess = true;
        this.SetColor(Color.yellow);        
        PermissionMask.Enable((int)EBitMask.ACSSESS,ref this.accsessFlag);
        return this;
    }

    public AIBrickState SetFound()
    {
        this.isFound = true;
        PermissionMask.Enable((int)EBitMask.FOUND,ref this.accsessFlag);
        if (isObstacle)
            return this;
        this.SetColor(Color.grey);
        return this;
    }

    public AIBrickState SetObstacle(bool isObstacle = true)
    {
        this.isObstacle = isObstacle;  
        int tf = (System.Convert.ToInt32(isObstacle) <<1 )& (int)EBitMask.OBSTACLE;//��boolֵתΪ������OBSTACLE &
        PermissionMask.Enable(tf,ref this.accsessFlag);//���ﵱSetObstacle ����Ϊ�٣����ܹر���flagȨ�ޣ���಻��     
        this.SetColor(Color.black);
        //this.isColorVariable =false;
        if(!isObstacle) PermissionMask.Disable((int)EBitMask.OBSTACLE,ref accsessFlag);//��Ϊ�ϱߵ�λ�������ܸı��Ѿ�����Ϊ1��λ
        return this;
    }

    public AIBrickState SetColor(Color color)
    {
        if (!isColorVariable) return this;       
        this.color = color;
        self.GetComponent<SpriteRenderer>().color = color;
        return this;
    }

    public AIBrickState SetParentState(AIBrickState parent)
    {
        if (isAccess) return this;
        this.parentState = parent;
        this.distance += parent.distance + 1;
        this.SetFound();
        return this;
    }

    public AIBrickState SetParent(AIBrickState parent)
    {
        if(isAccess) return this;
        this.parentState = parent;
        //this.SetFound();
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

    public AIBrickState SetWeiget(float weight, Color color)
    {
        this.weight = weight;
        this.color = color;
        return this;
    }

    public void Clear()
    {        
      isAccess = false;
      isFound = false;
      isColorVariable = true;
      isObstacle =false;
      distance = 0;      
      color = Color.white;    
      parentState  = null;
      accsessFlag = 0;
      this.SetColor(Color.white);
    }

    public Vector2Int GetUp()
    {
        return new Vector2Int(pos.x, pos.y + 1);
    }

    public Vector2Int GetDown()
    {
        return new Vector2Int(pos.x, pos.y - 1);
    }

    public Vector2Int GetLeft()
    {
        return new Vector2Int(pos.x - 1, pos.y);
    }

    public Vector2Int GetRight()
    {
        return new Vector2Int(pos.x + 1, pos.y);
    }

    public float GetPriority()
    {
        return this.distance;
    }


    public void SetPriority(float p)
    {
        this.distance = p;
    }

    public Vector2Int GetNeighbors(int i)
    {
        switch (i)
        {
            case 0:
                {
                    return GetUp();
                }
            case 1:
                {
                    return GetRight();
                }
            case 2:
                {
                    return GetDown();
                }
            case 3:
                {
                    return GetLeft();
                }
            default:
                {
                    return default(Vector2Int);
                }
        }
    }

    /// <summary>
    /// ��ʱ��
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Vector2Int GetNeighborsDiagnol(int i)
    {
        switch (i)
        {
            case 0://��
                {
                    setNeighborPos.Set(pos.x, pos.y + 1);
                    return setNeighborPos;
                    //return new Vector2Int(pos.x, pos.y+1);
                }
            case 1://��
                {
                    setNeighborPos.Set(pos.x + 1, pos.y);
                    return setNeighborPos;
                    //return new Vector2Int(pos.x +1,pos.y);
                }
            case 2://��
                {
                    setNeighborPos.Set(pos.x, pos.y - 1);
                    return setNeighborPos;
                    //return new Vector2Int(pos.x,pos.y -1);
                }
            case 3://��
                {
                    setNeighborPos.Set(pos.x - 1, pos.y);
                    return setNeighborPos;
                    //return new Vector2Int(pos.x -1,pos.y );
                }
            case 4://����
                {
                    setNeighborPos.Set(pos.x + 1, pos.y + 1);
                    return setNeighborPos;
                    //return new Vector2Int(pos.x +1,pos.y +1);
                }
        
            case 5://����
                {
                    setNeighborPos.Set(pos.x +1,pos.y -1); 
                    return setNeighborPos;
                    //return new Vector2Int(pos.x +1,pos.y -1);
                }
          
            case 6://����
                {
                    setNeighborPos.Set(pos.x -1,pos.y -1); 
                    return setNeighborPos;
                    //return new Vector2Int(pos.x -1,pos.y -1);
                }
         
            case 7://����
                {
                    setNeighborPos.Set(pos.x -1,pos.y +1); 
                    return setNeighborPos;
                    //return new Vector2Int(pos.x -1,pos.y +1);
                }
            default:
                return default(Vector2Int);
        }
    }


    public void SetData(float d)
    {
        this.distance = d;
    }

    public float GetData()
    {
        return this.distance;
    }

    public int Compare(AIBrickState a, AIBrickState b)
    {
        if(a == null) return -2;//a = null
        if(b == null) return -3;//b = null
        float delta = Mathf.Abs( a.distance - b.distance);
        if(delta < 0.01) return 0;
        
        if(a.distance > b.distance)
            return 1; //a > b
        return -1;//a < b
    }

    public void SetText(string str)
    {
        TextMeshPro tmp = self.transform.Find("Text")?.GetComponent<TextMeshPro>();
        if(tmp != null)tmp.text = str;
    }
}

