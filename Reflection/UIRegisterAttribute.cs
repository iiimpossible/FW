using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphyFW
{
    ///UI注册器，标记UI容器类（UIBasePanel），让其能够在组件上弹出菜单选择添加
    public class UIRegisterAttribute : Attribute
    {
        UIRegisterAttribute()
        {

        }

        //处理被标记的UI类，提取出信息
        public static void ProgressUIRegister()
        {

        }
    }

}
