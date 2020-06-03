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

add droso.zip, droso_colored.zip and unzip the files in _droso_ and _droso_colored_ respectively.  
  
In  
> Assets/  

add datasets.csv if not already there.
  
## Data Loading Parameters  
In the scene, the DataLoader object is responsible for loading the data of a respective dataset. You can select the number of time steps and depth you want to load. I recommend trying with ~7 time steps and ~20 depths steps to begin with. In total you can add up to 50 time steps and 125 depth steps.  


  
  
 


