using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Infrastructure.Data;
using TodoApi.Core.Domain;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
    [Route("api/[Controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger _logger;

        public TodoController(ITodoRepository todoRepository, ILogger<TodoController> logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;
        }   

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            IEnumerable<TodoItem> model = _todoRepository.GetAllTodos().Select(s => new TodoItem
            {
                Id = s.Id,
                Name = s.Name,
                IsComplete = s.IsComplete
            });
            return model;
        }

        [HttpGet("{id:long}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            Todo todo = _todoRepository.GetTodo(id);
            if (todo == null)
            {
                return NotFound();
            }

            var item = new TodoItem {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete
            };

            return new ObjectResult(item);
        }  

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        ///  
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        /// 
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>New Created Todo Item</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null or invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 201)]
        [ProducesResponseType(typeof(TodoItem), 400)]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            Todo todo = new Todo {
                Id = item.Id,
                Name = item.Name,
                IsComplete = item.IsComplete
            };
            _todoRepository.SaveTodo(todo);
            
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            Todo todo = _todoRepository.GetTodo(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;
            _todoRepository.UpdateTodo(todo);
            
            return new NoContentResult();
        }

        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            Todo todo = _todoRepository.GetTodo(id);
            if (todo == null)
            {
                return NotFound();
            }
            _todoRepository.DeleteTodo(todo);
            
            return new NoContentResult();
        }        
    }
}