using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour, ITurret {

    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;

    [Header("Upgrade")]
    [SerializeField] private TurretUpgrader upgrader;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 500f;
    // bps is initialised from upgrader in Start and updated on each successful upgrade.
    private float bps;

    private Transform target;
    private float timeUntilFire;

    // Initialises bps from the upgrader's current stat.
    void Start() {
        bps = upgrader.CurrentStat;
    }

    // Update is called once per frame.
    void Update() {
        if (target == null) {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange()) {
            target = null;
        } else {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps) {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    // Instantiates a bullet aimed at the current target.
    private void Shoot() {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    // Finds the nearest enemy within targeting range.
    private void FindTarget() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0) {
            target = hits[0].transform;
        }
    }

    // Returns true when the current target is still within range.
    private bool CheckTargetIsInRange() {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    // Rotates the turret head toward the current target.
    private void RotateTowardsTarget() {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // --- ITurret ---

    // Exposes the targeting range so build preview and PlotSelectionManager can read it.
    public float TargetingRange => targetingRange;
    public int Level => upgrader.Level;
    public int MaxLevel => upgrader.MaxLevel;
    public string StatLabel => upgrader.StatLabel;
    public float CurrentStat => upgrader.CurrentStat;
    public float NextStat => upgrader.NextStat;
    public int NextUpgradeCost => upgrader.NextUpgradeCost;
    public bool CanUpgrade => upgrader.CanUpgrade;

    // Delegates upgrade to the upgrader and applies the new bps on success.
    public bool TryUpgrade() {
        if (upgrader.TryUpgrade(out float newStat)) {
            bps = newStat;
            return true;
        }
        return false;
    }

    // Shows the upgrade panel and immediately refreshes its labels.
    public void OpenUpgradeUI() {
        upgradeUI.SetActive(true);
        upgradeUI.GetComponentInChildren<TurretUpgradeUI>()?.Refresh();
    }

    // Hides the upgrade panel.
    public void CloseUpgradeUI() {
        upgradeUI.SetActive(false);
    }

    private void OnDrawGizmosSelected() {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
