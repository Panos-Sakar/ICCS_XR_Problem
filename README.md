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
Platform: Should work on any platform the Unity game engine supports. (Tested on [Debian Linux](https://www.debian.org/))


```bash
# Clone this repository
$ git clone https://github.com/Panos-Sakar/ICCS_XR_Problem.git
```

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

### Contur Points

<p>
  The provided JSON files have a field called "contour_points" which holds 2D point data. 
</p>

<br>
<br>
<br>
<br>
<br>
<br>
<br>

## Task 2

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
