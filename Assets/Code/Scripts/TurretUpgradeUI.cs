using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Reusable panel component placed on each turret's upgrade UI GameObject.
// Populates labels from the owning ITurret and delegates the upgrade action to it.
public class TurretUpgradeUI : MonoBehaviour {

    [Header("References")]
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI statLabel;
    [SerializeField] private TextMeshProUGUI costLabel;
    [SerializeField] private Button upgradeButton;

    private ITurret turret;

    // Finds the owning ITurret anywhere in the parent hierarchy.
    private void Awake() {
        turret = GetComponentInParent<ITurret>();
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
