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
        Vector2Int _start;//是图形左下角
        //放框右下角
        Vector2Int _end;

        //0表示没有，其他数字表示数量
        private Dictionary<Vector2Int, int> _dicState = new Dictionary<Vector2Int, int>();

        private int totalCount = 0;

        private bool isFull = false;
        public MapStorageArea(Vector2Int center,Vector2Int size)
        {
            //从中间向两边遍历 偶数 和奇数怎么处理？
            //如果size 是偶数4，center.x = 5 那么  x[3,7]
            //如果size 是奇数3, 
            //算起点
            _size = size;
            _start = new Vector2Int(center.x - size.x/2, center.y - size.y/2 );
            for (int i = _start.x; i <= (_start.x +_size.x ); i++)
            {
                for (int j = _start.y; j <= (_start.y+ _size.y); j++)
                {
                   _dicState.Add(new Vector2Int(i,j),0);
                }
            }
           
        }

        public Vector2Int GetEmptyPos()
        {
            Debug.Log($"Start: {_start} End: {_end}");
            Vector2Int pos = Vector2Int.zero;
            for (int i = _start.x; i <= (_start.x +_size.x ); i++)
            {
                for (int j = _start.y; j <= (_start.y+ _size.y); j++)
                {
                    pos.Set(i, j);
                    Debug.Log(_dicState[pos]);
                    if (_dicState[pos] == 0)
                    {
                        totalCount++;
                        _dicState[pos]++; 
                        return pos;
                    }
                }
            }
            Debug.LogError($"Get area pos Faild: area size: {_size} ");
            //isFull = true;
            return Vector2Int.zero;
        }

        public void SetPosEmpty(Vector2Int pos)
        {
            if(!Valid(pos)) return ;
            _dicState[pos] = 0;
        }

        public bool Full()
        {
            return totalCount == _dicState.Count ? true : false;
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
