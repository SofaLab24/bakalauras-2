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

    [Header("L-system generation")]
    [SerializeField]
    string generationVariableS = "TTP";
    [SerializeField]
    string generationVariableT = "TST";
    [SerializeField]
    string generationVariableP = "STS";

    class GenerationLeaf
    {
        public GameObject lastPath;
        public int faceX;
        public int faceZ;
        public Vector3 endLocation;
    }

    [SerializeField]
    float offset;

    List<GenerationLeaf> paths;

    public void Start()
    {
        paths = new List<GenerationLeaf>();
        paths.Add(new GenerationLeaf());
        paths[0].faceX = 0;
        paths[0].faceZ = -1;
        paths[0].endLocation = Vector3.zero;
    }
    public void GeneratePaths()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            AddSinglePath(paths[i]);
        }
    }
    void AddSinglePath(GenerationLeaf info)
    {
        //Debug.Log("Z: " + info.faceZ + " X:" + info.faceX);
        GameObject pathToPlace;
        int newFaceX = 0;
        int newFaceZ = 0;
        float rotationToAdd = 0;
        int randomNumber = Random.Range(0, 3);
        if (randomNumber == 0) //straight
        {
            pathToPlace =  Instantiate(pathStraight, transform);

            if (info.faceZ == -1 || info.faceZ == 1)
            {
                newFaceZ = info.faceZ;
                newFaceX = 0;
            }
            else
            {
                rotationToAdd = 90;
                newFaceX = info.faceX;
                newFaceZ = 0;
            }
        }
        else if (randomNumber == 1)//turn
        {
            pathToPlace = Instantiate(pathTurn, transform);

            if (info.faceZ == 1)
            {
                newFaceX = -1;
                newFaceZ = 0;
            }
            else if (info.faceX == 1)
            {
                newFaceX = 0;
                newFaceZ = -1;
            }
            else if (info.faceX == -1)
            {
                rotationToAdd = -90;
                newFaceZ = -1;
                newFaceX = 0;
            }
            else
            {
                rotationToAdd = 90;
                newFaceZ = 0;
                newFaceX = -1;
            }
        }
        else //split
        {
            pathToPlace = Instantiate(pathSplit, transform);
            if (info.faceZ == 1)
            {
                rotationToAdd = 180;
                newFaceX = 1;
                newFaceZ = 0;
            }
            else if (info.faceX == 1)
            {
                rotationToAdd = -90;
                newFaceX = 0;
                newFaceZ = 1;
            }
            else if (info.faceX == -1)
            {
                rotationToAdd = 90;
                newFaceZ = 1;
                newFaceX = 0;
            }
            else
            {
                newFaceZ = 0;
                newFaceX = -1;
            }
        }
        Vector3 placeLocation = new Vector3((-info.faceX * offset) + info.endLocation.x, info.endLocation.y, (-info.faceZ * offset) + info.endLocation.z);
        info.endLocation = placeLocation;
        pathToPlace.transform.Rotate(0, rotationToAdd, 0);
        pathToPlace.transform.position = placeLocation;
        //Instantiate(pathToPlace, placeLocation, pathToPlace.transform.rotation, transform);
        info.faceX = newFaceX; 
        info.faceZ = newFaceZ;
    }
}
