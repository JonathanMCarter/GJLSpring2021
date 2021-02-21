using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public interface IPocessable
    {
        bool IsPocessed { get; set; }
        GameObject obj { get; set; }
    }
}