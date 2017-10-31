# Nvidia Rest Interface

Provides a simple restful interface to the low-level Nvidia API.

## Run the application:

If you have Visual Studio 2017 installed, simply open the project solution and build the project.

The project should also be compatible with Visual Studio 2015.

## Architecture

Each public function in the NvidiaModelAccessor class follows the same methodology. Example below is the call stack for acquiring thermal information.

1. NvidiaModelAccessor.GetTemperatureSettings() -> Called from the constructor.
2. NvidiaGpuModel.GetThermalSettings() -> Returns unformatted data from NvidiaGPUModel class to the Accessor
3. ConvertTemperatureSettings() -> Reformats data into an array that can be easily converted into JSON.

## Dev Notes

The API currently has 5 separate Controllers.

GET api/Nvidia ->

Returns a list of Graphics cards currently in use with their respective adapter index.
These indexes can then be used in further API calls for specific graphics cards.
```
{
  0: "GeForce GTX 1080"
}
```

GET api/Tempearture/{AdapterIndex} ->

Returns the current, minimum and maximum temperature of each graphics card connected.
If no AdapterIndex is provided temperature information from all graphics cards is returned.
```
[
  {
    CurrentTemp: 38,
    MaximumTemp: 127,
    MinimumTemp: 0,
    TargetTemp: 1
  }
]
```

Returns the total, used and free memory of each graphics card connected.
If no AdapterIndex is provided memory information from all graphics cards is returned.

GET api/Memory/{AdapterIndex} ->
```
[
  {
    AdapterID: 0,
    MemoryTotal: 8192,
    MemoryFree: 7536.246,
    MemoryUsed: 655.7539,
    MemoryLoad: 8.004808
  }
]
```
Returns the GPU Core, memory and shader clock speeds of each graphics card connected.
If no AdapterIndex is provided memory information from all graphics cards is returned.

GET api/Clock/{AdapterIndex} ->
```
[
  {
    GPU Core: 405,
    GPU Memory: 253,
    GPU Shader: 506
  }
]
```
Returns the current, minimum and maximum fan speeds of each graphics card connected.
If no AdapterIndex is provided memory information from all graphics cards is returned.

GET api/Fan/{AdapterIndex} ->
```
[
  {
    CurrentSpeed: 16,
    CurrentMin: 0,
    CurrentMax: 100
  }
]
```

## To Do

 - More proficient error handling
 - Implement architecture to push modifications (Change fan speeds, overlock etc)
 - Extend API to DriverSettings / Performance states.

## Acknowledgments
[OpenHardwareMonitor](https://github.com/openhardwaremonitor) & [Newtonsoft.JSON](https://github.com/JamesNK/Newtonsoft.Json) projects.

## License

Mozilla Public License 2.0
