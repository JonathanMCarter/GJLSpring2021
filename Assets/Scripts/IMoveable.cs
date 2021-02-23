using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public interface IMoveable
    {
        float MoveSpeed { get; set; }
        float JumpHeight { get; set; }
        void MoveAction(Vector2 dir);
        void JumpAction();
    }
}