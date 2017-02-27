using NvRestInterface.Models;
using System.Web.Http;

namespace NvRestInterface.Controllers
{
    public class MemoryController : ApiController
    {
        // GET api/Memory/{Graphics card ID} (Memory Settings for all Graphics Cards)
        public object Get(int id)
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetMemorySettings(id, true));
        }

        // GET: api/Temperature/{Graphics Card ID} (Memory Settings of particular Graphics Cards.)
        public object Get()
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetMemorySettings());
        }
    }
}