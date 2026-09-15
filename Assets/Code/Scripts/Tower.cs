using System;
using UnityEngine;

[Serializable]
public class Tower {

    public string name;
    public int cost;
    public GameObject prefab;

    public Tower(string _name, int _cost, GameObject _prefab) {
        name = _name;
        cost = _cost;
        prefab = _prefab;    
    }

    // Returns the targeting range from the prefab's ITurret component.
    // Any turret type just needs to implement ITurret — no changes needed here.
    public float GetRange() {
        if (prefab == null) {
            Debug.LogError($"[Tower.GetRange] Tower '{name}' has no prefab assigned. Assign a valid prefab in the Inspector.");
            return -1f;
        }
        ITurret turret = prefab.GetComponent<ITurret>();
        if (turret != null) return turret.TargetingRange;
        Debug.LogError($"[Tower.GetRange] Prefab '{prefab.name}' has no component implementing ITurret. Make the turret script implement ITurret.");
        return -1f;
    }
}
