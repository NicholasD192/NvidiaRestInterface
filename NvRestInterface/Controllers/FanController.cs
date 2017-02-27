using NvRestInterface.Models;
using System.Web.Http;

namespace NvRestInterface.Controllers
{
    public class FanController : ApiController
    {
        // GET api/Fan/{Graphics card ID} (Fan speed of particular Graphics Cards.)
        public object Get(int id)
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetCoolerSettings(id, true));
        }

        // GET: api/Fan/{Graphics Card ID} (Fan speed of particular Graphics Cards.)
        public object Get()
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetCoolerSettings());
        }
    }
}