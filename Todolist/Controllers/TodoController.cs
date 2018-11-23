using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Todolist.Models;

namespace Todolist.Controllers
{
    public class TodoController : Controller
    {
        private TodoContext context;

        public TodoController(TodoContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Todos.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Todo todo)
        {
            context.Todos.Add(todo);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Todo todo = context.Todos.Find(id);

            return View(todo);
        }

        [HttpPost]
        public IActionResult Edit(Todo editedTodo)
        {
            var todo = context.Todos.SingleOrDefault(currentTodo => currentTodo.Id == editedTodo.Id);
            todo.Title = editedTodo.Title;

            context.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult Show(int id)
        {
            Todo todo = context.Todos.Find(id);

            return View(todo);
        }

        public IActionResult Delete(int id)
        {
            context.Todos.Remove(context.Todos.Single(todo => todo.Id == id));
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}