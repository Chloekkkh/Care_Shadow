using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button ruleButton;
    public Button quitButton;
    public Button closeRuleButton;
    
    public GameObject rulePanel;

    private void Awake()
    {
        startButton.onClick.AddListener(LoadScene);
        ruleButton.onClick.AddListener(RuleButton);
        closeRuleButton.onClick.AddListener(CloseRulePanel);
        quitButton.onClick.AddListener(QuitGame);
    }
   public void LoadScene()
    {
        ScenceManager.instance.StartCoroutine(ScenceManager.instance.Fadeout());
    }
    public void RuleButton()
    {
        rulePanel.SetActive(true);
    }
    public void CloseRulePanel()
    {
        rulePanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
