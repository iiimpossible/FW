using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBitMask
{
    NONE = 0,
    ACSSESS = 1 << 0,
    OBSTACLE = 1 << 1,
    FOUND = 1 << 2
}

//32位整型？
public class PermissionMask
{

    private int flag;

    //重新设置权限
    public void SetPermistion(EBitMask mask)
    {
        flag = (int)mask;
    }

    //开启权限
    public void Enable(EBitMask mask)
    {
        //mask = 011  flag = 100  011 | 100 = 111
        flag |= (int)mask;
    }

    //关闭权限
    public void Disable(EBitMask mask)
    {
        //mask = 001 ~mask = 110  flag & ~mask     001 & 110 = 000   011 & 100 = 000
        flag &= ~(int)mask;
    }

    //是否拥有某权限
    public bool isAllow()
    {
        return false;
    }

    //是否未拥有权限
    public bool IsNotAllow()
    {
        return false;
    }

    //是否仅拥有某些权限
    public bool isOnlyAllow()
    {
        return false;
    }


}