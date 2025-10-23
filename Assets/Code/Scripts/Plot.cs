using UnityEngine;
using UnityEngine.Rendering;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
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
        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
