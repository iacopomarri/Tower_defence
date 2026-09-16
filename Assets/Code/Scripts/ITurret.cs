// Common contract for all turret types.
// Implement on every turret MonoBehaviour to keep Plot, PlotSelectionManager, and TurretUpgradeUI type-agnostic.
public interface ITurret {
    float TargetingRange { get; }
    int Level { get; }
    int MaxLevel { get; }
    string StatLabel { get; }
    float CurrentStat { get; }
    float NextStat { get; }
    int NextUpgradeCost { get; }
    bool CanUpgrade { get; }
    bool TryUpgrade();
    void OpenUpgradeUI();
    void CloseUpgradeUI();
}
