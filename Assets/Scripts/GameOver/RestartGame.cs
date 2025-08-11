using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1f; // 혹시 정지된 상태였으면 시간 다시 흐르게
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
