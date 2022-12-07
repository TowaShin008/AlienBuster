using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button titleButton;
    [SerializeField]SimpleUITransition[] transitions;
    [SerializeField] private GameObject[] nowPanels;
   
    bool pause;
    void Start()
    {
        pausePanel.SetActive(false);
        pauseButton.onClick.AddListener(Pause);
        returnButton.onClick.AddListener(Resume);
        exitButton.onClick.AddListener(GameEnd);
        titleButton.onClick.AddListener(TitleReturn);
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            Pause();       // Time.timeScale = 0;
        }
    }
    private void Pause()
    {
        foreach (var panel in nowPanels)
        {
            panel.SetActive(false);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pausePanel.SetActive(true);

        foreach (var trans in transitions)
        {
            trans.Show();
        }
    }
    private void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausePanel.SetActive(false);
        foreach (var panel in nowPanels)
        {
            panel.SetActive(true);
        }
    }
    private void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();
#endif
    }
    private void TitleReturn()
    {
        FadeManager.Instance.LoadScene("TitleScene", 1f);
    }
}
