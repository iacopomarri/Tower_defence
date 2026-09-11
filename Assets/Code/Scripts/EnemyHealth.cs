using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    private bool isDestroyed = false;  //prevents multiple parallel TakeDamage() invokation from destroying the same enemy many times
    private int maxHitPoints;
    private Transform healthBarFill;
    private SpriteRenderer healthBarRenderer;

    private const float HealthBarWidth = 0.8f;
    private const float HealthBarHeight = 0.05f;
    private const float HealthBarVerticalOffset = 0.65f;
    private static Sprite healthBarSprite;

    private void Awake()
    {
        maxHitPoints = Mathf.Max(1, hitPoints);
        CreateHealthBar();
        UpdateHealthBar();
    }


    private void CreateHealthBar()
    {
        GameObject healthBar = new GameObject("Health Bar");
        healthBar.transform.SetParent(transform, false);
        healthBar.transform.localPosition = new Vector3(0f, HealthBarVerticalOffset, -0.01f);

        healthBarFill = new GameObject("Fill").transform;
        healthBarFill.SetParent(healthBar.transform, false);

        healthBarRenderer = healthBarFill.gameObject.AddComponent<SpriteRenderer>();
        healthBarRenderer.sprite = GetHealthBarSprite();
        healthBarRenderer.color = Color.yellow;

        SpriteRenderer enemyRenderer = GetComponent<SpriteRenderer>();
        healthBarRenderer.sortingLayerID = enemyRenderer != null
            ? enemyRenderer.sortingLayerID
            : healthBarRenderer.sortingLayerID;
        healthBarRenderer.sortingOrder = enemyRenderer != null
            ? enemyRenderer.sortingOrder + 1
            : 1;
    }

    private static Sprite GetHealthBarSprite()
    {
        if (healthBarSprite == null)
        {
            healthBarSprite = Sprite.Create(
                Texture2D.whiteTexture,
                new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f),
                1f);
        }

        return healthBarSprite;
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill == null)
        {
            return;
        }

        float healthRatio = Mathf.Clamp01((float)hitPoints / maxHitPoints);
        healthBarFill.localScale = new Vector3(
            HealthBarWidth * healthRatio,
            HealthBarHeight,
            1f);
        healthBarFill.localPosition = new Vector3(
            HealthBarWidth * (healthRatio - 1f) * 0.5f,
            0f,
            0f);
    }

    public void TakeDamage(int dmg) 
    {
        hitPoints -= dmg;
        UpdateHealthBar();

        if (hitPoints <= 0 && !isDestroyed) {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
