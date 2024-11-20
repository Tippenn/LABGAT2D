using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float initialSpeed = 2f;
    public float speedIncreaseRate = 0.1f;
    public float resetPositionX = -10f;
    public float startPositionX = 10f;

    private float currSpeed;

    void Start()
    {
        Destroy(gameObject,30f);
        currSpeed = initialSpeed + LevelManager.Instance.score/1000f;
    }

    void Update()
    {
        transform.Translate(Vector2.left * currSpeed * Time.deltaTime);
    }
}
