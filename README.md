# Nvidia Rest Interface

Provides a simple RestFul Interface to the low level Nvidia API.

## Run the application:

If you have Visual Studio 2017 installed, Simply open the project solution and build the project.

Project should also be compatible with Visual Studio 2015.

## Dev notes

The API currently has 3 seperate Controllers.

GET api/Nvidia ->

Returns a list of Graphics cards currenlty in use with their respective adapter index.
These Index's can then be used in further API calls for specific graphics cards.
```
{
  0: "GeForce GTX 1080"
}
```

GET api/Tempearture/{AdapterIndex} ->

Returns the current, minimum and Maximum Temperature of each graphics card connected.
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

Returns the total, used and free Memory of each graphics card connected.
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
## To Do

 - Extend Api to extract CoolerSettings, DriverSettings, ClocksSpeeds
 - Implmenet Architecture to push change settings (Change Fan speeds etc)


## Acknowledgments
A big thank you to the OpenHardwareMonitor & Newtonsoft.JSON projects.

https://github.com/openhardwaremonitor

https://github.com/JamesNK/Newtonsoft.Json

## License

Mozilla Public License 2.0
