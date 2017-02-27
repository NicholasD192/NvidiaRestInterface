using NvRestInterface.Models;
using System.Web.Http;

namespace NvRestInterface.Controllers
{
    public class TemperatureController : ApiController
    {
        // GET: api/Temperature (Temperature Settings for all Graphics Cards.)
        public object Get(int id)
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetTemperatureSettings(id, true));
        }

        // GET: api/Temperature/{Graphics Card ID} (Temperature Settings of particular Graphics Cards.)
        public object Get()
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetTemperatureSettings());
        }
    }
}