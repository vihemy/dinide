using UnityEngine;
using UnityEngine.UI;
using MADD;

public class ImageGenerator : MonoBehaviour
{
    public InputField _if;
    public Button _generateBtn;
    public Model _selectedModel;
    public RawImage _modelPreview;

    private void OnEnable()
    {
        Kindly.Instance.OnImagesReceived += EnableBtn;
    }

    private void OnDisable()
    {
        if (!Kindly.Quitting)
            Kindly.Instance.OnImagesReceived -= EnableBtn;
    }

    public void GenerateStableDiffusion()
    {
        if (_selectedModel == null)
        {
            Debug.LogWarning("You gotta select a model first!");
            return;
        }
        string prompt = _if.text;
        Kindly.Instance.GenerateImage(prompt, "", _selectedModel.id);
        _generateBtn.interactable = false;
    }

    private void EnableBtn()
    {
        _generateBtn.interactable = true;
    }

    private void Start()
    {
        if (_selectedModel == null)
            _generateBtn.interactable = false;
    }

    public void SetModel(Model model, Texture tex)
    {
        _selectedModel = model;
        _modelPreview.texture = tex;
    }
}
