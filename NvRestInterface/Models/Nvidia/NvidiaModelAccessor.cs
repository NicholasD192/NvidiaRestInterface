using OpenHardwareMonitor.Hardware.Nvidia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;

namespace NvRestInterface.Models
{
    /// <summary>
    /// This Class acts as a High Level Acessor for the NvidiaGPUModel.cs.
    /// </summary>
    public class NvidiaModelAccessor
    {
        private List<NvidiaGpuModel> _gpuList = new List<NvidiaGpuModel>();

        public NvidiaModelAccessor()
        {
            //This constructor itterates through each of GPU's and generates a NvidiaGpuModel Object for each of them appending.
            int count;
            IDictionary<NvPhysicalGpuHandle, NvDisplayHandle> displayHandles = new Dictionary<NvPhysicalGpuHandle, NvDisplayHandle>();
            NvPhysicalGpuHandle[] handles = new NvPhysicalGpuHandle[NVAPI.MAX_PHYSICAL_GPUS];

            NvStatus status = NVAPI.NvAPI_EnumPhysicalGPUs(handles, out count);
            for (int i = 0; i < count; i++)
            {
                NvDisplayHandle displayHandle;
                displayHandles.TryGetValue(handles[i], out displayHandle);
                _gpuList.Add(new NvidiaGpuModel(i, handles[i], displayHandle));
            }
        }

        public string GetOverview()
        {
            //Returns an array of AdapterIndex & Graphics Card Names
            Dictionary<int, string> gfxInfo = new Dictionary<int, string>();
            foreach (NvidiaGpuModel gpu in _gpuList)
            {
                gfxInfo.Add(gpu.AdapterIndex, gpu.GpuName);
            }

            return Utilities.Utilities.SerializeObject(gfxInfo);
        }

        public string GetNvApiVersion()
        {
            //Returns Version of Nvidia API.
            string version = "";
            if (NVAPI.NvAPI_GetInterfaceVersionString(out version) == NvStatus.OK)
            {
                Utilities.Utilities.PrintDebug(version);
            }
            return Utilities.Utilities.SerializeObject(version);
        }

        public string GetTemperatureSettings(int adapterId = 0, bool specificGpu = false)
        {
            List<Dictionary<string, int>> tempInfo = new List<Dictionary<string, int>>();
            if (specificGpu)
            {
                foreach (NvidiaGpuModel gpu in _gpuList.Where(gpu => gpu.AdapterIndex == adapterId))
                {
                    tempInfo.Add(ConvertTemperatureSettings(gpu));
                }
            }
            else
            {
                foreach (NvidiaGpuModel gpu in _gpuList)
                {
                    tempInfo.Add(ConvertTemperatureSettings(gpu)); ;
                }
            }

            return Utilities.Utilities.SerializeObject(tempInfo);
        }

        public string GetMemorySettings(int adapterId = 0, bool specificGpu = false)
        {
            List<Dictionary<string, float>> memoryInfo = new List<Dictionary<string, float>>();
            if (specificGpu)
            {
                foreach (NvidiaGpuModel gpu in _gpuList.Where(gpu => gpu.AdapterIndex == adapterId))
                {
                    memoryInfo.Add(ConvertMemorySettings(gpu));
                }
            }
            else
            {
                foreach (NvidiaGpuModel gpu in _gpuList)
                {
                    memoryInfo.Add(ConvertMemorySettings(gpu)); ;
                }
            }

            return Utilities.Utilities.SerializeObject(memoryInfo);
        }

        public string GetClockSettings(int adapterId = 0, bool specificGpu = false)
        {
            List<Dictionary<string, int>> clockInfo = new List<Dictionary<string, int>>();

            if (specificGpu)
            {
                foreach (NvidiaGpuModel gpu in _gpuList.Where(gpu => gpu.AdapterIndex == adapterId))
                {
                    clockInfo.Add(ConvertClockSettings(gpu));
                }
            }
            else
            {
                foreach (NvidiaGpuModel gpu in _gpuList)
                {
                    clockInfo.Add(ConvertClockSettings(gpu)); ;
                }
            }

            return Utilities.Utilities.SerializeObject(clockInfo);
        }

