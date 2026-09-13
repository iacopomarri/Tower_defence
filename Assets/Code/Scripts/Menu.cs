using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu main;
   
    // Registers this object as the main menu controller.
    private void Awake()
    {
        main = this;
    }
}
