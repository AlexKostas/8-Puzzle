using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    
    private int buttonIndex;
    
    public void SetupButton(int tileNumber, int newButtonIndex) {
        EnableButton();
        text.text = tileNumber.ToString();

        this.buttonIndex = newButtonIndex;
        button.onClick.AddListener(OnClick);
    }

    // Can't just deactivate the game object to maintain grid layout
    public void DisableButton() {
        button.enabled = false;
        text.enabled = false;
        image.enabled = false;
    }

    public void EnableButton() {
        button.enabled = true;
        text.enabled = true;
        image.enabled = true;
    }

    public void ChangeButtonNumber(int newTileNumber) {
        text.text = newTileNumber.ToString();
    }

    private void OnClick() {
        GameManager.instance.OnButtonClicked(buttonIndex);
    }
}