using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoCarTransform : MonoBehaviour
{
    public MonoSequential monoSeq;

    void Update ()
    {
        transform.position += (transform.up * -1.0f) * monoSeq.movementSpeed * Time.deltaTime;

        if (transform.position.y < monoSeq.lowerBound)
        {
            Vector3 pos = transform.position;
            pos.y = monoSeq.upperBound;
            transform.position = pos;
        }
    }
}
