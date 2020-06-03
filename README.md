# Cells of life: a biomersive experience
Valentin Gabeff, Francesca Luongo, Robin Szymczak  
  
“Man lives between the infinitely large and the infinitely small.”  
— Blaise Pascal  

Cells of Life is a virtual reality experience offering a novative way to interact with the physioloigcal heritage of humanity.

## Download
After cloning the project, you can import it in Unity.  
Unity version: 2019.3.5f1  
Switch plateform to Android before adding the data to the project

## Adding data
In  
> Assets/Resources/  

add droso_colored.zip and unzip the files in  _droso_colored_.  
_droso_ should already be present in the folder, otherwise proceed similarly with droso.zip  
_datasets.csv_ should already be present in   
> Assets/   
  
## Data Loading Parameters  
* In the scene, the DataLoader object is responsible for loading the data of a selected dataset. You can set the number of time steps and depth you want to load. I recommend trying with ~7 time steps and ~20 depths steps to begin with. In total you can load to 50 time steps and 125 depth steps (6250 images).  
* CSVReader reads the data from _datasets.csv_, make sure the path is set to _Assets/datasets.csv_
  
## Computer Debugging
There is an FPS mode in this project.
To enable it make the following changes:  
* Uncheck the _droso_ rigidbody _Use Gravity_ option  
* In the right _OVR Controller Prefab_ uncheck controllers  
* In _User_ enable the _Camera Control_ script
  
Revert those changes before building the application to Android    
  
Move camera with the mouse
Move player with the they arrow keys  
Move droso object to the pedestal by changing its transform to 0, 1.05, 0  
Display UI by pressing "u"  
To play/pause a screen (second room), press "p", then you can move in time by pressing "e"/"r", or in space with "a"/"z".




  
  
 


