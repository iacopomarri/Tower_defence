using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour {

    public static Shop main;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private Animator anim;

    private const string ShopOpenParameter = "ShopOpen";
    private bool isShopOpen;

    // Registers the shop and resolves references that belong to its panel.
    private void Awake() {
        main = this;

        if (anim == null) {
            anim = GetComponent<Animator>();
            if ((anim == null || anim.runtimeAnimatorController == null) && transform.parent != null) {
                Animator parentAnim = transform.parent.GetComponent<Animator>();
                if (parentAnim != null) {
                    anim = parentAnim;
                }
            }
        }

        if (currencyUI == null) {
            Transform currency = transform.Find("Currency");
            if (currency == null && transform.parent != null) {
                currency = transform.parent.Find("Currency");
            }

            if (currency != null) {
                currencyUI = currency.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    private void Start() {
        CloseShop();
    }

    // Keeps the displayed currency synchronised with the level manager.
    private void Update() {
        UpdateCurrency();
    }

    // Refreshes the currency label when the shop becomes available.
    private void OnEnable() {
        UpdateCurrency();
    }

    // Updates the currency label when the level manager is ready.
    private void UpdateCurrency() {
        if (currencyUI != null && LevelManager.main != null) {
            currencyUI.text = "$ " + LevelManager.main.currency.ToString();
        }
    }

    // Opens the shop panel (backdrop and ignore-timer are now the manager's responsibility).
    public void OpenShop() {
        isShopOpen = true;
        UpdateAnimation();
        UpdateCurrency();
    }

    // Closes the shop panel and resets tower selection state.
    public void CloseShop() {
        isShopOpen = false;
        BuildManager.main.ResetSelectedTower();
        UpdateAnimation();
    }

    // Applies the shop state to its animator.
    private void UpdateAnimation() {
        if (anim != null) {
            anim.SetBool(ShopOpenParameter, isShopOpen);
        }
    }
}
