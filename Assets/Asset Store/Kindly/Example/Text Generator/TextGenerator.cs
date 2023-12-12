using UnityEngine;
using UnityEngine.UI;
using MADD;

public class TextGenerator : MonoBehaviour
{
    public InputField _if;
    public Button _generateBtn;
    public Text _outputText;

    private void OnEnable()
    {
        Kindly.Instance.OnTextReceived += UpdateText;
    }

    private void OnDisable()
    {
        if (!Kindly.Quitting)
            Kindly.Instance.OnTextReceived -= UpdateText;
    }

    public void GenerateText()
    {
        string prompt = _if.text;
        Kindly.Instance.GenerateText(prompt);
        _generateBtn.interactable = false;
    }

    private void UpdateText()
    {
        _outputText.text = Kindly.Instance._generatedText;
        _generateBtn.interactable = true;
    }
}
