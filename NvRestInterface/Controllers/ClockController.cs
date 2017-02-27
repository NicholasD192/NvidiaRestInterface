using NvRestInterface.Models;
using System.Web.Http;

namespace NvRestInterface.Controllers
{
    public class ClockController : ApiController
    {
        // GET api/Clock/{Graphics card ID} (Clock speed of particular Graphics Cards.)
        public object Get(int id)
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetClockSettings(id, true));
        }

        // GET: api/Clock/{Graphics Card ID} (Clock Speeds of a particular Graphics Cards.)
        public object Get()
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetClockSettings());
        }
    }
}