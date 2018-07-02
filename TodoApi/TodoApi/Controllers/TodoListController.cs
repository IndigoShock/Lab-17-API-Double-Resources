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

        /// <summary>
        /// this will have the private context be a readable context
        /// </summary>
        /// <param name="context"></param>
        public TodoListController(TodoContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// this will retrieve all of the lists
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<TodoList>>GetAll()
        {
            return _context.TodoList.ToList();
        }

        /// <summary>
        /// this will get a specific list by id. if there is no list, it will come back null and say not found. if found, it will return the ok.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// this will create the list
        /// </summary>
        /// <param name="listicle"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] TodoList listicle)
        {
            _context.TodoList.Add(listicle);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodoList", new {id=listicle.ID}, listicle);
        }

        /// <summary>
        /// this will update the list by id. if the list is not found, it will return not found. but if it does, any input under the name and itemstodo will be changed. updates will be changed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="listicle"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This will delete the list by id. if no list is found, it will return not found. if found, it will delete the list and all of the items attached to it. once deleted, it will return the ok.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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