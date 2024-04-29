using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlaneClick : MonoBehaviour, IPointerClickHandler
{
    private TowerManager _towerManager;
    private void Awake()
    {
        _towerManager = FindFirstObjectByType<TowerManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _towerManager.PlaceTower(gameObject);
    }
}
