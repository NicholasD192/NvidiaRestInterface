using OpenHardwareMonitor.Hardware.Nvidia;

namespace NvRestInterface.Models
{
    internal class NvidiaGpu
    {
        public int AdapterIndex;
        private string _gpuName;
        private readonly NvPhysicalGpuHandle _handle;
        private readonly NvDisplayHandle _displayHandle;
        private NvGPUThermalSettings _thermalSettings;
        private NvGPUCoolerSettings _coolerSettings;
        private NvMemoryInfo _memoryInfo;
        private NvDisplayDriverVersion _driverVersion;
        private NvClocks _allClocks;
        private NvPStates _performanceStates;

        public NvidiaGpu(int adapterIndex, NvPhysicalGpuHandle handle, NvDisplayHandle displayHandle)
        {
            this.AdapterIndex = adapterIndex;
            this._handle = handle;
            this._displayHandle = displayHandle;
            NVAPI.NvAPI_GPU_GetFullName(handle, out _gpuName);
        }

        public string GpuName
        {
            get { return _gpuName; }
            set { _gpuName = value; }
        }

        private void Update(NvidiaDataType updateType)
        {
            switch (updateType)
            {
                case NvidiaDataType.Temperature:
                    UpdateTemperatureSettings();
                    break;

                case NvidiaDataType.Memory:
                    UpdateMemorySettings();
                    break;

                case NvidiaDataType.Driver:
                    UpdateDriverSettings();
                    break;

                case NvidiaDataType.Cooler:
                    UpdateCoolerSettings();
                    break;

                case NvidiaDataType.PerformanceStats:
                    UpdatePerformaneStates();
                    break;

                case NvidiaDataType.Clocks:
                    UpdateClocks();
                    break;

                default:
                    break;
            }
        }

        public NvGPUThermalSettings GetThermalSettings
        {
            get
            {
                Update(NvidiaDataType.Temperature);
                return _thermalSettings;
            }
            set { _thermalSettings = value; }
        }

        public NvMemoryInfo GetMemorySettings
        {
            get
            {
                Update(NvidiaDataType.Memory);

                return _memoryInfo;
            }
            set { _memoryInfo = value; }
        }

        private void UpdateTemperatureSettings()
        {
            _thermalSettings = new NvGPUThermalSettings();
            _thermalSettings.Version = NVAPI.GPU_THERMAL_SETTINGS_VER;
            _thermalSettings.Count = NVAPI.MAX_THERMAL_SENSORS_PER_GPU;
            _thermalSettings.Sensor = new NvSensor[NVAPI.MAX_THERMAL_SENSORS_PER_GPU];
            NVAPI.NvAPI_GPU_GetThermalSettings(_handle, (int)NvThermalTarget.ALL, ref _thermalSettings);
        }

        private void UpdateCoolerSettings()
        {
            _coolerSettings = new NvGPUCoolerSettings();
            _coolerSettings.Version = NVAPI.GPU_COOLER_SETTINGS_VER;
            _coolerSettings.Count = NVAPI.MAX_THERMAL_SENSORS_PER_GPU;
            _coolerSettings.Cooler = new NvCooler[NVAPI.MAX_COOLER_PER_GPU];
            NvStatus status = NVAPI.NvAPI_GPU_GetCoolerSettings(_handle, 0, ref _coolerSettings);
        }

        private void UpdateMemorySettings()
        {
            _memoryInfo = new NvMemoryInfo();
            _memoryInfo.Version = NVAPI.GPU_MEMORY_INFO_VER;
            _memoryInfo.Values = new uint[NVAPI.MAX_MEMORY_VALUES_PER_GPU];
            NvStatus status = NVAPI.NvAPI_GPU_GetMemoryInfo(_displayHandle, ref _memoryInfo);
        }

        private void UpdateDriverSettings()
        {
            _driverVersion = new NvDisplayDriverVersion();
            _driverVersion.Version = NVAPI.DISPLAY_DRIVER_VERSION_VER;
            NVAPI.NvAPI_GetDisplayDriverVersion(_displayHandle, ref _driverVersion);
        }

        private void UpdateClocks()
        {
            _allClocks = new NvClocks();
            _allClocks.Version = NVAPI.GPU_CLOCKS_VER;
            _allClocks.Clock = new uint[NVAPI.MAX_CLOCKS_PER_GPU];
            NVAPI.NvAPI_GPU_GetAllClocks(_handle, ref _allClocks);
        }

        private void UpdatePerformaneStates()
        {
            _performanceStates = new NvPStates();
            _performanceStates.Version = NVAPI.GPU_PSTATES_VER;
            _performanceStates.PStates = new NvPState[NVAPI.MAX_PSTATES_PER_GPU];
            NVAPI.NvAPI_GPU_GetPStates(_handle, ref _performanceStates);
        }
    }
}