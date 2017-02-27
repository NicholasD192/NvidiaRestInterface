using NvRestInterface.Models;
using System.Web.Http;

namespace NvRestInterface.Controllers
{
    public class TemperatureController : ApiController
    {
        // GET: api/Temperature (Temperature Settings for all Graphics Cards.)
        public object Get(int id)
        {
            NvAccessor nvInstance = new NvAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvInstance.GetTemperatureSettings(id, true));
        }

        // GET: api/Temperature/{Graphics Card ID} (Temperature Settings of particular Graphics Cards.)
        public object Get()
        {
            NvAccessor nvInstance = new NvAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvInstance.GetTemperatureSettings());
        }
    }
}