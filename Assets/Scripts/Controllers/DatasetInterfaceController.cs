using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatasetInterfaceController : MonoBehaviour
{

    public GameObject _curvedUIPrefab;
    private GameObject _curvedUI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStart() {
          _curvedUI = instantiatePrefab(0);
          print("Start UI");
    }

    public void OnStop() {
          Destroy(_curvedUI);
    }


    private GameObject instantiatePrefab(int rotation_offset) {
      // Postion & Orientation
      Vector3 position_curved_plane = _curvedUIPrefab.transform.position;
      Quaternion orientation_curved_plane = Quaternion.Euler(270, rotation_offset*40, 0);

      // Instantiate
      GameObject curved_plane = Instantiate(_curvedUIPrefab, position_curved_plane, orientation_curved_plane);
      curved_plane.transform.parent = this.transform.parent;

      return curved_plane;
    }

}
