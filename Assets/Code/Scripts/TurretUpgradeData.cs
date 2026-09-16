using System;
using UnityEngine;

// ScriptableObject holding generic tiered upgrade data for one turret type.
// Create one asset per turret via Assets > Create > TowerDefence > Turret Upgrade Data.
[CreateAssetMenu(menuName = "TowerDefence/Turret Upgrade Data")]
public class TurretUpgradeData : ScriptableObject {

    [Serializable]
    public struct UpgradeTier {
        // Currency cost to reach this tier.
        public int cost;
        // Primary-stat value at this tier.
        public float statValue;
    }

    // Label shown in the upgrade UI (e.g. "BPS", "APS").
    public string statLabel;
    // Stat value at level 1 (before any upgrades).
    public float baseStat;
    // Upgrade tiers for levels 2..N. Fill 2 entries to reach max level 3.
    public UpgradeTier[] upgrades;

    // MaxLevel = base level (1) + number of upgrade tiers.
    public int MaxLevel => upgrades != null ? upgrades.Length + 1 : 1;
}
