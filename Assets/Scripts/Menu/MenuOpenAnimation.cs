using UnityEngine;

public class MenuOpenAnimation : MonoBehaviour
{
    public RectTransform menuTransform;

    private void OnEnable()
    {
        Time.timeScale = 0f;
        menuTransform.localScale = Vector3.zero;
        StartCoroutine(OpenAnimation());
    }

    private System.Collections.IEnumerator OpenAnimation()
    {
        float time = 0f;
        float duration = 0.5f; // 좀 더 부드럽게 살짝 길게 설정
        Vector3 start = Vector3.zero;
        Vector3 overshoot = Vector3.one * 1.2f; // 1.2배까지 커졌다가
        Vector3 undershoot = Vector3.one * 0.95f; // 0.95배까지 작아졌다가
        Vector3 end = Vector3.one;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;

            // 튕기는 이징 함수 (approximate EaseOutBack with bounce)
            if (t < 0.6f)
            {
                // 0 ~ 0.6 구간: 0 -> overshoot (1.2배)
                float t1 = t / 0.6f;
                float scaleValue = Mathf.Lerp(0f, 1.2f, Mathf.Sin(t1 * Mathf.PI * 0.5f));
                menuTransform.localScale = Vector3.one * scaleValue;
            }
            else if (t < 0.8f)
            {
                // 0.6 ~ 0.8 구간: overshoot -> undershoot (1.2 -> 0.95)
                float t2 = (t - 0.6f) / 0.2f;
                float scaleValue = Mathf.Lerp(1.2f, 0.95f, t2);
                menuTransform.localScale = Vector3.one * scaleValue;
            }
            else
            {
                // 0.8 ~ 1 구간: undershoot -> end (0.95 -> 1)
                float t3 = (t - 0.8f) / 0.2f;
                float scaleValue = Mathf.Lerp(0.95f, 1f, t3);
                menuTransform.localScale = Vector3.one * scaleValue;
            }

            yield return null;
        }

        menuTransform.localScale = end;
    }

private void OnDisable()
{
    Time.timeScale = 1f;
}

}
