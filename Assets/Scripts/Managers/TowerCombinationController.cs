using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class TowerCombinationController : MonoBehaviour, IPointerClickHandler
{
    public string towerType;

    VisualEffect combinationVfx;
    TowerManager manager;

    bool isDead;
    bool hasCombinationPlayed;

    private void Awake()
    {
        manager = FindFirstObjectByType<TowerManager>();
        combinationVfx = GetComponent<VisualEffect>();
        isDead = false;
        hasCombinationPlayed = false;
    }
    private void Update()
    {
        if (isDead && combinationVfx.aliveParticleCount <= 0 && hasCombinationPlayed)
        {
            Destroy(gameObject);
        }

        if (combinationVfx.aliveParticleCount > 0)
        {
            hasCombinationPlayed = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.CombineTowers(this.gameObject, towerType);
    }
    public void DestroyCurrentTower()
    {
        combinationVfx.Play();
        GetComponent<MeshRenderer>().enabled = false;
        if (TryGetComponent<ArrowTowerController>(out ArrowTowerController arrowController))
        {
            arrowController.enabled = false;
        }
        else if (TryGetComponent<FlameTowerController>(out FlameTowerController flameController))
        {
            flameController.enabled = false;
        }
        else if (TryGetComponent<WizardTowerController>(out WizardTowerController wizardController))
        {
            wizardController.enabled = false;
        }
        isDead = true;
    }
}
