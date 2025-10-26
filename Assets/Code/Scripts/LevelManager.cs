using UnityEngine;

public class LevelManager : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    public int currency;

    private void Awake() {
        main = this;
    }
    void Start() {
        currency = 100;
    }

    // Update is called once per frame
    void Update() {

    }

    public void IncreaseCurrency(int amount) {
        currency += amount;
    }

    public bool SpendCurrency(int amount) {
        if (amount <= currency) {
            currency -= amount;
            return true;
        }
        else {
            Debug.Log("Not enough money");
            return false;
        }
    }
}
