using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    public static Menu main;
   
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = false;

    private void OnGUI() {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    public void SetSelected() {

    }
    
     private void Awake() {
        main = this;
    }   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }
}
