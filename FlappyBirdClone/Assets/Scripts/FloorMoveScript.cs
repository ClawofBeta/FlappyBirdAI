﻿using UnityEngine;
using System.Collections;

public class FloorMoveScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x < -3f)
        {
            transform.localPosition = new Vector3(3, transform.localPosition.y, transform.localPosition.z);
        }
        transform.Translate(-Time.deltaTime, 0, 0);
    }


}
