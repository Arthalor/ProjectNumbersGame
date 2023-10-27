using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
	[field: SerializeField] public int tier { get; private set; }
	[SerializeField] private List<TierAndInfo> fruitSizeSpriteList;
	private Dictionary<int, SpriteAndSizeInfo> generatedTierInfoDict = new();

	[Serializable]
	struct SpriteAndSizeInfo
	{
		public Sprite sprite;
		public float size;
	}

	[Serializable]
	struct TierAndInfo
	{
		public int tier;
		public SpriteAndSizeInfo info;
	}

	void Start()
	{
		InitializeDictionary();
		InitializeFromTier();
	}

	private void InitializeDictionary()
	{
		foreach (TierAndInfo tierAndInfo in fruitSizeSpriteList) 
		{
			generatedTierInfoDict.Add(tierAndInfo.tier, tierAndInfo.info);
		}
	} 

    void InitializeFromTier() 
    {
		GetComponent<SpriteRenderer>().sprite = generatedTierInfoDict[tier].sprite;
		transform.localScale = Vector2.one * generatedTierInfoDict[tier].size;
    }
}