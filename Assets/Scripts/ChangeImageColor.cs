using UnityEngine.UI;
using UnityEngine;

public class ChangeImageColor : MonoBehaviour
{

    // Inspector에서 직접 설정할 수 있는 파스텔 색상 배열
    [Tooltip("게임 시작 시 랜덤하게 선택될 파스텔 배경 색상들입니다.")]

    public Image background;
    public Color[] pastelColors;

    void Start()
    {
        // 씬의 메인 카메라를 찾아 배경색을 변경합니다.
        

        // pastelColors 배열에 색상이 하나라도 있는지 확인
        if (pastelColors == null || pastelColors.Length == 0)
        {
            Debug.LogWarning("RandomPastelBackground: pastelColors 배열이 비어 있습니다. 기본 파스텔 색상으로 초기화합니다.");
            // 배열이 비어있으면 몇 가지 기본 파스텔 색상으로 초기화 (선택 사항)
            pastelColors = new Color[]
            {
                new Color(0.7f, 0.8f, 1.0f),  // 연한 하늘색
                new Color(1.0f, 0.7f, 0.8f),  // 연한 분홍색
                new Color(0.8f, 1.0f, 0.7f),  // 연한 연두색
                new Color(1.0f, 0.9f, 0.7f),  // 연한 살구색
                new Color(0.7f, 0.9f, 1.0f)   // 연한 청록색
            };
        }

        // 배열에서 랜덤한 인덱스 선택
        int randomIndex = Random.Range(0, pastelColors.Length);

        // 선택된 색상으로 카메라 배경색 변경
        background.color = pastelColors[randomIndex];

        Debug.Log($"Background color set to: {pastelColors[randomIndex]} (Index: {randomIndex})");
    }
}
