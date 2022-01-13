using Ashrftest.Context;
using Ashrftest.Models;
using Ashrftest.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ashrftest.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {

        private readonly NewDbContext dbContext;
        private readonly ILogger _logger;

        public SensorsController(NewDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private SensorValue CalculateSensorValue(double res,double Max, double Min)
        {
            double interval = (Max - Min)/3;

            if (Min <= res  &&res <=interval)
                return SensorValue.Low;

            else if (res <= (interval*2) && interval < res)
                return SensorValue.Medium;

            else
                return SensorValue.High;

        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("Get All sensors")]
        public async Task<ActionResult<Sensor>> GetSensor()
        {
            return Ok(dbContext.Sensors.ToList());
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}- view sensor by id")]
        public async Task<ActionResult<Sensor>> GetSensor(int? id)
        {

            var todoItem = await dbContext.Sensors.Include(x => x.Measurements).FirstOrDefaultAsync(x => x.SensorID == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(new SensorDTO
            {
                SensorID = todoItem.SensorID,
                SensorType = todoItem.SensorType,
                SensorMaxValue = todoItem.SensorMaxValue,
                SensorMinValue = todoItem.SensorMinValue,
                DataTime = todoItem.DataTime,
                SensorValue = CalculateSensorValue(((double)todoItem.SensorValue),todoItem.SensorMaxValue, todoItem.SensorMinValue),
            });
        }

        [Authorize(Roles = "Moderator,Admin")]
        [HttpPost("Add Sensor to the table")]
        public IActionResult AddSensor(SensorAddDTO dto)
        {
            dbContext.Sensors.Add(
                new Sensor
                {
                    SensorType = dto.SensorType,
                    SensorMaxValue = dto.SensorMaxValue,
                    SensorMinValue = dto.SensorMinValue,
                    DataTime = dto.DataTime,
                    SensorValue = CalculateSensorValue((double)dto.SensorValue,dto.SensorMaxValue, dto.SensorMinValue)
                });
            dbContext.SaveChanges();
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}- Edit sensor by All Parameters IN Sensor")]
        public async Task<IActionResult> PutSensor(int id, SensorDTO todoItemDTO)
        {

            if (id != todoItemDTO.SensorID)
            {
                return BadRequest();
            }
            var todoItem = await dbContext.Sensors.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            todoItem.SensorType = todoItemDTO.SensorType;
            todoItem.SensorMaxValue = todoItemDTO.SensorMaxValue;
            todoItem.SensorMinValue = todoItemDTO.SensorMinValue;
            todoItem.DataTime = todoItemDTO.DataTime;
            todoItem.SensorValue = CalculateSensorValue((double)todoItemDTO.SensorValue,todoItemDTO.SensorMaxValue, todoItemDTO.SensorMinValue);
            dbContext.Sensors.Update(todoItem);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}- Delete sensor by Sensor-ID")]
        public async Task<ActionResult<Sensor>> DeleteSensor(long? id)
        {
            var todoItem = await dbContext.Sensors.Include(x => x.Measurements).FirstOrDefaultAsync(x => x.SensorID == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            dbContext.Sensors.Remove(todoItem);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }


    }
}
