using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MADD;

public class DnDGenerator : MonoBehaviour
{
    #region nested classes

    [System.Serializable]
    public class Details
    {
        public string name;
        public string char_class;
        public string alignment;
    }

    [System.Serializable]
    public class Character
    {
        public string action;
        public string text;
        public string tags;
    }

    [System.Serializable]
    public class Response
    {
        public Character data;
    }

    #endregion

    #region member variables

    public TextAsset _promptSetup;
    public InputField _if;
    public Button _generateBtn;
    public RawImage _characterImage;
    public Text _outputText;

    private string _negativePrompt = "ugly, tiling, poorly drawn hands, poorly drawn feet, poorly drawn face, out of frame, extra limbs, disfigured, deformed, body out of frame, blurry, bad anatomy, blurred, watermark, grainy, signature, cut off, draft";
    private string _description;

    #endregion

    private void OnEnable()
    {
        Kindly.Instance.OnTextReceived += GenerateImage;
        Kindly.Instance.OnImagesReceived += SetupUI;
    }

    private void OnDisable()
    {
        if (!Kindly.Quitting)
        {
            Kindly.Instance.OnTextReceived -= GenerateImage;
            Kindly.Instance.OnImagesReceived -= SetupUI;
        }
    }

    public void Generate()
    {
        string prompt = _if.text;
        Kindly.Instance.GenerateText(_promptSetup + prompt);
        _generateBtn.interactable = false;
    }

    private void GenerateImage()
    {
        // parse the response
        Response res = JsonUtility.FromJson<Response>(Kindly.Instance._generatedText);
        string imagePrompt = res.data.tags;
        _description = res.data.text;

        Kindly.Instance.GenerateImage(imagePrompt, _negativePrompt, 57); // model is DucHaitenAnime
    }

    private void SetupUI()
    {
        // last image is the one we just generated
        Kindly.Instance.DownloadImage(Kindly.Instance._latest.image.url, _characterImage);
        // set the description
        _outputText.text = _description;

        _generateBtn.interactable = true;
    }
}
