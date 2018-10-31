using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSequential : MonoBehaviour
{
    [SerializeField]
    private GameObject carPrefab;
    private float leftBound;
    private float rightBound;
    private float upperBound;
    private float lowerBound;
    public float depth;
    public int carIncremement = 10;

    void Start ()
    {
        leftBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, depth)).x;
        rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f, depth)).x;
        lowerBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, depth)).y;
        upperBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Camera.main.pixelHeight, depth)).y;
    }

	void Update ()
    {
        if (Input.GetKeyDown("space"))
            AddCars(carIncremement);
    }

    void AddCars(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float xVal = Random.Range(leftBound, rightBound);
            float yVal = Random.Range(upperBound, lowerBound);
            Vector3 pos = new Vector3(xVal, yVal, depth);
            Quaternion rot = Quaternion.Euler(0f, 0.0f, 0f);
            var obj = Instantiate(carPrefab, pos, rot) as GameObject;
        }
    }
}
