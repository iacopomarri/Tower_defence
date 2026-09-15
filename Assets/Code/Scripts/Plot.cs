using UnityEngine;
using UnityEngine.Rendering;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    private GameObject previewObj;
    public Turret turret;
    private Color startColor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        startColor = sr.color;
    }

    private void OnMouseEnter() {
        sr.color = hoverColor;
    }

    private void OnMouseExit() {
        sr.color = startColor;
    }

    //User clicked on the plot
    private void OnMouseDown() {
        if (towerObj != null) {
            ManageExistentTower();
            return;  
        } 
        else {
            BuildManager.main.SetSelectedPlot(this);
            ManageEmptyPlot();
        }

    }


    private void ManageExistentTower() {
        Debug.Log("Tower already built.");
        turret.OpenUpgradeUI();
    }


    private void ManageEmptyPlot() {
       Shop.main.OpenShop();
    }


    // Spawns a temporary range-disc preview centered on this plot.
    public void PreviewTower(Tower towerToBuild) {
        ClearPreview();
        previewObj = RangePreview.Create(transform.position, towerToBuild.GetRange());
    }

    // Destroys the current range-disc preview if one exists.
    public void ClearPreview() {
        if (previewObj != null) {
            Destroy(previewObj);
            previewObj = null;
        }
    }

    // Instantiates the chosen tower on this plot if the player can afford it.
    public bool BuildTower(Tower towerToBuild) {
        ClearPreview();
        Debug.Log("Building new tower...");

        if (towerToBuild == null) {
            Debug.Log("No tower selected.");
            return false;
        }

        if (towerObj != null) {
            Debug.Log("Tower already built.");
            return false;
        }

        if (towerToBuild.cost > LevelManager.main.currency) {
            Debug.Log("You can't afford this tower");
            return false;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);

        //TODO: here we should check if the tower is a Turret or a TurretSlowmo and assign the correct component to the turret variable.
        turret = towerObj.GetComponent<Turret>();
        return true;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
