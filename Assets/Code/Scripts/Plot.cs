using UnityEngine;

public class Plot : MonoBehaviour {

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color selectedColor;

    private GameObject towerObj;
    private GameObject previewObj;
    private ITurret turret;
    private Color startColor;
    private bool isSelected;

    void Start() {
        startColor = sr.color;
    }

    // Tints on hover unless the plot is already selected.
    private void OnMouseEnter() {
        if (isSelected) return;
        sr.color = hoverColor;
    }

    // Restores tint on hover-exit unless the plot is selected.
    private void OnMouseExit() {
        if (isSelected) return;
        sr.color = startColor;
    }

    // Routes click to the PlotSelectionManager (single source of truth for selection).
    private void OnMouseDown() {
        PlotSelectionManager.main.SelectPlot(this);
    }

    // Applies or removes the selected-color tint.
    public void SetSelected(bool selected) {
        isSelected = selected;
        sr.color = selected ? selectedColor : startColor;
    }

    // True when a tower has been built on this plot.
    public bool HasTurret => towerObj != null;

    // The ITurret component of the built tower (null if no tower).
    public ITurret Turret => turret;

    // Spawns a temporary range-disc preview for a tower about to be built.
    public void PreviewTower(Tower towerToBuild) {
        ClearPreview();
        previewObj = RangePreview.Create(transform.position, towerToBuild.GetRange());
    }

    // Shows thet's range disc while the upgrade UI is open.
    public void PreviewTurretRange() {
        ClearPreview();
        if (turret != null) {
            previewObj = RangePreview.Create(transform.position, turret.TargetingRange);
        }
    }

    // Destroys the current range-disc preview if one exists.
    public void ClearPreview() {
        if (previewObj != null) {
            Destroy(previewObj);
            previewObj = null;
        }
    }

    // Instantiates the chosen tower on this plot if the player can afford it.
    public bool BuildTower(Tower towerToBuild) {
        ClearPreview();
        Debug.Log("Building new tower...");

        if (towerToBuild == null) {
            Debug.Log("No tower selected.");
            return false;
        }

        if (towerObj != null) {
            Debug.Log("Tower already built.");
            return false;
        }

        if (towerToBuild.cost > LevelManager.main.currency) {
            Debug.Log("You can't afford this tower");
            return false;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        // Resolves ITurret regardless of whether the prefab uses Turret or TurretSlowmo.
        turret = towerObj.GetComponent<ITurret>();
        return true;
    }

    void Update() { }
}
