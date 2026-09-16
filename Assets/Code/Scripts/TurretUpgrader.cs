using System;
using UnityEngine;

// Serializable composition helper encapsulating upgrade math.
// Reference this from any turret MonoBehaviour so upgrade logic is written once.
[Serializable]
public class TurretUpgrader {

    [SerializeField] private TurretUpgradeData data;

    // Current upgrade level; starts at 1.
    public int Level { get; private set; } = 1;

    public int MaxLevel => data != null ? data.MaxLevel : 1;
    public bool CanUpgrade => Level < MaxLevel;
    public string StatLabel => data != null ? data.statLabel : string.Empty;

    // Primary-stat value for the current level.
    public float CurrentStat {
        get {
            if (data == null) return 0f;
            return Level == 1 ? data.baseStat : data.upgrades[Level - 2].statValue;
        }
    }

    // Primary-stat value that would apply after the next upgrade.
    public float NextStat {
        get {
            if (data == null || !CanUpgrade) return CurrentStat;
            return data.upgrades[Level - 1].statValue;
        }
    }

    // Currency cost of the next upgrade.
    public int NextUpgradeCost => (data != null && CanUpgrade) ? data.upgrades[Level - 1].cost : 0;

    // Attempts to spend currency and advance one level.
    // Returns true and outputs the new stat on success; false (no spend) if blocked.
    public bool TryUpgrade(out float newStat) {
        newStat = CurrentStat;
        if (!CanUpgrade) return false;
        if (!LevelManager.main.SpendCurrency(NextUpgradeCost)) return false;
        Level++;
        newStat = CurrentStat;
        return true;
    }
}
