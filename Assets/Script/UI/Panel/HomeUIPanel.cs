using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUIPanel : MonoBehaviour
{
    [SerializeField] private Button startButton;
        
    [SerializeField] private string nextScene;

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartClicked);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartClicked);
    }

    private void OnStartClicked()
    {
        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator StartSequenceRoutine()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(nextScene);
    }
}
