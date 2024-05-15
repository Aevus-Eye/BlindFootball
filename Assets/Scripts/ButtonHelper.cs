using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public void QuitGame() => LevelManager.Instance.QuitGame();
    public void PlayGame() => LevelManager.Instance.PlayGame();
    public void LoadRandomScene() => LevelManager.Instance.LoadNextLevel();
    public void ReturnMenu() => LevelManager.Instance.ReturnMenu();
}
