using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private TowerManager _towerManager;
    private void Awake()
    {
        _towerManager = FindFirstObjectByType<TowerManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnPointerUp(PointerEventData eventData)
    {
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _towerManager.PlaceTower(gameObject);
    }
}
