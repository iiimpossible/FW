using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : MonoBehaviour
{
     private ActorController _controller;
     
    private void Awake() {
        _controller = new ActorController(transform.gameObject);
    }

    private void Update() {
        _controller.Update();
    }
}
