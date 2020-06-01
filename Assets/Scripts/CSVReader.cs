using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    /*

      Read information for a given dataset in the csv file corresponding to each dataset.

    */

    // Path to csv file
    // Expected format : [pseudo, title, author, microscope, resolution, time steps, number of images]
    public string _datasetpath;


    // Called in UIManager a dataset has been grabbed by the user
    public string[] getDatasetInfo(string dataset)
    {

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
