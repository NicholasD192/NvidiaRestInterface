using OpenHardwareMonitor.Hardware.Nvidia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;

namespace NvRestInterface.Models
{
    public class NvAccessor
    {
        private NVAPI _nvapi;
        private List<NvidiaGpu> _gpuList = new List<NvidiaGpu>();
        private IDictionary<NvPhysicalGpuHandle, NvDisplayHandle> _displayHandles = new Dictionary<NvPhysicalGpuHandle, NvDisplayHandle>();
        private NvPhysicalGpuHandle[] _handles = new NvPhysicalGpuHandle[NVAPI.MAX_PHYSICAL_GPUS];

        public NvAccessor()
        {
            _nvapi = new NVAPI();
            CreateGpu();
        }

        private bool GetHandles()
        {
            bool refreshState = false;
            if (NVAPI.NvAPI_EnumNvidiaDisplayHandle != null &&
              NVAPI.NvAPI_GetPhysicalGPUsFromDisplay != null)
            {
                refreshState = true;
                NvStatus status = NvStatus.OK;
                int i = 0;
                while (status == NvStatus.OK)
                {
                    NvDisplayHandle displayHandle = new NvDisplayHandle();
                    status = NVAPI.NvAPI_EnumNvidiaDisplayHandle(i, ref displayHandle);
                    i++;

                    if (status == NvStatus.OK)
                    {
                        NvPhysicalGpuHandle[] handlesFromDisplay =
                          new NvPhysicalGpuHandle[NVAPI.MAX_PHYSICAL_GPUS];
                        uint countFromDisplay;
                        if (NVAPI.NvAPI_GetPhysicalGPUsFromDisplay(displayHandle,
                          handlesFromDisplay, out countFromDisplay) == NvStatus.OK)
                        {
                            for (int j = 0; j < countFromDisplay; j++)
                            {
                                if (!_displayHandles.ContainsKey(handlesFromDisplay[j]))
                                    _displayHandles.Add(handlesFromDisplay[j], displayHandle);
                            }
                        }
                    }
                }
            }
            return refreshState;
        }

        private void CreateGpu()
        {
            int count;
            NvStatus status = NVAPI.NvAPI_EnumPhysicalGPUs(_handles, out count);
            for (int i = 0; i < count; i++)
            {
                NvDisplayHandle displayHandle;
                _displayHandles.TryGetValue(_handles[i], out displayHandle);
                _gpuList.Add(new NvidiaGpu(i, _handles[i], displayHandle));
            }
        }

        public string GetTemperatureSettings(int adapterId = 0, bool specificGpu = false)
        {
            Dictionary<string, int> tempertureInfo = new Dictionary<string, int>();
            string temperatureSettings = "";
            if (specificGpu)
            {
                foreach (NvidiaGpu gpu in _gpuList.Where(gpu => gpu.AdapterIndex == adapterId))
                {
                    temperatureSettings = Utilities.Utilities.SerializeObject(gpu.GetThermalSettings);
                }
            }
            else
            {
                foreach (NvidiaGpu gpu in _gpuList)
                {
                    temperatureSettings += Utilities.Utilities.SerializeObject(gpu.GetThermalSettings);
                }
            }

            return temperatureSettings;
        }

        public string GetMemorySettings(int adapterId = 0, bool specificGpu = false)
        {
            string memorySettings = "";
            foreach (NvidiaGpu gpu in _gpuList)
            {
                memorySettings += Utilities.Utilities.SerializeObject(ConvertMemorySettings(gpu));
            }

            return memorySettings;
        }

        private Dictionary<string, float> ConvertMemorySettings(NvidiaGpu gpu)
        {
            Dictionary<string, float> memoryInfo = new Dictionary<string, float>();
            uint totalMemory = gpu.GetMemorySettings.Values[0];

            uint freeMemory = gpu.GetMemorySettings.Values[4];
            float usedMemory = Math.Max(totalMemory - freeMemory, 0);
            memoryInfo.Add("AdapterID", gpu.AdapterIndex);
            memoryInfo.Add("MemoryTotal", (float)totalMemory / 1024);
            memoryInfo.Add("MemoryFree", (float)freeMemory / 1024);
            memoryInfo.Add("MemoryUsed", usedMemory / 1024);
            memoryInfo.Add("MemoryLoad", 100f * usedMemory / totalMemory);

            return memoryInfo;
        }

        public string GetOverview()
        {
            Dictionary<int, string> gfxInfo = new Dictionary<int, string>();
            foreach (NvidiaGpu gpu in _gpuList)
            {
                gfxInfo.Add(gpu.AdapterIndex, gpu.GpuName);
            }

            return Utilities.Utilities.SerializeObject(gfxInfo);
        }

        public string GetNvApiVersion()
        {
            string version = "";
            if (NVAPI.NvAPI_GetInterfaceVersionString(out version) == NvStatus.OK)
            {
                Utilities.Utilities.PrintDebug(version, true);
            }
            return version;
        }
    }
}