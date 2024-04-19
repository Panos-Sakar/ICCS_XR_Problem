<h1 align="center">
  <br>
  ICCS XR Problem
  <br>
</h1>

<h4 align="center">Problem-set: for Unity Developer, XR team @ISense Group, ICCS</h4>
<p align="center">
  <a href="#About">About</a> •
  <a href="#Task-1">Task 1</a> •
  <a href="#Task-2">Task 2</a> •
  <a href="#Attributions">Attributions</a>
</p>

# About

Created using: [Unity](https://unity.com/) version [2022.3.24f1](https://unity.com/releases/editor/whats-new/2022.3.24)
<br>
Platform: Should work on any platform the Unity game engine supports except Android and WebGL (see [Notes](#notes) for more). (Tested on [Debian Linux](https://www.debian.org/))

```bash
# Clone this repository
$ git clone https://github.com/Panos-Sakar/ICCS_XR_Problem.git
```

<br>
<br>

## Task 1

Relevant Scene: Assets > Scenes > Task 1.unity
<br>

Here the purpose of the task is to spawn a mesh every 5 seconds from a list of JSON files and move it along the Z axis. Then destroy the object after 2 meters of movement.
<br>

<p>
  The 'EventCaller.cs' component is responsible for reading the JSON files from StreamingAssets and fire an event every 5 seconds.
For the events, an "Event System" was used to decouple scripts from each other. ObjectEventSystem.cs is the class that implements this pattern (from now on "EventSystem" will refer to this script) and holds events that other scripts can access through the static field caled "Current". This script is the only dependency that is needed for the most part.
</p>

> [!NOTE]  
> The conversion of Y and Z axis does not happen in the EventCaller.cs script. This is to simplify the deserialization process. Conversions happen when the object is spawned. 
> Also data from the JSON file that are not used are not deserialized at all. eg. the header property.

<p>
  ObjectSpawner.cs listens for the create object event from the EventSystem and instantiates an object depending on the "object_id". The object that is spawned has the "SpawnedObject.cs" component, which is responsible for setting up, moving and deleting the object.
</p>

### Contour Points

<p>
  The provided JSON files have a field called "contour_points" which holds 2D point data. When attempting to visualize the data in Unity the following results were produced:
  
  <p align="center">
    <img width=30% src="https://raw.githubusercontent.com/Panos-Sakar/ICCS_XR_Problem/master/ReadmeImages/Image00001.png">
    <img width=30% src="https://raw.githubusercontent.com/Panos-Sakar/ICCS_XR_Problem/master/ReadmeImages/Image00003.png">
    <img width=30% src="https://raw.githubusercontent.com/Panos-Sakar/ICCS_XR_Problem/master/ReadmeImages/Image00006.png">
  </p>
  These points don't seem to display any meaningful shape and instead of using them, one of the default Unity meshes was used as visual for the Spawned Objects.
</p>
<br>

>[!NOTE]
> You can bring back the Contour Point visualisation by changing the following:
> * In "Project Settings > Player > Other Settings > Scripting Define Symbols" add a new Symbol with the string: ```VISUALIZE_CONTOUR_POINTS```
> * On the "Object Spawner" prefab inside the scene, change the "Object Prefab" reference with the prefab "Assets > Prefabs > Spawned Objects > Spawned Object (Line Renderer)"

<br>

### Notes

* On Android and WebGL reading from Streaming Assets should use an web call, and thus this project will need a bit of modification to read the JSON files.
* Every asset and script with the name "Spawned Object" refers to an object from Task 1.
* The velocity from the JSON files seemed very slow and is multiplyed by 10 to speed things up.

<br>
<br>

## Task 2

Relevant Scene: Assets > Scenes > Task 2.unity
<br>

Here the purpose of the task is to make an http call to a server, every time an object from Task 1 is destroyed. Depending on the response from the web call, spawn an object at a designated position, resize it and add labels with the contents of attributes provided within the http response.

<p>
  The EventSystem is also a dependansy here and should be included in the scene. The "Event Caller" and "Object Spawner" are also nedded, in order to spawn objects, but are not required. If another script calls the apropriate event this portion of the project will work fine.
</p>

<p>
  The "DynamicObjectSpawner.cs" component is listening for destroy events. Makes the http call to the mock server and spawns the apropriate object. The objects are named "Dynamic Objects" to distinguish them from objects from Task 1. The "DynamicObject.cs" component moves, resizes and animates the new object and gets one Label for each Attribute from the "LabelPool.cs" component. A box collider is also added.
</p>
<p>
  For the Labels, a Pooling System is used to create and provide 3D Labels for any script that needs one.
  The Labels are rendered with a different camera that is overlapped to the output of the main camera. With this method the Labels are not occluded by other objects.
</p>
<p>
  To destroy DynamicObjects the component "MouseRaycaster.cs" casts a ray when the left mouse button is pressed. If the ray hits a Dinamic Object, the "DestroyMe()" method is called on the object to destroy it. This method also informs the DynamicObjectSpawner for the destruction so another object of the same type can be spawned.
</p>

### Mock Get Request Pakage

For the mocking of http requests a new Unity package was created (Located in Packages > com.iccs.mock-get-request folder). This package is independent from the rest of the project and can be used to make http requests to the mocking server. The service [Mockable.io](https://www.mockable.io) was used to simulate a server.

<br>

> **Pakage Usage**
> * Create a GameObject with the MockHandler.cs component to use it.
> * Call the Create() method to get a Handler.
> * And then the GET() method on the handler to make the http call.
> * Use OnSuccess() and OnFail() methods on the handler to receive callbacks.

<br>

### Notes
* The fbx models used here are from the [Poly Pizza](https://poly.pizza) website (see [Attributions](#attributions)) and were not modified.
* The Raycaster that is used to destroy the objects is using the "Input.GetMouseButtonUp(0)" function. If this project runs on an HMD the raycast may not work properly. Additional checks should be made for this senario.

<br>
<br>

# Attributions

* Chair by [Kay Lousberg](https://poly.pizza/m/99QQsFyyMA)
* Tree by ParfaitUwU [CC-BY](https://creativecommons.org/licenses/by/3.0/) via [Poly Pizza](https://poly.pizza/m/MSuchZNT2G)
* Crate of Tomatoes by [Kay Lousberg](https://poly.pizza/m/YrCZgQcpMN)

<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
