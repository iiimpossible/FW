using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{

    public float speed = 10.0f;
    Vector3 tgPos = new Vector3();

    List<Vector2> poss = new List<Vector2>();
   
   Vector3 dir = new Vector3();
    void Start()
    {
        ScptInputManager.instance.eventMouseInWorldPos += AddPos;
    }
   
    void Update()
    {
        Move();
    }

    private void Move()
    {
       if(poss.Count>0)
       {
           dir = poss[0] - (Vector2) transform.position;
           transform.Translate(dir.x * Time.deltaTime * speed,dir.y * Time.deltaTime* speed,transform.position.z);
           if(Arrive(poss[0]))
           {
               poss.RemoveAt(0);
           }           
       }
    }

    private void AddPos(Vector2Int pos)
    {
        this.poss.Add(pos);
    }

    public bool Arrive(Vector3 pos)
    {
        if ((pos - transform.position).sqrMagnitude < 0.01f)
            return true;
        return false;
    }

    public Vector2 GetDir(Vector3 tg)
    {
        return (tg - transform.position).normalized;
    }
    
    //行为

    
}
