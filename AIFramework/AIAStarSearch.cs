using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    启发函数
        f(n) = g(n) + h(n)
        f 是综合优先级
        g 是节点n距离起点的代价
        h 是节点n距离终点的预计代价
    A*算法在运算过程中，每次选择f(n)最小的节点作为下一个待遍历的节点
    A*算法使用两个集合来表示待遍历的节点与已经遍历过的节点，称之为open_set close_set

    伪代码
    * 初始化open_set和close_set；
    * 将起点加入open_set中，并设置优先级为0（优先级最高）；
    * 如果open_set不为空，则从open_set中选取优先级最高的节点n：
        * 如果节点n为终点，则：
            * 从终点开始逐步追踪parent节点，一直达到起点；
            * 返回找到的结果路径，算法结束；
        * 如果节点n不是终点，则：
            * 将节点n从open_set中删除，并加入close_set中；
            * 遍历节点n所有的邻近节点：
                * 如果邻近节点m在close_set中，则：
                    * 跳过，选取下一个邻近节点
                * 如果邻近节点m也不在open_set中，则：
                    * 设置节点m的parent为节点n
                    * 计算节点m的优先级
                    * 将节点m加入open_set中

*/
public class AIAStarSearch : AISearchBase
{
    public AIAStarSearch(Vector2Int mapSize):base(mapSize)
    {

    }

    public override IEnumerator Search()
    {
        return base.Search();
    }
}
