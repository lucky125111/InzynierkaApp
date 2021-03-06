﻿using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolumeController : ControllerBase
    {
        private readonly IVolumeService _volumeService;

        public VolumeController(IVolumeService volumeService)
        {
            _volumeService = volumeService;
        }

        [HttpPost("Calculate/{id}&{type}")]
        public void Recalculate(int id, string type)
        {
            if (type != null && type != "Simple" && type != "ConvexHull" && type != "CountPixels")
                throw new AppException("type value can be either \"Simple\" or \"ConvexHull\" or \"CountPixels\"");

            if (type == null)
                type = "ConvexHull";

            _volumeService.CalculateVolume(id, type);
        }

        [HttpGet("{id}")]
        public double Get(int id)
        {
            var volume = _volumeService.GetVolume(id);
            return volume;
        }
    }
}