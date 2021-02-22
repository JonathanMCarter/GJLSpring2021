using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public interface IPossessable
    {
        bool IsPossessed { get; set; }
        GameObject GetGameObject { get; set; }
    }
}