        public string GetCoolerSettings(int adapterId = 0, bool specificGpu = false)
        {
            List<Dictionary<string, int>> coolerInfo = new List<Dictionary<string, int>>();
            if (specificGpu)
            {
                foreach (NvidiaGpuModel gpu in _gpuList.Where(gpu => gpu.AdapterIndex == adapterId))
                {
                    coolerInfo.Add(ConverCoolerSettings(gpu));
                }
            }
            else
            {
                foreach (NvidiaGpuModel gpu in _gpuList)
                {
                    coolerInfo.Add(ConverCoolerSettings(gpu)); ;
                }
            }

            return Utilities.Utilities.SerializeObject(coolerInfo);
        }

        private Dictionary<string, float> ConvertMemorySettings(NvidiaGpuModel gpuModel)
        {
            Dictionary<string, float> memoryInfo = new Dictionary<string, float>();

            uint totalMemory = gpuModel.GetMemorySettings.Values[0];
            uint freeMemory = gpuModel.GetMemorySettings.Values[4];
            float usedMemory = Math.Max(totalMemory - freeMemory, 0);
            memoryInfo.Add("AdapterID", gpuModel.AdapterIndex);
            memoryInfo.Add("MemoryTotal", (float)totalMemory / 1024);
            memoryInfo.Add("MemoryFree", (float)freeMemory / 1024);
            memoryInfo.Add("MemoryUsed", usedMemory / 1024);
            memoryInfo.Add("MemoryLoad", 100f * usedMemory / totalMemory);

            return memoryInfo;
        }

        private Dictionary<string, int> ConvertTemperatureSettings(NvidiaGpuModel gpuModel)
        {
            Dictionary<string, int> tempInfo = new Dictionary<string, int>();
            int count = 0;
            foreach (NvSensor sensor in gpuModel.GetThermalSettings.Sensor)
            {
                if (gpuModel.GetThermalSettings.Count != count)
                {
                    tempInfo.Add("CurrentTemp", Convert.ToInt32(sensor.CurrentTemp));
                    tempInfo.Add("MaximumTemp", Convert.ToInt32(sensor.DefaultMaxTemp));
                    tempInfo.Add("MinimumTemp", Convert.ToInt32(sensor.DefaultMinTemp));
                    tempInfo.Add("TargetTemp", Convert.ToInt32(sensor.Target));
                    count++;
                }
                else
                {
                    break;
                }
            }

            return tempInfo;
        }

        private Dictionary<string, int> ConvertClockSettings(NvidiaGpuModel gpuModel)
        {
            Dictionary<string, int> clockInfo = new Dictionary<string, int>();

            uint[] clocks = gpuModel.GetClockSettings.Clock;
            if (clocks != null)
            {
                clockInfo.Add("GPU Core", Convert.ToInt32(0.001f * clocks[8]));
                if (clocks[30] != 0)
                {
                    clockInfo.Add("GPU Memory", Convert.ToInt32(0.0005f * clocks[30]));
                    clockInfo.Add("GPU Shader", Convert.ToInt32(0.001f * clocks[30]));
                }
                else
                {
                    clockInfo.Add("GPU Memory", Convert.ToInt32(0.001f * clocks[0]));
                    clockInfo.Add("GPU Shader", Convert.ToInt32(0.001f * clocks[14]));
                }
            }

            return clockInfo;
        }

        private Dictionary<string, int> ConverCoolerSettings(NvidiaGpuModel gpuModel)
        {
            Dictionary<string, int> coolerInfo = new Dictionary<string, int>();
            int count = 0;
            foreach (NvCooler cooler in gpuModel.GetCoolerSettings.Cooler)
            {
                if (gpuModel.GetCoolerSettings.Count != count)
                {
                    coolerInfo.Add("CurrentSpeed", Convert.ToInt32(cooler.CurrentPolicy));
                    coolerInfo.Add("CurrentMin", Convert.ToInt32(cooler.CurrentMin));
                    coolerInfo.Add("CurrentMax", Convert.ToInt32(cooler.CurrentMax));
                    count++;
                }
                else
                {
                    break;
                }
            }

            return coolerInfo;
        }
    }
}