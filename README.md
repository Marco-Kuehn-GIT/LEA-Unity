# LEA-Unity

## Installation
1. Create a new Unity Project 

2. Clear Assests folder 

3. Navigate to your project with the git bash
   
4. $ git init
   
5. $ git remote add origin \<link\>
   
6. $ git pull origin master
   
7. $ git push --set-upstream origin master

8. Install NuGet for Unity: https://github.com/GlitchEnzo/NuGetForUnity/releases

9. Get the "websocket sharp-netstandard" via NuGet

10. Get the Plugins from https://github.com/jirihybek/unity-websocket-webgl

11. Install the Cinemachine-Package in the Unity package manager

---

## Building for WebGL
1. Change at PlayerSettings -> Publishing Settings -> Compression Format to __Gzip__

2. Check at PlayerSettings -> Resolution and Presentation -> __Run In Background__
