using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

public class JobManager : MonoBehaviour
{
    TransformAccessArray transforms;
    JobCarTransform carJob;
    JobHandle carJobHandle;

    [SerializeField]
    private GameObject carPrefab;
    public float leftBound;
    public float rightBound;
    public float upperBound;
    public float lowerBound;
    public float depth;
    public int carIncremementAmount = 10;
    public float movementSpeed = 5.0f;

    private void OnDisable()
    {
        carJobHandle.Complete();
        transforms.Dispose();
    }

    void Start()
    {
        transforms = new TransformAccessArray(0, -1);

        leftBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, depth)).x;
        rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f, depth)).x;
        lowerBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, depth)).y;
        upperBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Camera.main.pixelHeight, depth)).y;
    }

    void Update()
    {
        carJobHandle.Complete();

        if (Input.GetKeyDown("space"))
            AddCars(carIncremementAmount);

        carJob = new JobCarTransform()
        {
            upperBound = upperBound,
            lowerBound = lowerBound,
            deltaTime = Time.deltaTime,
            movementSpeed = movementSpeed
        };

        carJobHandle = carJob.Schedule(transforms);

        JobHandle.ScheduleBatchedJobs();
    }

    void AddCars(int amount)
    {
        carJobHandle.Complete();

        transforms.capacity = transforms.length + carIncremementAmount;

        for (int i = 0; i < amount; i++)
        {
            float xVal = Random.Range(leftBound, rightBound);
            float yVal = Random.Range(upperBound, lowerBound);
            Vector3 pos = new Vector3(xVal, yVal, depth);
            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            var obj = Instantiate(carPrefab, pos, rot) as GameObject;
            transforms.Add(obj.transform);
        }
    }
}
