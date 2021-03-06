﻿using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SpawnObject = SpawnObjects[Random.Range(0, SpawnObjects.Length)];
		Spawn ();
    }

    void Spawn()
    {

		if (GameStateManager.GameState == GameState.Playing)
        {
            //random y position
            float y = Random.Range(0.3f, 1.1f);
            Instantiate(SpawnObject, this.transform.position + new Vector3(0, y, 0), Quaternion.identity);
        }
		Invoke("Spawn", Random.Range(2.0f, 3.5f));

    }

    private GameObject SpawnObject;
    public GameObject[] SpawnObjects;

    //public float timeMin = 500f;
    //public float timeMax = 1000f;
}
