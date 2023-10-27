using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int maxSpawnTier;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject ball;

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetButtonDown("Fire1")) 
        {
            Vector3 noise = Vector3.one * Random.Range(0, 0.001f);
            noise = new Vector3(noise.x, noise.y, 1);
            GameObject gameObject = Instantiate(ball, spawnPoint.position + noise, Quaternion.identity);
            gameObject.GetComponent<BallBehaviour>().SetTier(Random.Range(0, maxSpawnTier + 1));
        }
    }

    private void Move()
    {
        float velocity = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.position += new Vector3(velocity, 0, 0);
    }
}
