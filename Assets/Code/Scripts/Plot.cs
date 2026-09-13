using UnityEngine;
using UnityEngine.Rendering;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
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
            ManageEmptyPlot();
        }

    }


    private void ManageExistentTower() {
        Debug.Log("Tower already built.");
        turret.OpenUpgradeUI();
    }


    private void ManageEmptyPlot() {
       Shop.main.OpenShop();
       //BuildTower();
    }


    private void BuildTower(Tower towerToBuild) {
        Debug.Log("Building new tower...");

        if (towerToBuild.cost > LevelManager.main.currency) {
            Debug.Log("You can't afford this tower");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
