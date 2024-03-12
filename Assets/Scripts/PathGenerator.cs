using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    //       |+z
    //       |
    //-x     |      +x
    //----------------
    //       |
    //       |
    //       |-z

    [SerializeField] // these coordinates are where it's going out (from bottom and from right)
    GameObject pathStraight; //S, -z +z
    [SerializeField]
    GameObject pathTurn; //T, in from: +z +x, out to: -z -x
    [SerializeField]
    GameObject pathSplit; //P, in from: +z, out to: -x +x
    [SerializeField]
    GameObject pathSpawnPoint;

    [Header("L-system generation")]
    [SerializeField]
    string generationVariableS = "TTP";
    [SerializeField]
    string generationVariableT = "TST";
    [SerializeField]
    string generationVariableP = "STS";

    public Vector3 firstLocation = Vector3.zero;

    EnemySpawner enemySpawner;
    public int pathCount;

    class GenerationLeaf
    {
        public GameObject lastPath;
        public int faceX;
        public int faceZ;
        public Vector3 endLocation;
        public bool spawnFound;
        public bool upFree;
        public bool downFree;
        public bool leftFree;
        public bool rightFree;
        public int pathIndex;
    }

    [SerializeField]
    float offset;

    List<GenerationLeaf> paths;
    List<GenerationLeaf> pathsToAdd;

    public void Start()
    {
        pathCount = 0;
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
        paths = new List<GenerationLeaf>();
        pathsToAdd = new List<GenerationLeaf>();
        paths.Add(new GenerationLeaf());
        paths[0].faceX = 0;
        paths[0].faceZ = 1;
        paths[0].endLocation = firstLocation;
        paths[0].spawnFound = false;
        paths[0].pathIndex = pathCount;
    }
    public void GeneratePaths()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            if (!paths[i].spawnFound) AddSinglePath(paths[i]);
        }
        paths.AddRange(pathsToAdd);
        pathsToAdd = new List<GenerationLeaf>();
    }
    bool CheckIfSpawn(GenerationLeaf info)
    {
        //         + < Check location (y+offset)
        // Ray    /|
        // cast> / |
        //      /  | < offset
        //     /   |
        //    +----+ < Last tile
        //    ^
        //    Place to check

        Vector3 checkLocation = new Vector3((-info.faceX * offset) + info.endLocation.x, info.endLocation.y + offset, (-info.faceZ * offset) + info.endLocation.z);
        info.rightFree = true;
        info.leftFree = true;
        info.upFree = true;
        info.downFree = true;
        if (info.faceZ == -1)
        {
            info.downFree = false;
            // need to check +z -x +x
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.left + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.leftFree = false;
            }
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.forward + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.upFree = false;
            }
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.right + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.rightFree = false;
                if (!info.leftFree && !info.upFree)
                {
                    return true;
                }
            }
            return false;
        }
        else if (info.faceX == -1)
        {
            info.leftFree = false;
            // need to check +z -z +x
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.back + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.downFree = false;
            }
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.forward + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.upFree = false;
            }
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.right + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.rightFree = false;
                if (!info.downFree && !info.upFree)
                {
                    return true;
                }
            }
            return false;
        }
        else if (info.faceZ == 1)
        {
            info.upFree = false;
            // need to check +x -x -z
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.right + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.rightFree = false;
            }
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.left + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.leftFree = false;
            }
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.back + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.downFree = false;
                if (!info.rightFree && !info.leftFree)
                {
                    return true;
                }
            }
            return false;
        }
        else //faceX == 1
        {
            info.rightFree = false;
            // need to check +z -x -z
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.forward + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.upFree = false;
            }
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.left + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.leftFree = false;
            }
            if (Physics.Raycast(checkLocation, Vector3.Normalize(Vector3.back + Vector3.down), offset * 2, LayerMask.GetMask("Path")))
            {
                info.downFree = false;
                if (!info.upFree && !info.leftFree)
                {
                    return true;
                }
            }
            return false;
        }
    }
    void PlaceFinal(GenerationLeaf info)
    {
        float rotationToAdd = 0;
        if (info.faceX == 1)
        {
            rotationToAdd = -90;
        }
        else if (info.faceX == -1)
        {
            rotationToAdd = 90;
        }
        else if (info.faceZ == 1)
        {
            rotationToAdd = 180;
        }
        Vector3 placeLocation = new Vector3((-info.faceX * offset) + info.endLocation.x, info.endLocation.y, (-info.faceZ * offset) + info.endLocation.z);
        GameObject pathToPlace = Instantiate(pathSpawnPoint, transform);
        pathToPlace.transform.Rotate(0, rotationToAdd, 0);
        pathToPlace.transform.position = placeLocation;
        info.spawnFound = true;
    }
    void AddSinglePath(GenerationLeaf info, float randomNumber = -1)
    {
        if (randomNumber < 0) 
        { 
            randomNumber = UnityEngine.Random.Range(0, 2.25f);

            if (CheckIfSpawn(info))
            {
                PlaceFinal(info);
                return;
            }
        }
        GameObject pathToPlace;
        int newFaceX = 0;
        int newFaceZ = 0;
        float rotationToAdd = 0;

        if (randomNumber >= 0 && randomNumber < 1) //straight
        {
            if (info.faceZ == -1 && info.upFree)
            {
                newFaceZ = info.faceZ;
                newFaceX = 0;
            }
            else if (info.faceZ == 1 && info.downFree)
            {
                newFaceZ = info.faceZ;
                newFaceX = 0;
            }
            else if (info.faceX == -1 && info.rightFree)
            {
                rotationToAdd = 90;
                newFaceX = info.faceX;
                newFaceZ = 0;
            }
            else if (info.faceX == 1 && info.leftFree)
            {
                rotationToAdd = 90;
                newFaceX = info.faceX;
                newFaceZ = 0;
            }
            else
            {
                // make a turn
                AddSinglePath(info, 1);
                return;
            }
            pathToPlace =  Instantiate(pathStraight, transform);
        }
        else if (randomNumber >= 1 && randomNumber < 2)//turn, default turn - L (up-right/right-up)
        {
            if (info.faceZ == 1)
            {
                if (info.leftFree)
                {
                    if (info.rightFree) // if both directions possible, randomize
                    {
                        int sideFlip = UnityEngine.Random.Range(0, 2);
                        if (sideFlip == 0)
                        {
                            // go up-left
                            rotationToAdd = -90;
                            newFaceX = 1;
                            newFaceZ = 0;
                        }
                        else
                        {
                            // go up-right
                            newFaceX = -1;
                            newFaceZ = 0;
                        }
                    }
                    else
                    {
                        // go up-left
                        rotationToAdd = -90;
                        newFaceX = 1;
                        newFaceZ = 0;
                    }
                }
                else if (info.rightFree)
                {
                    // go up-right
                    newFaceX = -1;
                    newFaceZ = 0;
                }
                else
                {
                    // go straight
                    AddSinglePath(info, 0);
                    return;
                }
            }
            else if (info.faceX == 1)
            {
                if (info.upFree)
                {
                    if (info.downFree) // if both directions possible, randomize
                    {
                        int sideFlip = UnityEngine.Random.Range(0, 2);
                        if (sideFlip == 0)
                        {
                            // go right-up
                            newFaceX = 0;
                            newFaceZ = -1;
                        }
                        else
                        {
                            // go right-down
                            rotationToAdd = 90;
                            newFaceX = 0;
                            newFaceZ = 1;
                        }
                    }
                    else
                    {
                        // go right-up
                        newFaceX = 0;
                        newFaceZ = -1;
                    }
                }
                else if (info.downFree)
                {
                    // go right-down
                    rotationToAdd = 90;
                    newFaceX = 0;
                    newFaceZ = 1;
                }
                else
                {
                    // go straight
                    AddSinglePath(info, 0);
                    return;
                }
            }
            else if (info.faceX == -1)
            {
                if (info.upFree)
                {
                    if (info.downFree) // if both directions possible, randomize
                    {
                        int sideFlip = UnityEngine.Random.Range(0, 2);
                        if (sideFlip == 0)
                        {
                            // go left-up
                            rotationToAdd = -90;
                            newFaceX = 0;
                            newFaceZ = -1;
                        }
                        else
                        {
                            // go left-down
                            rotationToAdd = 180;
                            newFaceX = 0;
                            newFaceZ = 1;
                        }
                    }
                    else
                    {
                        // go left-up
                        rotationToAdd = -90;
                        newFaceX = 0;
                        newFaceZ = -1;
                    }
                }
                else if (info.downFree)
                {
                    // go left-down
                    rotationToAdd = 180;
                    newFaceX = 0;
                    newFaceZ = 1;
                }
                else
                {
                    // go straight
                    AddSinglePath(info, 0);
                    return;
                }
            }
            else // faceZ == -1
            {
                {
                    if (info.leftFree)
                    {
                        if (info.rightFree) // if both directions possible, randomize
                        {
                            int sideFlip = UnityEngine.Random.Range(0, 2);
                            if (sideFlip == 0)
                            {
                                // go down-left
                                rotationToAdd = 180;
                                newFaceX = 1;
                                newFaceZ = 0;
                            }
                            else
                            {
                                // go down-right
                                rotationToAdd = 90;
                                newFaceX = -1;
                                newFaceZ = 0;
                            }
                        }
                        else
                        {
                            // go down-left
                            rotationToAdd = 180;
                            newFaceX = 1;
                            newFaceZ = 0;
                        }
                    }
                    else if (info.rightFree)
                    {
                        // go down-right
                        rotationToAdd = 90;
                        newFaceX = -1;
                        newFaceZ = 0;
                    }
                    else
                    {
                        // go straight
                        AddSinglePath(info, 0);
                        return;
                    }
                }
            }
            pathToPlace = Instantiate(pathTurn, transform);
        }
        else //split, default T (down - left/right)
        {
            if (info.faceZ == 1)
            {
                if (info.leftFree && info.rightFree)
                {
                    rotationToAdd = 180;
                    newFaceX = 1;
                    newFaceZ = 0;
                    GenerationLeaf newLeaf = new GenerationLeaf();
                    newLeaf.faceX = -1;
                    newLeaf.faceZ = 0;
                    newLeaf.endLocation = new Vector3((-info.faceX * offset) + info.endLocation.x, info.endLocation.y, (-info.faceZ * offset) + info.endLocation.z);
                    pathCount++;
                    newLeaf.pathIndex = pathCount;
                    enemySpawner.AddNewPath(newLeaf.endLocation, newLeaf.pathIndex, info.pathIndex);
                    newLeaf.spawnFound = false;
                    pathsToAdd.Add(newLeaf);
                }
                else if (info.faceZ == -1)
                {
                    // go straight
                    AddSinglePath(info, 0);
                    return;
                }
                else
                {
                    // turn
                    AddSinglePath(info, 1);
                    return;
                }
            }
            else if (info.faceX == 1)
            {
                if (info.upFree && info.downFree)
                {
                    rotationToAdd = -90;
                    newFaceX = 0;
                    newFaceZ = 1;
                    GenerationLeaf newLeaf = new GenerationLeaf();
                    newLeaf.faceX = 0;
                    newLeaf.faceZ = -1;
                    newLeaf.endLocation = new Vector3((-info.faceX * offset) + info.endLocation.x, info.endLocation.y, (-info.faceZ * offset) + info.endLocation.z);
                    pathCount++;
                    newLeaf.pathIndex = pathCount;
                    enemySpawner.AddNewPath(newLeaf.endLocation, newLeaf.pathIndex, info.pathIndex);
                    newLeaf.spawnFound = false;
                    pathsToAdd.Add(newLeaf);
                }
                else if (info.faceZ == -1)
                {
                    // go straight
                    AddSinglePath(info, 0);
                    return;
                }
                else
                {
                    // turn
                    AddSinglePath(info, 1);
                    return;
                }
            }
            else if (info.faceX == -1)
            {
                if (info.upFree && info.downFree)
                {
                    rotationToAdd = 90;
                    newFaceX = 0;
                    newFaceZ = 1;
                    GenerationLeaf newLeaf = new GenerationLeaf();
                    newLeaf.faceX = 0;
                    newLeaf.faceZ = -1;
                    newLeaf.endLocation = new Vector3((-info.faceX * offset) + info.endLocation.x, info.endLocation.y, (-info.faceZ * offset) + info.endLocation.z);
                    pathCount++;
                    newLeaf.pathIndex = pathCount;
                    enemySpawner.AddNewPath(newLeaf.endLocation, newLeaf.pathIndex, info.pathIndex);
                    newLeaf.spawnFound = false;
                    pathsToAdd.Add(newLeaf);
                }
                else if (info.faceZ == -1)
                {
                    // go straight
                    AddSinglePath(info, 0);
                    return;
                }
                else
                {
                    // turn
                    AddSinglePath(info, 1);
                    return;
                }
            }
            else // faceZ == -1
            {
                if (info.leftFree && info.rightFree)
                {
                    newFaceX = 1;
                    newFaceZ = 0;
                    GenerationLeaf newLeaf = new GenerationLeaf();
                    newLeaf.faceX = -1;
                    newLeaf.faceZ = 0;
                    newLeaf.endLocation = new Vector3((-info.faceX * offset) + info.endLocation.x, info.endLocation.y, (-info.faceZ * offset) + info.endLocation.z);
                    pathCount++;
                    newLeaf.pathIndex = pathCount;
                    enemySpawner.AddNewPath(newLeaf.endLocation, newLeaf.pathIndex, info.pathIndex);
                    newLeaf.spawnFound = false;
                    pathsToAdd.Add(newLeaf);
                }
                else if (info.faceZ == -1)
                {
                    // go straight
                    AddSinglePath(info, 0);
                    return;
                }
                else
                {
                    // turn
                    AddSinglePath(info, 1);
                    return;
                }
            }
            pathToPlace = Instantiate(pathSplit, transform);
        }
        Vector3 placeLocation = new Vector3((-info.faceX * offset) + info.endLocation.x, info.endLocation.y, (-info.faceZ * offset) + info.endLocation.z);
        info.endLocation = placeLocation;
        pathToPlace.transform.Rotate(0, rotationToAdd, 0);
        pathToPlace.transform.position = placeLocation;

        enemySpawner.AddWalkPoint(placeLocation, info.pathIndex);
        
        info.faceX = newFaceX; 
        info.faceZ = newFaceZ;
    }
}
