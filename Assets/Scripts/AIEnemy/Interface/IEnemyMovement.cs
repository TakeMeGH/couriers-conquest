using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovement
{
    Rigidbody RB { get; set; }

    void Chasing(Vector2 velocity);
    void Patroling(Vector2 velocity);
}
