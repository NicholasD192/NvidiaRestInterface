using NvRestInterface.Models;
using System.Web.Http;

namespace NvRestInterface.Controllers
{
    public class MemoryController : ApiController
    {
        // GET api/Memory/{Graphics card ID}
        public object Get(int id)
        {
            NvAccessor nvInstance = new NvAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvInstance.GetMemorySettings(id));
        }

        // GET: api/Temperature/{Graphics Card ID} (Temperature Settings of particular Graphics Cards.)
        public object Get()
        {
            NvAccessor nvInstance = new NvAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvInstance.GetMemorySettings());
        }
    }
}