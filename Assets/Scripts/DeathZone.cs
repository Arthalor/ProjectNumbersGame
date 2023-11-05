using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public delegate void DeathDelegate();
    public event DeathDelegate deathEvent;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BallBehaviour ball))
        {
            if (ball.HasCollidedAtLeastOnce())
            {
                collision.GetComponent<SpriteRenderer>().color = Color.red;
                deathEvent?.Invoke();
            }
        }
    }
}