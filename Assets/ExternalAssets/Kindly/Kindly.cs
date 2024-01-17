using System;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MADD
{
    #region internal classes

    public class ApiKeyReq
    {
        public string apiKey;
        public int page = 1;
        public int pageSize = 10;
    }

    public class GenerationRequest
    {
        public string prompt;
        public string negative_prompt;
        public int model;
        public string apiKey;
    }

    [System.Serializable]
    public class Image
    {
        public string url;
    }

    [System.Serializable]
    public class Model
    {
        public int id;
        public string name;
        public string description;
        public string image_url;
        public string[] triggers;
        public string[] categories;
    }

    [System.Serializable]
    public class Generation
    {
        public int id;
        public Image image;
        public string prompt;
        public Model sd_model;
    }

    public class TextGenerationRequest
    {
        public string prompt;
        public string apiKey;
    }

    public class TextGenerationResponse
    {
        public string status;
        public string text;
    }

    #endregion

    public class Kindly : Singleton<Kindly>
    {
        #region member variables

        [Header("Settings")]
        public string _apiKey;

        private string _apiUrl = "https://api.kindly.dev";
        public string ApiUrl
        {
            get { return _apiUrl; }
        }

        [Header("Images")]
        public bool _loadModelsOnStartup = false;
        public Model[] _models;
        public Generation[] _generations;
        public Generation _latest;

        public Action OnModelsReceived, OnImagesReceived;

        [Header("GPT")]
        public string _generatedText;
        public Action OnTextReceived;

        #endregion

        #region core methods

        public void GetModels()
        {
            RequestHelper req = new RequestHelper
            {
                Uri = _apiUrl + "/api/models",
            };
            RestClient.GetArray<Model>(req).Then(res =>
            {
                _models = res;
                OnModelsReceived?.Invoke();
            }).Catch(err =>
            {
                Debug.LogWarning(err.Message);
            });
        }

        public void GenerateImage(string prompt, string negativePrompt, int model)
        {
            RequestHelper req = new RequestHelper
            {
                Uri = _apiUrl + "/api/t2i",
                Body = new GenerationRequest
                {
                    prompt = prompt,
                    negative_prompt = negativePrompt,
                    model = model,
                    apiKey = _apiKey
                },
            };
            RestClient.Post<Generation>(req).Then(res =>
            {
                _latest = res;
                List<Generation> gens = new List<Generation>(_generations);
                gens.Add(res);
                _generations = gens.ToArray();
                OnImagesReceived?.Invoke();
            }).Catch(err =>
            {
                Debug.LogWarning(err.Message);
            });
        }

        public void DownloadImage(string url, RawImage image)
        {
            RestClient.Get(new RequestHelper
            {
                Uri = url,
                DownloadHandler = new DownloadHandlerTexture()
            }).Then(res => {
                Texture2D tex = ((DownloadHandlerTexture)res.Request.downloadHandler).texture;
                image.texture = tex;
            }).Catch(err => {
                Debug.LogWarning(err.Message);
            });
        }

        public void DownloadImages(int page, int pageSize)
        {
            RequestHelper req = new RequestHelper
            {
                Uri = _apiUrl + "/api/generations",
                Body = new ApiKeyReq
                {
                    apiKey = _apiKey,
                    page = page,
                    pageSize = pageSize
                },
            };
            RestClient.PostArray<Generation>(req).Then(res => {
                _latest = res[0];
                List<Generation> gens = new List<Generation>(_generations);
                // add only the generations we don't already have locally
                Array.ForEach(res, g =>
                {
                    if (gens.Find(x => x.id == g.id) == null)
                        gens.Add(g);
                });
                _generations = gens.ToArray();
                OnImagesReceived?.Invoke();
            });
        }

        #endregion

        #region GPT

        public void GenerateText(string prompt)
        {
            RequestHelper req = new RequestHelper
            {
                Uri = _apiUrl + "/api/chat-simple",
                Body = new TextGenerationRequest
                {
                    prompt = prompt,
                    apiKey = _apiKey
                },
            };
            RestClient.Post<TextGenerationResponse>(req).Then(res =>
            {
                if (res.status == "OK")
                {
                    _generatedText = res.text;
                    OnTextReceived?.Invoke();
                }
                else
                {
                    _generatedText = "Sorry, something went wrong :(";
                    OnTextReceived?.Invoke();
                }
            }).Catch(err =>
            {
                Debug.LogWarning(err.Message);
            });
        }

        #endregion

        private void Start()
        {
            if (_loadModelsOnStartup)
                GetModels();
        }
    }
}