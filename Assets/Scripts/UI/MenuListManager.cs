
/*
    Manages a stack/list of menus for UI navigation, including handling back actions and main menu reference.
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuListManager : MonoBehaviour
{
   [SerializeField] private InputActionReference backMenuAction;
   public List<GameObject> menuList = new List<GameObject>();
   public GameObject mainMenu;


    private void OnEnable()
    {
        // Do not enable backMenuAction here; control it via menu logic
        backMenuAction.action.performed += HandleBackMenu;
    }

    private void OnDisable()
    {
        backMenuAction.action.performed -= HandleBackMenu;
        backMenuAction.action.Disable();
    }

    public void EnableBackMenuAction()
    {
        backMenuAction.action.Enable();
    }

    public void DisableBackMenuAction()
    {
        backMenuAction.action.Disable();
    }

    private void Awake()
    {
        menuList.Add(mainMenu); // Add the main menu to the list at the start
    }

    private void HandleBackMenu(InputAction.CallbackContext context)
    {

        Debug.Log("Back menu action performed"); // Log a message when the back menu action is performed

        GoBackToPreviousMenu(); // Call the method to go back to the previous menu
    }

   public void AddMenuToList(GameObject menu)
   {
       menu.SetActive(true); // Activate the menu GameObject to show it
       menuList.Add(menu); // Add the menu to the list
       EnableBackMenuAction(); // Only enable back action when a menu is open
   }

   public void GoBackToPreviousMenu()
   {
       if (menuList.Count > 1)
       {
           GameObject currentMenu = menuList[menuList.Count - 1]; // Get the last menu in the list
           currentMenu.SetActive(false); // Deactivate the current menu to hide it
           menuList.RemoveAt(menuList.Count - 1); // Remove the current menu from the list
           if (menuList.Count <= 1)
           {
               DisableBackMenuAction(); // Disable back action if no menus left
           }
       }
   }
}
