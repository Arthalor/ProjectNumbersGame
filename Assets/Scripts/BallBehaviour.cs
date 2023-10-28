using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BallBehaviour : MonoBehaviour
{
	[SerializeField] private int tier;
	[SerializeField] private GameObject ballObject;
	[SerializeField] private BallSettings ballSettings;
	[SerializeField] private float scaleFactor;

    private Dictionary<int, SpriteAndSizeInfo> generatedTierInfoDict = new();

	void Start()
	{
		InitializeDictionary();
		InitializeFromTier();
	}

	private void InitializeDictionary()
	{
		foreach (TierAndInfo tierAndInfo in ballSettings.ballSizeSpriteList) 
		{
			generatedTierInfoDict.Add(tierAndInfo.tier, tierAndInfo.info);
		}
	}

    void InitializeFromTier() 
    {
		GetComponent<SpriteRenderer>().sprite = generatedTierInfoDict[tier].sprite;
		transform.localScale = Vector2.one * generatedTierInfoDict[tier].size * scaleFactor;
    }

    [ContextMenu("InitializeFromTier")]
    void EditorInitalizeTier() 
	{
		if(generatedTierInfoDict.Count <= 0)
			InitializeDictionary();
		InitializeFromTier();
	}

	public int GetTier() 
	{
		return tier;
	}

	public void SetTier(int value)
	{
		tier = value;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (!collision.gameObject.CompareTag("Ball")) return;
		if (collision.gameObject.GetComponent<BallBehaviour>().GetTier() != tier) return;

		if (collision.gameObject.GetInstanceID() > GetInstanceID())
			CombineBalls(collision.gameObject);
    }

	private void CombineBalls(GameObject otherBall) 
	{
		Vector2 spawnPosition = (transform.position + otherBall.transform.position) / 2;

		GameObject newBall = Instantiate(ballObject, spawnPosition, Quaternion.identity);

		newBall.GetComponent<BallBehaviour>().SetTier(tier + 1);

		Destroy(otherBall);
		Destroy(gameObject);
	}

	public void FreezeBall(bool value) 
	{
        GetComponent<Rigidbody2D>().bodyType = value ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = !value;
    }
}