using System.Collections;
using UnityEditor;
using UnityEngine;

public class TurretSlowmo : MonoBehaviour, ITurret {

    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;

    [Header("Upgrade")]
    [SerializeField] private TurretUpgrader upgrader;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    // aps is initialised from upgrader in Start and updated on each successful upgrade.
    private float aps;
    [SerializeField] private float freezeTime = 1f;

    private float timeUntilFire;

    // Initialises aps from the upgrader's current stat.
    void Start() {
        aps = upgrader.CurrentStat;
    }

    // Update is called once per frame.
    void Update() {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps) {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    // Slows all enemies within range.
    private void FreezeEnemies() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0) {
            for (int i = 0; i < hits.Length; i++) {
                EnemyMovement em = hits[i].transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);
                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    // Restores enemy speed after freezeTime seconds.
    private IEnumerator ResetEnemySpeed(EnemyMovement em) {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
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

    // Delegates upgrade to the upgrader and applies the new aps on success.
    public bool TryUpgrade() {
        if (upgrader.TryUpgrade(out float newStat)) {
            aps = newStat;
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
