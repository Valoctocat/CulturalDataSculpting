using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


// Show off all the Debug UI components.
public class QuestDebugHelper: MonoBehaviour
{
    public static QuestDebugHelper Instance;

    bool inMenu;
    Text logText1;
    Text logText2;


    void Awake(){
      Instance = this;
    }

  	void Start ()
      {
          var rt = DebugUIBuilder.instance.AddLabel("Position Circle");
          var rt2 = DebugUIBuilder.instance.AddLabel("Position Player");
          inMenu = false;
          logText1 = rt.GetComponent<Text>();
          logText2 = rt2.GetComponent<Text>();
  	   }

      void Update()
      {
          if(OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
          {
              if (inMenu) DebugUIBuilder.instance.Hide();
              else DebugUIBuilder.instance.Show();
              inMenu = !inMenu;
          }
      }

      public void Log(float message, int instance)
      {
        if(instance == 1) logText1.text ="x player: "+ message;
        if(instance == 2) logText2.text ="z player: "+ message;
      }
}
