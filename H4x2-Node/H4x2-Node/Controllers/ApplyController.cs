﻿// Tide Protocol - Infrastructure for the Personal Data economy
// Copyright (C) 2019 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Source License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Source License for more details.
// You should have received a copy of the Tide Community Open 
// Source License along with this program.
// If not, see https://tide.org/licenses_tcosl-1-0-en

using Microsoft.AspNetCore.Mvc;
using H4x2_TinySDK.Ed25519;
using H4x2_TinySDK.Math;
using System.Numerics;

namespace H4x2_Node.Controllers
{
    public class ApplyController : Controller
    {
        private Settings _settings { get; }
        private ThrottlingManager _throttlingManager;
        public ApplyController(Settings settings)
        {
            _settings = settings;
            _throttlingManager = new ThrottlingManager();
        }

        public ActionResult<string> Prism([FromBody] Point point) => Apply(point, _settings.PRISM);

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
        public ActionResult<int> Throttle()
        {
            var ip = Request.HttpContext.Connection.RemoteIpAddress; // Get client's IP
            if(ip is not null)
                return _throttlingManager.Throttle(ip.ToString()).GetAwaiter().GetResult(); 
            else
                return BadRequest("IP address is null !"); 
        }
    }
}
