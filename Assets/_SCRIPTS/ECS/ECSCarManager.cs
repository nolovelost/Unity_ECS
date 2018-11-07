using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSCarManager : MonoBehaviour
{
    public ECSCarManager instance = null;

    EntityManager em;

    [SerializeField]
    private GameObject carPrefab;
    public float leftBound;
    public float rightBound;
    public float upperBound;
    public float lowerBound;
    public float depth;
    public int carIncremementAmount = 10;
    public float movementSpeed = 5.0f;
    [Unity.Collections.ReadOnly] public int currentCarAmount = 0;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        em = World.Active.GetOrCreateManager<EntityManager>();

        leftBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, depth)).x;
        rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f, depth)).x;
        lowerBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, depth)).y;
        upperBound = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Camera.main.pixelHeight, depth)).y;
    }

    void Update ()
    {
        if (Input.GetKeyDown("space"))
            AddCars();
    }

    void AddCars()
    {
        NativeArray<Entity> carEntities = new NativeArray<Entity>(carIncremementAmount, Allocator.Temp);

        /// This takes the valid ECS component data from a GameObject and fills the NativeArray with
        /// entities having those components.
        em.Instantiate(carPrefab, carEntities);

        for (int i = 0; i < carIncremementAmount; i++)
        {
            float xVal = UnityEngine.Random.Range(leftBound, rightBound);
            float yVal = UnityEngine.Random.Range(upperBound, lowerBound);

            em.SetComponentData<Position>(carEntities[i],
                new Position
                {
                    Value = new float3(xVal, yVal, depth)
                });
            em.SetComponentData<Rotation>(carEntities[i],
                new Rotation
                {
                    Value = new quaternion(Quaternion.Euler(90.0f, 0.0f, 180.0f).x,
                        Quaternion.Euler(90.0f, 0.0f, 180.0f).y,
                        Quaternion.Euler(90.0f, 0.0f, 180.0f).z,
                        Quaternion.Euler(90.0f, 0.0f, 180.0f).w)
                });
            em.SetComponentData<MoveSpeed>(carEntities[i],
                new MoveSpeed
                {
                    value = movementSpeed
                });

            currentCarAmount++;
        }

        /// We do not need the NativeArray anymore after instantiations as the entities
        /// are handled by the EntityManager now.
        carEntities.Dispose();
    }
}
