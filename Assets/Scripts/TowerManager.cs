using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TowerManager : MonoBehaviour
{
    [Header("Arrow Tower")]
    [SerializeField]
    private GameObject arrowTower;
    [SerializeField]
    private float arrowTowerHeightOffset;
    public bool arrowTowerUnlocked;

    [Header("Flame Tower")]
    public bool flameTowerUnlocked;

    [Header("Wizard Tower")]
    public bool wizardTowerUnlocked;

    public void PlaceTower(GameObject callObject)
    {
        Vector3 location = callObject.transform.position;
        Instantiate(arrowTower, new Vector3(location.x, location.y + arrowTowerHeightOffset, location.z), Quaternion.identity);
        Destroy(callObject);
    }
}
