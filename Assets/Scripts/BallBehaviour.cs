using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
	[SerializeField] private int tier;
	[SerializeField] private GameObject ballObject;
	[SerializeField] private BallSettings ballSettings;
	[SerializeField] private float scaleFactor;
	private bool collidedOnce = false;

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

	public bool HasCollidedAtLeastOnce() 
	{
		return collidedOnce;
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
		collidedOnce = true;
		if (!collision.gameObject.CompareTag("Ball")) return;
		if (collision.gameObject.GetComponent<BallBehaviour>().GetTier() != tier) return;

		if (collision.gameObject.GetInstanceID() > GetInstanceID())
			CombineBalls(collision.gameObject);
    }

    private void CombineBalls(GameObject otherBall) 
	{
		GameAndUIManager.Instance.UpdateScore(ScoreFromTier(tier + 1));

        if (tier != 10)
		{
			Vector2 spawnPosition = (transform.position + otherBall.transform.position) / 2;

			GameObject newBall = Instantiate(ballObject, spawnPosition, Quaternion.identity);

			newBall.GetComponent<BallBehaviour>().SetTier(tier + 1);
		}

		Destroy(otherBall);
		Destroy(gameObject);
	}

	private int ScoreFromTier(int _tier) 
	{
		return (_tier * (_tier + 1)) / 2;
	}

	public void FreezeBall(bool value) 
	{
        GetComponent<Rigidbody2D>().bodyType = value ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = !value;
		if (!value) GetComponent<Rigidbody2D>().angularVelocity = Random.Range(0, 0.01f);
    }

    #region editorUtility
    [ContextMenu("InitializeFromTier")]
    void EditorInitalizeTier()
    {
        if (generatedTierInfoDict.Count <= 0)
            InitializeDictionary();
        InitializeFromTier();
    }
    #endregion
}