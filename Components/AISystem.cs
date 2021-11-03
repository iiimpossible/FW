using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : MonoBehaviour
{
     private ActorController _controller;
     
    private void Awake() {
        //因为这个组件比gameOject还先实例吗？
        //_controller = new ActorController(transform.gameObject);
    }

    private void Start() {
        _controller = new ActorController(transform.gameObject);
        _controller.Start();
    }
    private void Update() {
        _controller.Update();
    }
}
