using System.Collections.Generic;
using UnityEngine;
using Statics;
using System;
using Menus;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Managers
{
    public class UIManager : Manager
    {
        public static UIManager Instance;
        [SerializeField] private Transform menuParent;
        private List<Menu> menus = new List<Menu>();
        public bool IsAnyMenuOpen
        {
            get
            {
                if (FindObjectOfType<Menu>()) return true;
                else return false;
            }
        }

        public override void ManagerAwake()
        {
            if (Instance == null) { Instance = this; }

            Menu[] loadedMenus = Resources.LoadAll<Menu>("Menus");

            foreach (Menu menu in loadedMenus)
            {
                CreateAndInitMenu(menu);
            }
        }

        public override void ManagerInitFromLoad(bool newGame)
        {

        }

        public void CloseAllOpenMenus(bool playSound, bool animate)
        {
            foreach (Menu menu in menus)
            {
                menu.CloseWithoutSound(animate);
            }

            if (playSound)
            {
                // Only want to play the close sound once regardless of how many menus are closing
                SFXPlayer.Instance.PlayMenuCloseSound();
            }
        }

        private void CreateAndInitMenu(Menu menu)
        {
            Menu newMenu = Instantiate(menu, menuParent);
            menus.Add(newMenu);
            newMenu.CloseWithoutSound(false);
        }

        public void OpenMenu(string menuName)
        {
            Type menuType = FindMenuType(menuName);

            if (menuType != null && menuType.IsSubclassOf(typeof(Menu)))
            {
                OpenMenu(menuType);
            }
            else
            {
                Debug.LogError($"Error: Could not find menu with name {menuName}");
            }
        }

        public void DisableAllButtons()
        {
            // Call this when loading a new scene to prevent buttons from being pressed again
            Button[] buttons = FindObjectsOfType<Button>();
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }

        private Type FindMenuType(string menuName)
        {
            Type[] types = typeof(Menu).Assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.Name == menuName)
                {
                    string namespaceName = type.Namespace;

                    if (!string.IsNullOrEmpty(namespaceName))
                    {
                        string fullMenuName = $"{namespaceName}.{menuName}";
                        return Type.GetType(fullMenuName);
                    }
                }
            }

            return null;
        }

        private void OpenMenu(Type menuType)
        {
            Menu menuToOpen = menus.Find(menu => menu.GetType() == menuType);
            if (menuToOpen != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                menuToOpen.gameObject.SetActive(true);
                menuToOpen.InitFromOpen();
            }
            else
            {
                Debug.LogError($"Error: Could not find menu of type {menuType.Name}");
            }
        }

        public void OpenMenu<T>(object args = null) where T : Menu
        {
            // To open a menu just call OpenMenu<MenuScriptName>();
            T menuToOpen = menus.Find(menu => menu is T) as T;
            if (menuToOpen != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                menuToOpen.gameObject.SetActive(true);
                menuToOpen.InitFromOpen(args);
            }
            else
            {
                Debug.LogError($"Error: Could not find menu of type {typeof(T).Name}");
            }
        }
    }
}
