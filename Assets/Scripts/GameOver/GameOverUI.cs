using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameOverUIController : MonoBehaviour
{
    public PlayerController playerController;
    public CanvasGroup gameOverGroup;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public Button retryButton;

    public float fadeDuration = 0.5f;
    public float delayBetween = 0.3f;

    void OnEnable()
    {
        gameOverGroup.alpha = 1f;
        StartCoroutine(ShowGameOverSequence());
    }

    private IEnumerator ShowGameOverSequence()
    {
        // 초기 상태 설정
        SetAlpha(gameOverText, 0f);
        SetAlpha(scoreText, 0f);
        SetAlpha(retryButton.GetComponentInChildren<TextMeshProUGUI>(), 0f);
        retryButton.interactable = false;

        // Game Over 텍스트 등장
        yield return FadeInText(gameOverText);

        // 잠깐 대기
        yield return new WaitForSecondsRealtime(delayBetween);

        // 최고 점수 등장
        scoreText.text = "획득한 점수: " + playerController.score.ToString();
        yield return FadeInText(scoreText);

        // 잠깐 대기
        yield return new WaitForSecondsRealtime(delayBetween);

        // 버튼 텍스트 페이드 인 후 버튼 활성화
        yield return FadeInText(retryButton.GetComponentInChildren<TextMeshProUGUI>());
        retryButton.interactable = true;
    }

    private IEnumerator FadeInText(TMP_Text text)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeDuration;
            SetAlpha(text, t);
            yield return null;
        }
        SetAlpha(text, 1f);
    }

    private void SetAlpha(TMP_Text text, float a)
    {
        Color c = text.color;
        c.a = a;
        text.color = c;
    }
}
