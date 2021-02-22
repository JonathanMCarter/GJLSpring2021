using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TotallyNotEvil
{
    public interface IThinkable
    {
        bool HasShownThought { get; set; }
        Thought CurrentThought { get; set; }
        AssetReference Bubble { get; set; }
        void ShowBubble();
    }
}