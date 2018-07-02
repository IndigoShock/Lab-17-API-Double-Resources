using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoListController(TodoContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public ActionResult<List<TodoList>>GetAll()
        {
            return _context.TodoList.ToList();
        }

        [HttpGet("{id}", Name = "GetTodoList")]
        public ActionResult<TodoList> GetByID(int id)
        {
            TodoList listicle = _context.TodoList.Find(id);

            var todoItem = _context.TodoItems.Where(j => j.ListID == id).ToList();

            listicle.ItemsTodo = todoItem;

            if (listicle == null)
            {
                return NotFound();
            }
            return Ok(listicle);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoList listicle)
        {
            _context.TodoList.Add(listicle);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodoList", new {id=listicle.ID}, listicle);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]TodoList listicle)
        {
            var todoList = _context.TodoList.Find(id);

            if (todoList == null)
            {
                return NotFound();
            }

            todoList.Name = listicle.Name;
            todoList.ItemsTodo = listicle.ItemsTodo;

            _context.TodoList.Update(todoList);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            var todoList = _context.TodoList.Find(id);

            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoList.Remove(todoList);

            var todoItem = _context.TodoItems.Where(u => u.ListID == id).ToList();

            foreach (var item in todoItem)
            {
                _context.TodoItems.Remove(item);
            }

            _context.SaveChanges();
            return Ok();
        }

    }
}