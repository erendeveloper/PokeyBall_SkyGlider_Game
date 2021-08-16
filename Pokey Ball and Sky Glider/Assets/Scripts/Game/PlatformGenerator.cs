using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generatesd platforms on the ground
//Platforms will be created on grid system randomly
//Added on main camera
public class PlatformGenerator : MonoBehaviour
{
    //prefabs
    public GameObject cubePrefab;
    public GameObject cylinderPrefab;

    public Transform generationAreaPosition; //middle position of platfotms that will be generated

    private readonly int GridLength = 10; //one grid size

    private const float PlatformDistance = 8;// distance between each cell on the grid

    //random prefab sizes
    private const float MinScaleRate = 0.5f;
    private const float MaxScaleRate = 1.5f;
    private const float MaxDeviationRate = 0.25f; //deviation for position X and Z
    private const float MaxDeviationDistanceY= 2f;  //deviation for position Y

    //platform creation possibility
    private const int CreateChancePercent = 50;
    private const int MaxCreateChancePercent = 100;
    // Start is called before the first frame update

    void Awake()
    {
        Generate();
    }

    // Update is called once per frame
    void Generate() //generates paltforms
    {
        float leftBottomPositionX;
        float leftBottomPositionZ;

       if(GridLength %2 == 0)
        {
            leftBottomPositionX = generationAreaPosition.position.x - GridLength / 2 - PlatformDistance / 2;
            leftBottomPositionZ = generationAreaPosition.position.z - GridLength / 2 - PlatformDistance / 2;
        }
        else
        {
            leftBottomPositionX = generationAreaPosition.position.x - (GridLength - 1) / 2;
            leftBottomPositionZ = generationAreaPosition.position.z - (GridLength - 1) / 2;
        }
        FillGrid(leftBottomPositionX, leftBottomPositionZ);
        DisableScript();
    }
    void FillGrid(float leftBottomPositionX, float leftBottomPositionZ)//spawn items on grid cells
    {
        for (int z = 0; z < GridLength; z++)
        {
            for (int x = 0; x < GridLength; x++)
            {
                Vector3 position = new Vector3(leftBottomPositionX + x * PlatformDistance, 0, leftBottomPositionZ + z * PlatformDistance);
                SpawnItem(position);
            }
        }
    }
    void SpawnItem(Vector3 position)
    {
        if (RandomCreateChance())
        {
            int itemType = Random.Range(1, 3);
            Vector3 scaleFactor;
            GameObject prefabType;
            if (itemType == 1) //cube
            {
                prefabType = cubePrefab;
                scaleFactor = RandomCubeScaleFactor();
            }
            else //cylinder
            {
                prefabType = cylinderPrefab;
                scaleFactor = RandomCylinderScaleFactor();
            }
            GameObject item = Instantiate(prefabType, position + RandomPositionOffset(), Quaternion.identity);
            item.transform.localScale = new Vector3(item.transform.localScale.x * scaleFactor.x, item.transform.localScale.y, item.transform.localScale.z * scaleFactor.z);
        }
        
   }

    bool RandomCreateChance()//possibility creation on a cell of grid
    {
        int createChance = Random.Range(0, MaxCreateChancePercent);
        if (createChance < CreateChancePercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    Vector3 RandomPositionOffset()//Deviates platforms little bit
    {
        float deviationRateX = Random.Range(0, MaxDeviationRate);
        float x = PlatformDistance * deviationRateX * RandomSign();

        float deviationRateZ = Random.Range(0, MaxDeviationRate);
        float z = PlatformDistance * deviationRateX * RandomSign();

        float y = Random.Range(0, MaxDeviationDistanceY) * RandomSign();

        return new Vector3(x, y, z);
    }
    Vector3 RandomCubeScaleFactor()
    {
        float x = Random.Range(MinScaleRate, MaxScaleRate);

        float z= Random.Range(MinScaleRate, MaxScaleRate);

        return new Vector3(x, 1, z);
    }
    Vector3 RandomCylinderScaleFactor()
    {
        float scaleRate= Random.Range(MinScaleRate, MaxScaleRate);

        return new Vector3(scaleRate, 1, scaleRate);
    }
    int RandomSign()
    {
        int chance = Random.Range(0, 2);
        if (chance == 1)
        {
            return 1;
        }
        else return -1;
    }
    void DisableScript()//Disabling script after generation
    {
        this.enabled = false;
    }
}
