using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public float screenDragSensitivity = 0.1f;

    public float minX = -2f;
    public float maxX = 2f;

    private Vector2 touchStartPosition;
    private bool isTouchingScreen = false;

    private GameObject currentHoldingAnimalGO;
    private FallingAnimal currentHoldingAnimalScript;


    public int score = 0;
    private int bestScore = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    private const string BEST_SCORE_KEY = "Origami_Animal_Stacker_BestScore"; // 나중에 구체적인 이름으로 수정이 필요


    public Camera mainCamera; // 메인 카메라 참조
    public float cameraMoveSpeed = 2f; // 카메라 이동 속도 (조절 가능)
    public float cameraHeightOffset = 3f; // 가장 높은 블록 위로 카메라가 얼마나 더 올라갈지 (조절 가능)
    public float cameraMinY = 0f;
    private float _cachedHighestBlockY = 0f;
    public float cameraActivationHeight = 2f;
    // 떨어진 블록들을 추적하기 위한 리스트
    private List<GameObject> fallenBlocks = new List<GameObject>();

    // 게임 오버 상태를 추적하는 변수
    private bool isGameOver = false;
    public float rotationSpeed = 30f;



    void Start()
    {
        score = 0;
        LoadBestScore();
        UpdateScoreUI();
        UpdateBestScoreUI();
        mainCamera = Camera.main;
        isGameOver = false;
        _cachedHighestBlockY = cameraMinY;

        SpawnAnimalBlock();
    }

    void Update()
    {
        if (isGameOver) return; // 게임 오버 상태면 업데이트 중단

        if (currentHoldingAnimalGO.transform.parent != null)
         {
             // 들고 있는 동안 블록을 회전시킵니다.
             currentHoldingAnimalGO.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
         }

        // 카메라 이동 로직 호출
        MoveCamera();
    }



    public void OnScreenTouchDown(PointerEventData eventData)
    {
        touchStartPosition = eventData.position;
        isTouchingScreen = true;
    }

    public void OnScreenDrag(PointerEventData eventData)
    {
        if (!isTouchingScreen) return;

        Vector2 currentTouchPosition = eventData.position;
        float dragDeltaX = currentTouchPosition.x - touchStartPosition.x;
        float horizontalMoveAmount = dragDeltaX * screenDragSensitivity;

        Vector3 currentHandPosition = transform.position;
        float targetX = currentHandPosition.x + horizontalMoveAmount;
        targetX = Mathf.Clamp(targetX, minX, maxX);
        transform.position = new Vector3(targetX, currentHandPosition.y, currentHandPosition.z);

        touchStartPosition = currentTouchPosition;
    }

    public void OnScreenTouchUp(PointerEventData eventData)
    {
        if (!isTouchingScreen) return;


        isTouchingScreen = false;
        DropAnimalBlock();

    }

    public void SpawnAnimalBlock()
    {
        int r = Random.Range(0, animalPrefabs.Length);
        float randomZRotation = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomZRotation);
        GameObject newAnimal = Instantiate(animalPrefabs[r], transform.position, randomRotation, this.transform);

        
        currentHoldingAnimalGO = newAnimal;
        currentHoldingAnimalScript = newAnimal.GetComponent<FallingAnimal>();
        currentHoldingAnimalScript.SetKinematic(true);

        currentHoldingAnimalScript.SetPlayerController(this); //의존성 주입
        currentHoldingAnimalScript.enabled = false;
    }

    void DropAnimalBlock()
    {
        if (currentHoldingAnimalGO != null && currentHoldingAnimalScript != null)
        {
            currentHoldingAnimalGO.transform.parent = null;
            currentHoldingAnimalScript.SetKinematic(false);
            

        }
    }

    public void OnBlockStopped()
    {
        AddScore(1);
        
        // !!! 최적화: 여기서 최고 높이 블록을 업데이트 합니다 !!!
        if (currentHoldingAnimalGO != null)
        {// 떨어진 블록을 리스트에 추가
            fallenBlocks.Add(currentHoldingAnimalGO);
            // 방금 멈춘 블록의 Y 위치가 현재 캐시된 최고 높이보다 높으면 업데이트
            // (여기서 currentHoldingAnimalGO는 방금 떨어진 그 블록입니다.)
            float newlyDroppedBlockY = currentHoldingAnimalGO.transform.position.y;
            if (newlyDroppedBlockY > _cachedHighestBlockY)
            {
                _cachedHighestBlockY = newlyDroppedBlockY;
                Debug.Log($"New Highest Block Y: {_cachedHighestBlockY}");
            }
        }
        
        currentHoldingAnimalGO = null;
        currentHoldingAnimalScript = null;
        SpawnAnimalBlock();
    }






    // *** 카메라 이동 로직 (새로 추가) ***
    void MoveCamera()
{
    if (mainCamera == null) return;

    // 현재 카메라의 목표 Y 위치를 계산합니다.
    // 이는 _cachedHighestBlockY에 cameraHeightOffset을 더한 값입니다.
    float calculatedTargetY = _cachedHighestBlockY + cameraHeightOffset;

    float finalTargetCameraY;

    // 만약 가장 높은 블록의 Y 위치가 카메라 활성화 높이보다 낮다면,
    // 카메라는 최소 Y 위치 (cameraMinY)에 고정됩니다.
    if (_cachedHighestBlockY < cameraActivationHeight)
    {
        finalTargetCameraY = cameraMinY;
    }
    // 그렇지 않고 가장 높은 블록이 활성화 높이를 넘어섰다면,
    // 계산된 목표 Y 위치를 따르되, cameraMinY보다 낮아지지 않도록 합니다.
    else
    {
        finalTargetCameraY = Mathf.Max(calculatedTargetY, cameraMinY);
    }

    // 현재 카메라 위치
    Vector3 currentCameraPos = mainCamera.transform.position;
    // 최종 목표 카메라 위치
    Vector3 targetCameraPos = new Vector3(currentCameraPos.x, finalTargetCameraY, currentCameraPos.z);

    // 부드럽게 카메라 이동
    mainCamera.transform.position = Vector3.Lerp(currentCameraPos, targetCameraPos, Time.deltaTime * cameraMoveSpeed);
}


    // *** 게임 오버 시 호출될 함수 ***
    public void GameOver()
    {
        if (isGameOver) return; 

        isGameOver = true;
        Debug.Log("Game Over! All gameplay functions should stop now.");

        Time.timeScale = 0f; // 게임을 완전히 멈춤

        // 게임 오버 UI 활성화 (필요하다면)
        // 예시: public GameObject gameOverPanel;
        // if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // 플레이어 컨트롤러 스크립트 비활성화
        this.enabled = false; 

        // 모든 떨어진 블록의 Rigidbody2D를 Kinematic으로 변경하여 물리 시뮬레이션 중단 (선택 사항)
        foreach (GameObject block in fallenBlocks)
        {
            if (block != null)
            {
                FallingAnimal fa = block.GetComponent<FallingAnimal>();
                if (fa != null)
                {
                    fa.SetKinematic(true);
                }
            }
        }
    }


    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (score > bestScore)
        {
            bestScore = score;
            UpdateBestScoreUI();
            SaveBestScore();
        }
    }


    void UpdateScoreUI()
    {
        scoreText.text = "현재 점수: " + score.ToString();
    }

    void UpdateBestScoreUI()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = "최고 점수: " + bestScore.ToString();
        }
        else
        {
            Debug.LogWarning("Best Score Text UI is not assigned in PlayerController!");
        }
    }

    void LoadBestScore()
    {
        // PlayerPrefs.GetInt(KEY, defaultValue)
        // KEY에 해당하는 값이 없으면 defaultValue를 반환
        bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0); 
        Debug.Log($"Loaded Best Score: {bestScore}");
    }

    // 최고 점수를 저장하는 함수 (새로 추가)
    void SaveBestScore()
    {
        PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
        PlayerPrefs.Save(); // 변경 사항을 디스크에 즉시 저장 (필수는 아니지만 안전을 위해 호출)
        Debug.Log($"Saved New Best Score: {bestScore}");
    }

    // 개발/테스트를 위한 최고 점수 초기화 함수 (선택 사항)
    [ContextMenu("Reset Best Score")] // Unity 에디터 컴포넌트에서 우클릭 시 메뉴로 표시
    void ResetBestScore()
    {
        PlayerPrefs.DeleteKey(BEST_SCORE_KEY);
        bestScore = 0;
        UpdateBestScoreUI();
        Debug.Log("Best Score has been reset!");
    }


}