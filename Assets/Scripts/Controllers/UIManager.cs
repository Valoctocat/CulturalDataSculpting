using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject _datasetReaderObject;
    private CSVReader _datasetReader;

    private TextMeshPro _textMeshOnGrabDataset;
    private string _title = null;
    private string _author = null;
    private string _microscope = null;
    private string _resolution = null;
    private string _timeSteps = null;
    private string _totalImages = null;

    void Start() {

        //Hide Screen UI
        isRenderScreen(false);

        // Get Text mesh
        _textMeshOnGrabDataset = FindObjectOfType<TextMeshPro>();

        // Get Reader
        _datasetReader = _datasetReaderObject.GetComponent<CSVReader>();
    }

    void Update() {

        if(Input.GetKeyDown("a")) {
          OnGrab(FindObjectOfType<CubeController>());
        }

        if(Input.GetKeyUp("a")) {
          OnRelease(FindObjectOfType<CubeController>());
        }


    }

    public void OnGrab(CubeController cube) {

        // Display screen
        isRenderScreen(true);

        // Read information from CSV file
        RecoverInfoDataset(cube.gameObject.name);

        // Build Display
        string toDisplay = buildDisplay();

        //Display
        _textMeshOnGrabDataset.SetText(toDisplay);
    }

    public void OnRelease(CubeController cube) {

        // Reset info of dataset
        ResetInfoDataset();

        // Hide display
        isRenderScreen(false);

    }

    private void RecoverInfoDataset(string name) {
        string[] fields = _datasetReader.getDatasetInfo(name);
        print(fields.Length);

        if(fields.Length == 7) {
            _title = fields[1];
            _author = fields[2];
            _microscope = fields[3];
            _resolution = fields[4];
            _timeSteps = fields[5];
            _totalImages = fields[6];
        }
    }

    private void ResetInfoDataset() {

        _title = null;
        _author = null;
        _microscope = null;
        _resolution = null;
        _timeSteps = null;
        _totalImages = null;

    }

    private string buildDisplay() {

        StringBuilder _sb = new StringBuilder();
        _sb.AppendLine();
        _sb.AppendLine();
        _sb.Append($"{_title}");
        _sb.AppendLine();
        _sb.AppendLine();
        _sb.AppendLine();
        _sb.AppendLine();
        _sb.Append($"{_author}");
        _sb.AppendLine();
        _sb.AppendLine();
        _sb.Append($"{_microscope}");
        _sb.AppendLine();
        _sb.Append($"{_resolution}");
        _sb.AppendLine();
        _sb.Append($"{_timeSteps}");
        _sb.AppendLine();
        _sb.Append($"{_totalImages}");
        _sb.Append($" images.");


        return _sb.ToString();
    }

    private void isRenderScreen(bool isRender) {
        this.GetComponent<Renderer>().enabled = isRender;
        foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
            r.enabled = isRender;
        }
    }
}
