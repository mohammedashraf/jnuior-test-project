using Ashrftest.Context;
using Ashrftest.Models;
using Ashrftest.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ashrftest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly NewDbContext dbContext;
        private readonly ILogger _logger;

        public MeasurementsController(NewDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [Authorize(Roles = "User")]
        [HttpGet("Get All Measurements for all sensors")]
        public async Task<ActionResult<Measurement>> GetMeasurement()
        {
            return Ok(dbContext.Measurements.ToList());
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Measurement>> GetMeasurement(int? id)
        {
            var todoItem = await dbContext.Sensors.Include(x => x.Measurements)
                .FirstOrDefaultAsync(x => x.SensorID == id);

            if (todoItem == null)
            {
                return NotFound();
            }
            var res = dbContext.Measurements.ToList();

            //var metadata = new
            //{
            //    todoItem.TotalCount,
            //    accounts.PageSize,
            //    accounts.CurrentPage,
            //    accounts.TotalPages,
            //    accounts.HasNext,
            //    accounts.HasPrevious
            //};

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            //_logger.LogInformation($"Returned {Measurement.TotalCount} owners from database.");

            //        return PagedList<Measurement>.ToPagedList(F().OrderBy(on => on.Name),
            //ownerParameters.PageNumber,
            //ownerParameters.PageSize);
            return Ok(res);

        }
        [Authorize(Roles = "Moderator")]
        [HttpPost("Add Measurement to sensor")]
        public IActionResult AddMeasurement(MeasurementViewDTO dto)
        {
            //Make Sure That sensor type is olready exists
            var todoItem =  dbContext.Sensors.Include(x => x.Measurements)
                .FirstOrDefaultAsync(x => x.SensorType == dto.SensorType);
            if (todoItem == null)
                return BadRequest();
            if (dto.MeasurementValue > todoItem.Result.SensorMaxValue || dto.MeasurementValue < todoItem.Result.SensorMinValue)
                return BadRequest();
                dbContext.Measurements.Add(
                new Measurement
                {
                    SensorType = dto.SensorType,
                    DateTime = dto.DateTime,
                    MeasurementValue = dto.MeasurementValue,
                    SensorID = todoItem.Result.SensorID
                });
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
