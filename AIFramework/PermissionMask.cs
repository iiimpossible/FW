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

//32λ���ͣ�
public class PermissionMask
{

    private int flag;

    //��������Ȩ��
    public void SetPermistion(EBitMask mask)
    {
        flag = (int)mask;
    }

    //����Ȩ��
    public void Enable(EBitMask mask)
    {
        //mask = 011  flag = 100  011 | 100 = 111
        flag |= (int)mask;
    }

    //�ر�Ȩ��
    public void Disable(EBitMask mask)
    {
        //mask = 001 ~mask = 110  flag & ~mask     001 & 110 = 000   011 & 100 = 000
        flag &= ~(int)mask;
    }

    //�Ƿ�ӵ��ĳȨ��
    public bool isAllow()
    {
        return false;
    }

    //�Ƿ�δӵ��Ȩ��
    public bool IsNotAllow()
    {
        return false;
    }

    //�Ƿ��ӵ��ĳЩȨ��
    public bool isOnlyAllow()
    {
        return false;
    }


}