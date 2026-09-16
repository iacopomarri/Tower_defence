using UnityEngine;

// Singleton coordinator for plot selection, shared backdrop, and panel open/close routing.
// Plot.OnMouseDown delegates here; the manager highlights the plot and opens the correct UI.
public class PlotSelectionManager : MonoBehaviour {

    public static PlotSelectionManager main;

    [SerializeField] private GameObject backdrop;

    // The plot that is currently selected (null when nothing is open).
    public Plot SelectedPlot { get; private set; }

    private float ignoreBackdropUntil;

    private void Awake() {
        main = this;
    }

    // Selects the given plot, closes any previously open panel, and opens the correct new one.
    public void SelectPlot(Plot plot) {
        if (SelectedPlot == plot) return;

        if (SelectedPlot != null) {
            CloseCurrentPanel();
            SelectedPlot.ClearPreview();
            SelectedPlot.SetSelected(false);
        }

        SelectedPlot = plot;
        plot.SetSelected(true);

        if (plot.HasTurret) {
            plot.Turret.OpenUpgradeUI();
            plot.PreviewTurretRange();
        } else {
            Shop.main.OpenShop();
        }

        backdrop.SetActive(true);
        ignoreBackdropUntil = Time.unscaledTime + 0.2f;
    }

    // Closes the current panel, removes highlighting, and hides the backdrop.
    public void Deselect() {
        if (SelectedPlot == null) return;

        CloseCurrentPanel();
        SelectedPlot.ClearPreview();
        SelectedPlot.SetSelected(false);
        backdrop.SetActive(false);
        SelectedPlot = null;
    }

    // Called by the shared backdrop button's onClick.
    public void OnBackdropClick() {
        if (Time.unscaledTime < ignoreBackdropUntil) return;
        Debug.Log("Backdropclicked");

        Deselect();
    }

    // Closes whichever panel is open for the currently selected plot.
    private void CloseCurrentPanel() {
        if (SelectedPlot == null) return;

        if (SelectedPlot.HasTurret) {
            SelectedPlot.Turret.CloseUpgradeUI();
        } //else {
            Shop.main.CloseShop();
        //}
    }
}
