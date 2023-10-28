using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class TileManager : MonoBehaviourSingleton<TileManager>
{

    public GameObject tilePrefabs;
    public float horizontalLimit = 10f;
    public float verticalLimit = 10f;
    public float minimumDistance = 2f;
    public float lowerAngle = 45f;
    public float upperAngle = 315f;
    public int maxAttempts = 100;

    private int totalTileCount;
    public List<GameObject> allTileObj;
    public List<Transform> occupiedPositions;


    public int TotalTileCount { get => totalTileCount; set => totalTileCount = Math.Clamp(value, 0, Int32.MaxValue); }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(horizontalLimit * 2, verticalLimit * 2, 0f));
    }

    private void Start()
    {

        //init coccupiedPosition map
        occupiedPositions = new List<Transform>();

        //calculate spawn
    }

    public bool LoadLevel(string lvName, SpawnType spawn =SpawnType.SPAWNBYRATE)
    {
        LevelConfig data = Resources.Load<LevelConfig>("Data/lv/" + lvName);
        if (data == null) return false;
        if (spawn==SpawnType.SPAWNBYPERCENT)
        {
            data?.tileConfigs.ForEach(
            config =>
            {
                double s = data.CalculateNormalize(config);
                int trioToSpawn = (int)Math.Round(s * (double)data.maxAmount / 3d);
                trioToSpawn = Math.Clamp(trioToSpawn, 1, Int32.MaxValue);
                SpawnTile(tilePrefabs, trioToSpawn * 3
                    , config);
            });
        }
        else
        {
            int i = Random.Range(0, 100);
            for (int s = 0; s < data.maxAmount/3; s++)
            {
               SpawnByChance(data, 3);
            }

        }

        GameManager.Instance.currentLv = lvName;
        GameManager.Instance.Init();
        totalTileCount = allTileObj.Count;
        return true;
    }
    public void SpawnByChance(LevelConfig config,int amountOfDuplicate)
    {
  
        // Generate a random number between 0 and 1
        float randomValue = Random.Range(0,config.GetTotalRate());

        // Track the cumulative probability as we iterate over the items
        float cumulativeProbability = 0f;

        // Iterate over the drop chances and check if the random value falls within the range
        for (int i = 0; i < config.tileConfigs.Count; i++)
        {
            cumulativeProbability += config.tileConfigs[i].chance;

            // If the random value is less than the cumulative probability, drop the corresponding item
            if (randomValue < cumulativeProbability)
            {

                // Instantiate the item prefab at the specified spawn position
                SpawnTile(tilePrefabs, amountOfDuplicate, config.tileConfigs[i]);
                
                // Apply any additional logic or behavior to the dropped item, if needed

                // Start any animations or effects associated with the dropped item

                break; // Exit the loop once the item is dropped
            }
        }
    }

    private void SpawnTile(GameObject prefab, int amount, TileConfig config, int zDepth = -2)
    {
        int succeedSpawn = 0;
        for (int i = 0; i < amount; i++)
        {
            int at = 0;

            bool foundPosition = false;

            while (!foundPosition && at < maxAttempts)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-horizontalLimit, horizontalLimit), Random.Range(-verticalLimit, verticalLimit), zDepth);

                bool positionOccupied = false;
                foreach (Transform occupiedPosition in occupiedPositions)
                {
                    if (Vector3.Distance(spawnPosition, occupiedPosition.position) < minimumDistance)
                    {
                        positionOccupied = true;
                        break;
                    }
                }

                if (!positionOccupied)
                {
                    Quaternion spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

                    // Clamp the generated rotation within the desired range
                    float zRotation = spawnRotation.eulerAngles.z;
                    if (zRotation < lowerAngle)
                    {
                        spawnRotation = Quaternion.Euler(0f, 0f, lowerAngle);
                    }
                    else if (zRotation > upperAngle)
                    {
                        spawnRotation = Quaternion.Euler(0f, 0f, upperAngle);
                    }

                    //init prefab
                    var tileInstance = Instantiate(prefab, spawnPosition, spawnRotation) as GameObject;
                    allTileObj.Add(tileInstance);
                    Rigidbody rigidbody = tileInstance.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.MovePosition(spawnPosition);
                    }

                    TileAdapter(config, tileInstance);
                    //add to map position to prevent intersect
                    occupiedPositions.Add(tileInstance.transform);
                    succeedSpawn++;
                    foundPosition = true;
                }

                at++;
            }
        }
        if (succeedSpawn < amount)
        {
            zDepth -= 1;
            SpawnTile(prefab, amount - succeedSpawn, config, zDepth);
        }


    }
    public void Reset()
    {
        occupiedPositions.Clear();
        allTileObj.ForEach(x => Destroy(x));
        allTileObj.Clear();
    }
    private static void TileAdapter(TileConfig config, GameObject tileInstance)
    {
        var tile = tileInstance.gameObject.GetComponent<Tile>();
        tile.Sprite.sprite = config.sprite;
        tile.tileName = config.tileName;
    }
}
