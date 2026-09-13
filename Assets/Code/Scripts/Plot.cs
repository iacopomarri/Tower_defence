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

    private void OnMouseDown() {
        Debug.Log("Mouse down...");

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
       Menu.main.ToggleMenu();
       //BuildTower();
    }


    private void BuildTower() {
        Debug.Log("Building new tower...");

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

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
