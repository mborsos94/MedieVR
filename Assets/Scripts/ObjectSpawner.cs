using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {
    public int objectMax = 10;
    public int startObjectMax = 5;
    public GameObject[] ObjectsToSpawn;
    public float radiusX, radiusZ;
    private int objectsRemaining;
    private int totalObjectsSpawned;

    // Use this for initialization
    void Start () {
        SpawnStartingObjects();
        GameStats.remainingEnemies = objectsRemaining = objectMax;
	}
	
	// Update is called once per frame
	void Update () {
        if (objectsRemaining != GameStats.remainingEnemies && GameStats.remainingEnemies > 0)
        {
            while (objectsRemaining > GameStats.remainingEnemies && objectMax > totalObjectsSpawned)
            {
                int index = Random.Range(0, ObjectsToSpawn.Length);
                SpawnObject(index, objectMax - startObjectMax - GameStats.remainingEnemies);
                objectsRemaining--;
            }
        }
	}

    private void SpawnStartingObjects()
    {
        for(int i = 0; i < startObjectMax; i++)
        {
            int index = Random.Range(0, ObjectsToSpawn.Length);
            SpawnObject(index, i);
        }
    }

    private void SpawnObject(int index, int currNum)
    {
        var centrePos = new Vector3(0, 0, 0);
        var i = (currNum * 1.0) / objectMax;
        var angle = i * Mathf.PI * 2;
        // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
        var x = Mathf.Sin((float)angle) * radiusX;
        var z = Mathf.Cos((float)angle) * radiusZ;
        var pos = new Vector3(x, ObjectsToSpawn[index].transform.position.y, z) + centrePos;
        GameObject objectSpawned = Instantiate(ObjectsToSpawn[index], pos, Quaternion.Euler(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359)));
        objectSpawned.SetActive(true);
        totalObjectsSpawned++;
    }
}
