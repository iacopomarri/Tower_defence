using UnityEngine;

public class BuildManager : MonoBehaviour {

    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    private int selectedTower = -1;

    private void Awake() {
        main = this;
    }

    // Returns the currently selected Tower definition, or null if none.
    private Tower GetSelectedTower() {
        if (selectedTower < 0 || selectedTower >= towers.Length) return null;
        return towers[selectedTower];
    }

    // Called by shop buttons; previews on first click, builds on second click of same tower.
    public void SelectTowerToBuild(int _selectedTower) {
        if (_selectedTower < 0 || _selectedTower >= towers.Length) {
            selectedTower = -1;
            Debug.Log("Selected tower is out of range.");
            return;
        }

        if (PlotSelectionManager.main.SelectedPlot == null) {
            Debug.Log("No plot selected.");
            return;
        }

        // First click of the user on the tower to build: show a preview.
        if (selectedTower != _selectedTower) {
            selectedTower = _selectedTower;
            PreviewTowerOnPlot();
        } else {
            // Second click: build the tower.
            BuildTowerOnPlot();
        }
    }

    // Shows a temporary range-disc preview of the selected tower on the currently selected plot.
    private void PreviewTowerOnPlot() {
        Plot plot = PlotSelectionManager.main.SelectedPlot;
        if (plot == null || GetSelectedTower() == null) return;
        plot.PreviewTower(GetSelectedTower());
    }

    // Builds the selected tower on the currently selected plot and deselects on success.
    private void BuildTowerOnPlot() {
        Plot plot = PlotSelectionManager.main.SelectedPlot;
        if (plot == null) return;
        if (plot.BuildTower(GetSelectedTower())) {
            PlotSelectionManager.main.Deselect();
        }
    }

    // Clears the range preview and resets selection state (called on shop close or cancel).
    public void ResetSelectedTower() {
        Plot plot = PlotSelectionManager.main?.SelectedPlot;
        if (plot != null) plot.ClearPreview();
        selectedTower = -1;
    }

    void Start() { }
    void Update() { }
}
