using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BallSettings")]
public class BallSettings : ScriptableObject
{
    public List<TierAndInfo> ballSizeSpriteList;
}

[Serializable]
public struct SpriteAndSizeInfo
{
    public Sprite sprite;
    public float size;
}

[Serializable]
public struct TierAndInfo
{
    public int tier;
    public SpriteAndSizeInfo info;
}