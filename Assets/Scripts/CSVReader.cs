using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CSVReader : MonoBehaviour
{

    public string _datasetpath;


    // Update is called once per frame
    public string[] getDatasetInfo(string dataset)
    {
      print(dataset);
      string[] recordNotFound = {"No Dataset"};

          string[] lines = System.IO.File.ReadAllLines(@_datasetpath);
          print(lines.Length);

          for (int i=0; i<lines.Length; i++) {
              string[] fields = lines[i].Split(';');
              if(nameMatchesDSeet(dataset, fields)) {
                  return fields;
              }
          }

      return recordNotFound;
    }

    public static bool nameMatchesDSeet(string dataset, string[] fields)
    {

        return (fields[0].Equals(dataset));
    }
}
