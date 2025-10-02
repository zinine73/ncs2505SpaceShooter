using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    // 버튼을 연결할 변수
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    UnityAction action;

    void Start()
    {
        // UnityAction을 사용한 이벤트 연결 방식
        action = () => OnButtonClick(startButton.name);
        startButton.onClick.AddListener(action);

        // 무명메서드를 사용
        optionButton.onClick.AddListener(
            delegate { OnButtonClick(optionButton.name); }
        );

        // 람다식을 사용
        shopButton.onClick.AddListener(
            () => OnButtonClick(shopButton.name));
    }
    
    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg}");
    }
}
