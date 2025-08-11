using UnityEngine;
using UnityEngine.UI;

public class UIAutoRegister : MonoBehaviour
{
    public enum UIType { BgmButton, EventMuteToggle }
    public UIType uiType;

    void Start()
    {
        var sm = FindFirstObjectByType<SoundManager>();
        if (sm == null) return;

        if (uiType == UIType.BgmButton)
        {
            var btn = GetComponent<Button>();
            if (btn != null) sm.RegisterBgmButton(btn);
        }
        else if (uiType == UIType.EventMuteToggle)
        {
            var toggle = GetComponent<Toggle>();
            if (toggle != null) sm.RegisterEventMuteToggle(toggle);
        }
    }
}
