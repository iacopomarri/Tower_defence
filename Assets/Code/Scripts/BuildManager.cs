using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    private int selectedTower = -1;
    private Plot selectedPlot;

    private void Awake() {
        main = this;
    }

    private Tower GetSelectedTower() {
        if (selectedTower < 0 || selectedTower >= towers.Length)
            return null;

        return towers[selectedTower];
    }

    // Stores the plot the player clicked so a shop purchase can build on it.
    public void SetSelectedPlot(Plot plot) {
        selectedPlot = plot;
    }

    public void SelectTowerToBuild(int _selectedTower) {
        if (_selectedTower < 0 || _selectedTower >= towers.Length) {
            selectedTower = -1;
            Debug.Log("Selected tower is out of range.");
            return;
        }

        if (selectedPlot == null) {
            Debug.Log("No plot selected.");
            return;
        }
    
        // First click of the user on the tower to buld. Show a preview of the tower on the field.
        if (selectedTower != _selectedTower) {
            selectedTower = _selectedTower;
            PreviewTowerOnPlot();

        } else {
            // Second click of the user on the tower to build. Build the tower on the field.
            BuildTowerOnPlot();
        }     
    }

    // Shows a temporary range-disc preview of the selected tower on the currently selected plot.
    private void PreviewTowerOnPlot() {
        if (selectedPlot == null || GetSelectedTower() == null) return;
        selectedPlot.PreviewTower(GetSelectedTower());
    }

    private void BuildTowerOnPlot() {
          if (selectedPlot.BuildTower(GetSelectedTower())) {
            Shop.main.CloseShop(true);
          }
    }


    // Clears the range preview and resets selection state (called on shop close or cancel).
    public void ResetSelectedTower() {
        if (selectedPlot != null) selectedPlot.ClearPreview();
        selectedTower = -1;
        selectedPlot = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //test commit
}
