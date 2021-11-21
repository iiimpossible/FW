using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBitMask
{
    NONE = 0,
    ACSSESS = 1 << 0,
    OBSTACLE = 1 << 1,
    FOUND = 1 << 2,
    ALL = NONE | ACSSESS | FOUND
}

//32λ���ͣ�
public class PermissionMask
{

    private int flag;

    //��������Ȩ��
    public void SetPermistion(EBitMask mask)
    {
        flag = (int)mask;
    }

    public static int SetPermistion(int mask,ref int flag)
    {
        return flag = mask;
    }    

    //����Ȩ��
    public static int Enable(int mask ,ref int flag)
    {
         //mask = 011  flag = 100  011 | 100 = 111
        return flag |= mask;
    } 

    public static int Disable(int mask, ref int flag)
    {
         //mask = 001 ~mask = 110  flag & ~mask     001 & 110 = 000   011 & 100 = 000
        return flag &= ~(int)mask;
    }

    public static bool ISAllow(int mask, int flag)
    {
        // mask = 001 falg = 011  mask & falg = 001
        //mask = 000 flag = 000
        return (mask & flag) == flag;       
    }

    //�Ƿ������ĳЩȨ��
    public static bool IsNotAllow(int mask ,int flag)
    {
        return ( mask & flag ) == 0;
    }

    //�Ƿ��ӵ��ĳЩȨ��
    public static bool isOnlyAllow(int mask, int flag)
    {
       return mask == flag;
    }

     


}