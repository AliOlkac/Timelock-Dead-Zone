using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    public Button returnButton;

    private void Awake()
    {
        returnButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
}
