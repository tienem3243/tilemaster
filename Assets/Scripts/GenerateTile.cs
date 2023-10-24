using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTile : MonoBehaviour
{
    public List<GameObject> tilePrefabs;
    public int amount;
    public float horizontalLimit;
    public float verticalLimit;
    private float minimumDistance=1;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero,new Vector3(horizontalLimit,verticalLimit));
    }
    private void Start()
    {
        List<Vector3> occupiedPositions = new List<Vector3>();

        for (int i = 0; i < amount; i++)
        {
            bool foundPosition = false;
            int maxAttempts = 100;
            int attempts = 0;

            while (!foundPosition && attempts < maxAttempts)
            {
                // Generate random position within the specified limits
                float xPos = Random.Range(-horizontalLimit / 2, horizontalLimit / 2);
                float yPos = Random.Range(-verticalLimit / 2, verticalLimit / 2);
                Vector3 spawnPosition = new Vector3(xPos, yPos, -2f);

                bool isOccupied = false;

                // Check if the spawn position is too close to any occupied position
                for (int j = 0; j < occupiedPositions.Count; j++)
                {
                    if (Vector3.Distance(spawnPosition, occupiedPositions[j]) < minimumDistance)
                    {
                        isOccupied = true;
                        break;
                    }
                }

                if (!isOccupied)
                {
                    // Generate random rotation
                    Quaternion spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

                    // Spawn tile at the random position with random rotation
                    var tileSpawn = tilePrefabs[Random.Range(0, tilePrefabs.Count - 1)];
                    Instantiate(tileSpawn, spawnPosition, spawnRotation);

                    occupiedPositions.Add(spawnPosition);
                    foundPosition = true;
                }

                attempts++;
            }
        }
    }

    

}
