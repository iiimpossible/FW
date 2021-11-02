using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI的状态机
/// 1.状态机含有许多状态（或者说行为）
/// 2.状态转换需要条件
/// 3.设置行为优先级
/// 4.状态的对象一定是一个可以被控制的游戏物体
/// 5.一个状态应该由一个回调函数和某些数据组成，回调函数控制数据，状态根据数据变化？
/// </summary>
public class AIStateMachine 
{
    
    /// <summary>
    /// 循环执行状态
    /// </summary>
    public void Execute()
    {
        while(false)
        {

        }
    }
}
