using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
public class MenuCommands : MonoBehaviour
{
    [SerializeField] List<GameObject> contextMenus = new List<GameObject>();
    [SerializeField] List<Button> contextButtons = new List<Button>();
    [SerializeField] GameObject contextCloser;
    [SerializeField] List<KeyCode> shortcuts = new List<KeyCode>();
    [SerializeField] ConstraintsManager constraintsManager;
    [SerializeField] GameObject helpPanels;
    [SerializeField] TMP_Text versionText;
    [SerializeField] List<Button> closeOnClickBtn = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var button in closeOnClickBtn)
        {
            button.onClick.AddListener(() => button.transform.parent.gameObject.SetActive(false));
        }
        versionText.text = "v." + Application.version;
        int i = 0;
        foreach (var button in contextButtons)
        {
            var n = i;
            button.onClick.AddListener(() => CloseAllContext());
            button.onClick.AddListener(() => OpenContext(n));
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckShortcut();
    }

    void CheckShortcut()
    {
        if (EventSystem.current.currentSelectedGameObject != null) return;
        int i = 0;
        foreach(var shortcut in shortcuts)
        {
            if (Input.GetKeyDown(shortcut))
            {
                switch (i)
                {
                    case 0:
                        constraintsManager.ToggleGizmos();
                        break;
                    case 1:
                        helpPanels.SetActive(!helpPanels.activeInHierarchy);
                        break;
                }
            }
            i++;
        }
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenContext(int n)
    {
        contextMenus[n].SetActive(true);
        contextCloser.SetActive(true);
    }

    public void CloseAllContext()
    {
        contextCloser.SetActive(false);
        foreach(var menu in contextMenus)
        {
            menu.SetActive(false);
        }
    }

    public void OpenWebsite(string url)
    {
        Application.OpenURL(url);
    }
}
