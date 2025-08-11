using UnityEngine;
using System.Collections;

public class GameOverTrigger : MonoBehaviour
{
    public GameObject gameOverUI;     // Inspector에서 연결
    public float slowDuration = 1f;   // 시간 감속까지 걸리는 시간
    public float delayBeforeUI = 0.5f; // 멈춘 후 UI 등장까지 대기 시간

    private bool isGameOver = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGameOver && collision.CompareTag("Block"))
        {
            isGameOver = true;
            Debug.Log("Game Over");
            StartCoroutine(SlowTimeAndShowUI());
        }
    }

    private IEnumerator SlowTimeAndShowUI()
    {
        float elapsed = 0f;
        float startScale = 1f;
        float endScale = 0f;

        while (elapsed < slowDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / slowDuration;
            Time.timeScale = Mathf.Lerp(startScale, endScale, t);
            yield return null;
        }

        Time.timeScale = 0f;

        // 잠깐 멈춘 후 UI 활성화
        yield return new WaitForSecondsRealtime(delayBeforeUI);
        gameOverUI.SetActive(true);
    }
}
