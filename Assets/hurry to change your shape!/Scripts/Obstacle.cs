using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public GameObject leftCube;
    public GameObject rightCube;
    public GameObject topCube;

    void Start()
    {
        float x = 1.7f;

        // Якщо включено режим кліків — вибираємо чітко з 5 варіантів стін
        if (PlayerLogic.useDiscreteMode)
        {
            float[] xChoices = new float[] { 0.9f, 1.3f, 1.7f, 2.1f, 2.5f };
            x = xChoices[Random.Range(0, xChoices.Length)];
        }
        // Якщо оригінальний режим свайпів — повний непередбачуваний рандом
        else
        {
            x = Random.Range(0.9f, 2.5f);
        }

        float y = (2.5f / x) + (0.9f / x) + 0.36f;

        leftCube.transform.localPosition = new Vector3(-x, (y - 1) / 2, 0);
        leftCube.transform.localScale = new Vector3(1, y, 1);
        rightCube.transform.localPosition = new Vector3(x, (y - 1) / 2, 0);
        rightCube.transform.localScale = new Vector3(1, y, 1);

        topCube.transform.localPosition = new Vector3(0, y, 0);
        topCube.transform.localScale = new Vector3((x * 2 + 1), 1, 1);
    }
}