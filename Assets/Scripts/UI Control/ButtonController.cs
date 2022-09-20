using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;

    public void SetupButton(int tileNumber) {
        EnableButton();
        text.text = tileNumber.ToString();
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
}
