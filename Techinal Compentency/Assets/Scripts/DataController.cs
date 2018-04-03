using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField]
    [Tooltip("Component used to display downloaded data.")]
    private Text _dataDisplay;
    [SerializeField]
    [Tooltip("Component used by user to input new data.")]
    private InputField _dataInput;
    [SerializeField]
    private ScrollRect _scrollView;

    [Header("Server")]
    [SerializeField]
    private string _hostUrl = "http://charnesnell.000webhostapp.com/letsquiz/";
    [SerializeField]
    private string _uploadFileName = "upload.php?data=";
    [SerializeField]
    private string _downloadFileName = "download.php";

    [Header("Setting")]
    private float _uploadTimer;
    private float _uploadTimeLimit = 1000000.0f;
    private float _downloadTimer;
    private float _downloadTimeLimit = 30.0f;
    private bool _failed = false;

    private void Awake()
    {
        _scrollView.horizontal = false;
        _scrollView.vertical = false;
        _dataDisplay.alignment = TextAnchor.MiddleCenter;
    }

    public void Upload()
    {
        var uploadUrl = _hostUrl + _uploadFileName + _dataInput.text;
        _uploadTimer = 0.0f;
        StartCoroutine(UploadData(uploadUrl));
        _scrollView.vertical = true;
        _dataInput.text = "";
    }

    private IEnumerator UploadData(string url)
    {
        WWW upload = new WWW(url);

        while (!upload.isDone)
        {
            _uploadTimer += Time.deltaTime;
            if (_uploadTimer > _uploadTimeLimit)
            {
                _dataDisplay.text = "Error: Server time out";
                _failed = true;
                break;
            }
            yield return null;
        }
        if (!upload.isDone || upload.error != null)
        {
            _dataDisplay.alignment = TextAnchor.MiddleCenter;
            _dataDisplay.text = "Error: " + upload.error;
        }
        else
        {
            if (!_failed)
            {
                Download();
                yield return upload;
            }
        }
    }

    public void Download()
    {
        var downloadUrl = _hostUrl + _downloadFileName;
        _downloadTimer = 0.0f;
        StartCoroutine(DownloadData(downloadUrl));
        _scrollView.vertical = true;
        _dataInput.text = "";
    }

    private IEnumerator DownloadData(string url)
    {
        WWW download = new WWW(url);

        while (!download.isDone)
        {
            _downloadTimer += Time.deltaTime;
            if (_downloadTimer > _downloadTimeLimit)
            {
                _dataDisplay.text = "Error: Server time out";
                _failed = true;
                break;
            }
            yield return null;
        }
        if (!download.isDone || download.error != null)
        {
            _dataDisplay.alignment = TextAnchor.MiddleCenter;
            _dataDisplay.text = "Error: " + download.error;
        }
        else
        {
            if (!_failed)
            {
                _dataDisplay.alignment = TextAnchor.UpperLeft;
                _dataDisplay.text = download.text;
                yield return download;
            }
        }
    }
}