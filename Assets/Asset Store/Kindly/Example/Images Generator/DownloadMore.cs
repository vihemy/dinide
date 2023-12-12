using UnityEngine;
using MADD;

public class DownloadMore : MonoBehaviour
{
    public int _page = 1;

    public void DownloadMoreImages()
    {
        Kindly.Instance.DownloadImages(_page, 5);
        _page++;
    }
}
