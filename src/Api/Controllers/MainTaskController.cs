using Infra.DB;
using Domain.Mappers;
using Domain.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MainTaskController : ControllerBase
    {
        private readonly MyDBContext _context;

        public MainTaskController(MyDBContext context)
        {
            _context = context;
        }

        [HttpGet("List")]
        public IActionResult Get()
        {
            return Ok(_context.MainTasks);
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            var task = _context.MainTasks.Where(x => x.UserId == userId).ToList();
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost("Register")]
        public IActionResult Post(MainTaskRequest taskRequest)
        {
            var newTask = MainTaskMapper.ToClass(taskRequest);
            _context.MainTasks.Add(newTask);
            _context.SaveChanges();
            return Ok(newTask);
        }

        [HttpPut("Update/{mainTaskId}")]
        public IActionResult Put(int mainTaskId, [FromBody] MainTaskRequest updateDescription)
        {
            var task = _context.MainTasks.Find(mainTaskId);
            if (task is null)
                return NotFound("MainTask not found!");

            task.Description = updateDescription.Description;

            _context.Update(task);
            _context.SaveChanges();

            return Ok(task);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int mainTaskId)
        {
            var task = _context.MainTasks.Find(mainTaskId);
            if (task is null)
                return NotFound("MainTask not found!");

            _context.Remove(task);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
