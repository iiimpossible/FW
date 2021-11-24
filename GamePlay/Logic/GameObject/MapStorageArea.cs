using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GraphyFW.GamePlay
{
    /// <summary>
    /// 地图上的存储区
    /// 1.地图坐标系的存储区范围
    /// 2.每个位置是否有道具
    /// 3.
    /// </summary>
    public class MapStorageArea
    {
        Vector2Int _size;
        Vector2Int _center;

        //方框左上角
        Vector2Int _start;
        //放框右下角
        Vector2Int _end;

        //0表示没有，其他数字表示数量
        private int[,] _arrayState;

        private int totalCount = 0;

        public MapStorageArea(Vector2Int center,Vector2Int size)
        {
            _size = size;
             _arrayState = new int[size.x,size.y];
        }

        public Vector2Int GetEmptyPos()
        {
            for(int i = 0; i <_size.x;i++ )
            {
                for(int j = 0; j < _size.y; j++)
                {
                    if(_arrayState[i,j] == 0)
                    {
                        totalCount += 1;
                        return new Vector2Int(i,j);
                    }
                }
            }
            return Vector2Int.zero;
        }

        public void SetPosEmpty(Vector2Int pos)
        {
            if(!Valid(pos)) return ;
            _arrayState[pos.x,pos.y] = 0;
        }

        public bool Full()
        {
            return totalCount == _size.x * _size.y? true : false;
        }

        private bool Valid(Vector2Int pos)
        {
            if(pos.x < _size.x && pos.x >= 0)
            {
                if(pos.y < _size.y && pos.y >= 0)
                {
                    return true;
                }
            }
            return false;
        }



    }

}
