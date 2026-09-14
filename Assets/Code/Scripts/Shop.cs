using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop main;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject backdrop;

    private const string ShopOpenParameter = "ShopOpen";
    private bool isShopOpen;
    private float ignoreBackdropClicksUntil;

    // Registers the shop and resolves references that belong to its panel.
    private void Awake()
    {
        main = this;

        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if ((anim == null || anim.runtimeAnimatorController == null) && transform.parent != null)
            {
                Animator parentAnim = transform.parent.GetComponent<Animator>();
                if (parentAnim != null)
                {
                    anim = parentAnim;
                }
            }
        }

        if (currencyUI == null)
        {
            Transform currency = transform.Find("Currency");
            if (currency == null && transform.parent != null)
            {
                currency = transform.parent.Find("Currency");
            }

            if (currency != null)
            {
                currencyUI = currency.GetComponent<TextMeshProUGUI>();
            }
        }

        if (backdrop == null)
        {
            Transform searchRoot = transform.parent;
            while (searchRoot != null)
            {
                Transform backdropTransform = searchRoot.Find("Shop Backdrop");
                if (backdropTransform != null)
                {
                    backdrop = backdropTransform.gameObject;
                    break;
                }

                searchRoot = searchRoot.parent;
            }
        }

    }

    private void Start() {
        CloseShop();
    }

    // Keeps the displayed currency synchronized with the level manager.
    private void Update()
    {
        UpdateCurrency();
    }

    // Refreshes the currency label when the shop becomes available.
    private void OnEnable()
    {
        UpdateCurrency();
    }

    // Updates the currency label when the level manager is ready.
    private void UpdateCurrency()
    {
        if (currencyUI != null && LevelManager.main != null)
        {
            currencyUI.text = "$ " + LevelManager.main.currency.ToString();
        }
    }

    // Opens the shop panel.
    public void OpenShop()
    {
        isShopOpen = true;
        ignoreBackdropClicksUntil = Time.unscaledTime + 0.2f;
        SetBackdropActive(true);
        UpdateAnimation();
        UpdateCurrency();
    }

    // Closes the shop panel.
    public void CloseShop(bool force = false)
    {
        if (!force && Time.unscaledTime < ignoreBackdropClicksUntil)
        {
            return;
        }

        isShopOpen = false;
        BuildManager.main.ResetSelectedTower();
        SetBackdropActive(false);
        UpdateAnimation();
    }

    public void BackdropClick()
    {
        CloseShop();
    }

    // Activates or deactivates the click-capturing backdrop.
    private void SetBackdropActive(bool isActive)
    {
        if (backdrop != null)
        {
            backdrop.SetActive(isActive);
        }
    }

    // Applies the shop state to its animator.
    private void UpdateAnimation()
    {
        if (anim != null)
        {
            anim.SetBool(ShopOpenParameter, isShopOpen);
        }
    }
}
