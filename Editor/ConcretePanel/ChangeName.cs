using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //分层遍历某个游戏物体的所有子物体


    public static void GetAllChildObject(GameObject farther)
    {
        //队列以分层遍历
        Queue<Transform> trans_queue = new Queue<Transform>();
        trans_queue.Enqueue(farther.transform);
        int cur_QuerueCount = 0;
        while (true)
        {
            cur_QuerueCount = trans_queue.Count;
            Transform cur_trans = trans_queue.Dequeue();

            while ((cur_QuerueCount--) == 0)
            {
                for (int i = 0; i < cur_trans.childCount; i++)
                {
                    trans_queue.Enqueue(cur_trans.GetComponentsInChildren<Transform>()[i]);
                }
                Debug.Log(cur_trans.name);
            }
        }
    }


    //模型数据结构
    struct MyStruct
    {
        //记录模型的世界坐标以匹配
        Vector3 m_ModlePos;

        
    }



    public static void ModefyModelInfo()
    {
        //1.生成原模型信息
        //TODO：将原模型的子物体以数特定数据结构的形式储存起来

        //2.生成新模型信息
        
        //3.分别从两者中选择对象以世界坐标的差距为依据判断是否是对应的模型

        //4.将对应的旧模型的数据给到新模型

        //5.完成
    }

}
