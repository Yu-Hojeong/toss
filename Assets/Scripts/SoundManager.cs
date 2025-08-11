using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource BgmAudio;
    [SerializeField] AudioSource EventAudio;
    [SerializeField] AudioClip[] audioClips;

    private Button bgmMuteButton;
    private TMP_Text bgmButtonText;
    private Toggle eventMute;

    private static SoundManager instance;
    private bool isBgmMuted = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        BgmSound("BGM");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 바뀌면 UI 업데이트
        UpdateBgmButtonUI();
    }

    // 씬의 버튼이 자기 자신을 등록하는 함수
    public void RegisterBgmButton(Button btn)
    {
        bgmMuteButton = btn;
        bgmButtonText = bgmMuteButton.GetComponentInChildren<TMP_Text>();

        bgmMuteButton.onClick.RemoveAllListeners();
        bgmMuteButton.onClick.AddListener(ToggleBgmMute);

        UpdateBgmButtonUI();
    }

    // 씬의 토글이 자기 자신을 등록하는 함수
    public void RegisterEventMuteToggle(Toggle toggle)
    {
        eventMute = toggle;

        eventMute.onValueChanged.RemoveAllListeners();
        eventMute.onValueChanged.AddListener(OnEventMute);

        eventMute.isOn = EventAudio.mute;
    }

    public void BgmSound(string clipName)
    {
        if (BgmAudio.isPlaying) return;

        foreach (var clip in audioClips)
        {
            if (clip.name == clipName)
            {
                BgmAudio.clip = clip;
                BgmAudio.Play();
                return;
            }
        }
    }

    public void EventSound(string clipName)
    {
        foreach (var clip in audioClips)
        {
            if (clip.name == clipName)
            {
                EventAudio.PlayOneShot(clip);
                return;
            }
        }
    }

    void ToggleBgmMute()
    {
        isBgmMuted = !isBgmMuted;
        BgmAudio.mute = isBgmMuted;
        UpdateBgmButtonUI();
    }

    void UpdateBgmButtonUI()
    {
        if (bgmButtonText != null)
            bgmButtonText.text = isBgmMuted ? "배경음악 꺼짐" : "배경음악 켜짐";
    }

    void OnEventMute(bool isMute)
    {
        EventAudio.mute = isMute;
    }
}



// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// public class SoundManager : MonoBehaviour
// {
//     [SerializeField] AudioSource BgmAudio;
//     [SerializeField] AudioSource EventAudio;
//     [SerializeField] AudioClip[] audioClips;

//     [SerializeField] Button bgmMuteButton;
//     [SerializeField] Toggle eventMute;
//     private static SoundManager instance;
//     private bool isBgmMuted = false;


//     void Awake()
//     {
//         // 이미 인스턴스가 있으면 자기 자신 삭제 (중복 방지)
//         if (instance != null && instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         // 처음 생성된 SoundManager면 유지
//         instance = this;
//         DontDestroyOnLoad(gameObject);
//         SceneManager.sceneLoaded += OnSceneLoaded;

//         if (bgmMuteButton != null)
//             UpdateBgmButtonUI();

//         if (eventMute != null)
//         eventMute.isOn = EventAudio.mute;
//     }

//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         // 새 씬에서 UI 오브젝트 다시 찾아서 연결
//         if (bgmMuteButton == null)
//             bgmMuteButton = GameObject.Find("BgmMuteButton")?.GetComponent<Button>();

//         if (bgmButtonText == null && bgmMuteButton != null)
//             bgmButtonText = bgmMuteButton.GetComponentInChildren<TMP_Text>();

//         if (bgmMuteButton != null)
//         {
//             bgmMuteButton.onClick.RemoveAllListeners(); // 중복 방지
//             bgmMuteButton.onClick.AddListener(ToggleBgmMute);
//             UpdateBgmButtonUI();
//         }

//         if (eventMute == null)
//             eventMute = GameObject.Find("EventMuteToggle")?.GetComponent<Toggle>();

//         if (eventMute != null)
//         {
//             eventMute.isOn = EventAudio.mute;
//             eventMute.onValueChanged.RemoveAllListeners();
//             eventMute.onValueChanged.AddListener(OnEventMute);
//         }
//     }

//     void Start()
//     {
//         BgmSound("BGM");
//         if (bgmMuteButton != null)
//         bgmMuteButton.onClick.AddListener(ToggleBgmMute);
//         if (eventMute != null)
//         eventMute.onValueChanged.AddListener(OnEventMute);
//     }

//     public void BgmSound(string clipName)
//     {
//         if (BgmAudio.isPlaying)
//             return;
//         foreach (var clip in audioClips)
//         {
//             if (clip.name == clipName)
//             {
//                 BgmAudio.clip = clip;
//                 BgmAudio.Play();
//             }
//         }

//     }

//     public void EventSound(string clipName) // 다른 스크립트에서 불러옴
//     {
//         foreach (var clip in audioClips)
//         {
//             if (clip.name == clipName)
//             {

//                 EventAudio.PlayOneShot(clip);
//             }
//         }
//     }


//     void ToggleBgmMute() // 🔹 버튼 클릭 시 호출
//     {
//         isBgmMuted = !isBgmMuted;
//         BgmAudio.mute = isBgmMuted;
//         UpdateBgmButtonUI();
//     }

//     void UpdateBgmButtonUI() // 🔹 버튼에 텍스트 표시 (선택사항)
//     {
//         TMP_Text btnText = bgmMuteButton.GetComponentInChildren<TMP_Text>();
//         if (btnText != null)
//             btnText.text = isBgmMuted ? "BGM is Off" : "BGM is On";
//     }

//     void OnEventMute(bool isMute)
//     {
//         EventAudio.mute = isMute;
//     }
// }
