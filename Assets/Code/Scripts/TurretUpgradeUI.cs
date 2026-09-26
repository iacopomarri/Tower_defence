using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Singleton upgrade panel that lives in the scene (like Shop).
// Any turret calls Open(this) to bind itself and show the panel at its fixed screen position.
public class TurretUpgradeUI : MonoBehaviour {

    public static TurretUpgradeUI main;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI statLabel;
    [SerializeField] private TextMeshProUGUI costLabel;
    [SerializeField] private Button upgradeButton;

    private ITurret turret;

    // Registers the singleton and starts hidden.
    private void Awake() {
        main = this;
        gameObject.SetActive(false);
    }

    // Binds to the given turret, shows the panel, and refreshes all labels.
    public void Open(ITurret turretToShow) {
        Debug.Log("TurretUpgradeUI.Open called");
        turret = turretToShow;
        gameObject.SetActive(true);
        Refresh();
    }

    // Hides the panel and clears the turret reference.
    public void Close() {
        Debug.Log("TurretUpgradeUI.Close called");
        gameObject.SetActive(false);
        turret = null;
    }

    // Refreshes all labels and button interactability from the current turret state.
    public void Refresh() {
        if (turret == null) return;

        levelLabel.text = $"Level {turret.Level} / {turret.MaxLevel}";

        if (turret.CanUpgrade) {
            statLabel.text = $"{turret.StatLabel}: {turret.CurrentStat:0.##} → {turret.NextStat:0.##}";
            costLabel.text = $"$ {turret.NextUpgradeCost}";
        } else {
            statLabel.text = $"{turret.StatLabel}: {turret.CurrentStat:0.##} (MAX)";
            costLabel.text = string.Empty;
        }

        bool affordable = LevelManager.main != null && LevelManager.main.currency >= turret.NextUpgradeCost;
        upgradeButton.interactable = turret.CanUpgrade && affordable;
    }

    // Bound to the Upgrade button's onClick in the Inspector.
    public void OnUpgradeClicked() {
        if (turret != null && turret.TryUpgrade()) {
            Refresh();
        }
    }
}
