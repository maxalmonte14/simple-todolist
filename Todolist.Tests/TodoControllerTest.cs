using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todolist.Controllers;
using Todolist.Models;
using Xunit;

namespace Todolist.Tests.Controllers
{
    public class TodoControllerTest : IDisposable
    {
        private TodoController controller;

        private TodoContext todoContext;

        public TodoControllerTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();

            optionsBuilder.UseInMemoryDatabase("test");

            todoContext = new TodoContext(optionsBuilder.Options);

            todoContext.Todos.Add(new Todo { Id = 1, Title = "Go to the supermarket" });
            todoContext.SaveChanges();

            controller = new TodoController(todoContext);
        }

        public void Dispose()
        {
            var todo = todoContext.Todos.SingleOrDefault(currentTodo => currentTodo.Id == 1);

            if (todo == null) {
                return;
            }

            todoContext.Todos.Remove(todo);
            todoContext.SaveChanges();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithAListOfTodos_WhenSucceeded()
        {
            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsAssignableFrom<List<Todo>>(viewResult.Model);
        }

        [Fact]
        public void Create_ReturnsViewResult_WhenSucceeded()
        {
            var result = controller.Create();
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void PostCreate_RedirectsToIndex_WhenSucceeded()
        {
            var result = controller.Create(new Todo { Id = 2, Title = "Go to the supermarket" });
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_ReturnsViewResult_WithATodo_WhenSucceeded()
        {
            var result = controller.Edit(1);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsAssignableFrom<Todo>(viewResult.Model);
        }

        [Fact]
        public void PostEdit_RedirectsToIndex_WhenSucceeded()
        {
            var result = controller.Edit(new Todo { Id = 1, Title = "Buy Gas" });
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Show_ReturnsViewResult_WithATodo_WhenSucceeded()
        {
            var result = controller.Show(1);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsAssignableFrom<Todo>(viewResult.Model);
        }

        [Fact]
        public void Delete_RedirectsToIndex_WhenSucceeded()
        {
            var result = controller.Delete(1);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}