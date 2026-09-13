using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    private int selectedTower = -1;

    private void Awake() {
        main = this;
    }

    public Tower GetSelectedTower() {
        if (selectedTower < 0 || selectedTower >= towers.Length)
            return null;

        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower) {
        if (_selectedTower < 0 || _selectedTower >= towers.Length) {
            selectedTower = -1;
            Debug.Log("Selected tower is out of range.");
            return;
        }

        selectedTower = _selectedTower;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //test commit
}
