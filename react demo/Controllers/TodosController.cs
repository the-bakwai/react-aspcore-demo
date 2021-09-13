using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using react_demo.Models;

namespace react_demo.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TodosController> _logger;

        public TodosController(IUnitOfWork unitOfWork, ILogger<TodosController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Todo>> Index()
        {
            return await _unitOfWork.Todos.GetAll();
        }

        [HttpGet("{id:long}")]
        public async Task<Todo> Get(long id)
        {
            return await _unitOfWork.Todos.Get(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([Bind("description, completed")] Todo todo)
        {
            if (User.Identity == null)
            {
                return Unauthorized();
            }

            var user = await _unitOfWork.Users.FindOrCreateByAuthId(User.Identity.Name);

            if (ModelState.IsValid)
            {
                todo.User = user;
                await _unitOfWork.Todos.Add(todo);
                await _unitOfWork.SaveChanges();

                return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
            }
            else
            {
                return UnprocessableEntity();
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [Bind("description, completed")] Todo todo)
        {
            if (User.Identity == null)
            {
                return Unauthorized();
            }

            var user = await _unitOfWork.Users.FindByAuthId(User.Identity.Name);

            if (user == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var oldTodo = await _unitOfWork.Todos.Get(id);
                if (oldTodo.UserId != user.Id)
                {
                    return BadRequest();
                }

                oldTodo.Completed = todo.Completed;
                oldTodo.Description = todo.Description;
                oldTodo.DateUpdated = DateTime.Now;
                if (todo.Completed && oldTodo.DateCompleted != null)
                {
                    oldTodo.DateCompleted = DateTime.Now;
                }
                else
                {
                    oldTodo.DateCompleted = null;
                }

                await _unitOfWork.SaveChanges();

                return NoContent();
            }
            else
            {
                return UnprocessableEntity();
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (User.Identity == null)
            {
                return Unauthorized();
            }

            var user = await _unitOfWork.Users.FindByAuthId(User.Identity.Name);

            if (user == null)
            {
                return BadRequest();
            }

            var todo = await _unitOfWork.Todos.Get(id);
            if (todo == null)
            {
                return NoContent();
            }

            if (todo.UserId != user.Id)
            {
                return BadRequest();
            }

            todo.Active = false;

            await _unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}