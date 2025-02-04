# Docker settings to containerize 
## Prepare
1 - Add the Dockerfile and the docker-compose<br>
2 - Add in the *.csproj file the following lines at the end of **PropertyGroup** : <br>
```js
<!-- added elements to pass compilation errors about duplication in generated /obj and /bin content -->
<GenerateAssemblyInfo>false</GenerateAssemblyInfo>
<GenerateTargetFrameworkAttribute>false</GenerateTargetFrameworkAttribute>
<EnableWindowsTargeting>true</EnableWindowsTargeting>
```
## Build and execute
Build Dockerfile : `docker build --rm -t easysave-app .`<br>
Build docker-compose : `docker-compose up --build`

## Evolution
The Dockerfile and the docker-compose are ready for further improvements.
Some functions are currently disabled and can be uncommented to enable container port exposition, target, depends and other...