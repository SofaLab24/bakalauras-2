using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCombinationController : MonoBehaviour, IPointerClickHandler
{
    public string towerType;

    TowerManager manager;

    private void Awake()
    {
        manager = FindFirstObjectByType<TowerManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on tower");
        manager.CombineTowers(this.gameObject, towerType);
    }
    public void DestroyCurrentTower()
    {
        Debug.Log("Add visual sparkles");

        // wait for sparkles to execute
        Destroy(gameObject);
    }
}
