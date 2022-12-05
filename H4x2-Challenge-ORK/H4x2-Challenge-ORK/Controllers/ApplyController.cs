using Microsoft.AspNetCore.Mvc;
using H4x2_TinySDK.Ed25519;
using H4x2_TinySDK.Math;
using System.Numerics;
using System.Runtime;

namespace H4x2_Challenge_ORK.Controllers
{
    public class ApplyController : Controller
    {
        private Settings _settings { get; }
        public ApplyController(Settings settings)
        {
            _settings = settings;
        }

        public ActionResult<string> Prize([FromQuery] Point point) => Apply(point, _settings.PrizeKey);

        public ActionResult<string> Test([FromQuery] Point point) => Apply(point, _settings.TestKey);

        private ActionResult<string> Apply(Point toApply, BigInteger key)
        {
            try
            {
                Point appliedPoint = PRISM.Apply(toApply, key);
                return appliedPoint.ToBase64();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
