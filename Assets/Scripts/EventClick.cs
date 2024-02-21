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
        Debug.Log("Down");

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        _towerManager.PlaceTower(gameObject);
    }
}
