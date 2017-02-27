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
            NvAccessor nvInstance = new NvAccessor();
            return Utilities.Utilities.DeSerialiseObject(nvInstance.GetOverview());
        }
    }
}