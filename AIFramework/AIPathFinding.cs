using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��2Dƽ���ϵ�Ѱ·�㷨��A*,Dj,BFS,DFS��
/// </summary>
public class AIPathFiding
{
    /// <summary>
    /// �����������·��
    /// </summary>
    /// <param name="a">���嵱ǰ��λ��</param>
    /// <param name="b">Ŀ��λ��</param>
    /// <param name="step">����������Ӱ�����·����λ����Ŀ����Ӱ������</param>
    /// <param name="stopRange">������Χ��ÿ���������ķ�Χ���ҵ����ʵ�λ�ã�������Χ�����ҵ���������</param>
    /// <returns></returns>
    public static List<Vector3> BFS(Vector3 a,Vector3 b, float step, float stopRange,bool drawLine)
    {
        //����Ŀ��λ���Ƿ���ʣ���Ҫһ��������ѯ

        //��ȡ��a �� b ������

        /*
         ˼·��ֱ���Բ���Ϊ�뾶����һ�����꣬
              ���������Ƿ�Ϸ�
                �ǣ����ظ�����
                ���Զ��ַ�����һ���Ϸ����겢����         
         */

        float distance = Vector3.Distance(a, b);
        Vector3 pos = Vector3.Normalize( a - b) * stopRange;//��ȡ�����

        //��⵱ǰ������Ƿ����
        //TODO��
        //while(CheckPosValid(pos))
        //{

        //}




        return new List<Vector3>();
    }

    public static void DFS(Vector3 targetPos)
    {

    }

    public static void AStar(Vector3 targetPos)
    {

    }

    public static void Dj(Vector3 targetPos)
    {

    }


}
