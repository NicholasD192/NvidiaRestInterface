using NvRestInterface.Models;
using System.Web.Http;

namespace NvRestInterface.Controllers
{
    public class NvidiaController : ApiController
    {
        // GET api/Nvidia
        //Returns list of current GPU's attached with their respective adapterID's.
        public object GetOverview()
        {
            NvidiaModelAccessor nvidiaModelInstance = new NvidiaModelAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvidiaModelInstance.GetOverview());
        }
    }
}