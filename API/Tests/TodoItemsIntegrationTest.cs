using API.Data;
using API.Data.SeedData;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Tests
{
    public class TodoItemsIntegrationTest
    {
        private TestServer _server;

        public HttpClient Client { get; private set; }

        public TodoItemsIntegrationTest()
        {
            SetUpClient();
        }

        private void SetUpClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<API.Startup>()
                .ConfigureServices(services =>
                {
                    var context = new TodoContext(new DbContextOptionsBuilder<TodoContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(TodoContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }


        private async Task SeedData()
        {

            var testData = new List<Todo>() {
            new() { Task = "Task true",Done = true},
            new() { Task = "post 2",Done = false},
            new() { Task = "post 3",Done = false},
            new() { Task = "post 4",Done = false},
            new() { Task = "Task false",Done = false}
            };

            foreach (var item in testData)
            {
                var payload = GenerateCreateForm(item.Task, item.Done);
                var postResponse = await Client.PostAsync("/api/todoitems", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
                postResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status201Created);

            }

        }

        private CreateTodoForm GenerateCreateForm(string task, bool done)
        {
            return new CreateTodoForm()
            {
                Id = Guid.NewGuid(),
                Task = task,
                Done = done,
            };
        }
        private UpdateTodoForm GenerateUpdateForm(string task, bool done)
        {
            return new UpdateTodoForm()
            {
                Task = task,
                Done = done,
            };
        }

        [Fact]
        public async Task TestGetEndPointSpeed()
        {
            await SeedData();

            Stopwatch sw = new();
            sw.Start();
            var getResponse = await Client.GetAsync("/api/todoitems");
            getResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);
            sw.Stop();
            var todos = JsonConvert.DeserializeObject<IEnumerable<Todo>>(getResponse.Content.ReadAsStringAsync().Result);
            todos.Count().Should().Be(5);
            sw.ElapsedMilliseconds.Should().BeLessThan(2000);
        }

        [Fact]
        public async Task TestGetEndPointSpeedFail()
        {
            await SeedData();

            var getResponse = await Client.GetAsync("/api/v1/todo");
            getResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status404NotFound);
   
        }

        [Fact]
        public async Task TestingFlowGetAndDelete()
        {
            await SeedData();

            var getResponse = await Client.GetAsync("/api/todoitems");
            getResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);
            var todos = JsonConvert.DeserializeObject<IEnumerable<Todo>>(getResponse.Content.ReadAsStringAsync().Result);
            todos.Count().Should().BeGreaterThan(4);

            var deleteResponse = await Client.DeleteAsync($"/api/todoitems/{todos.FirstOrDefault().Id}");
            deleteResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status204NoContent);

            var verifyIfDataWasDeleted = await Client.GetAsync($"/api/todoitems/{todos.FirstOrDefault().Id}");
            verifyIfDataWasDeleted.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status404NotFound);

            var ValidateAllData = await Client.GetAsync("/api/todoitems");
            ValidateAllData.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);
            var newTodos = JsonConvert.DeserializeObject<IEnumerable<Todo>>(ValidateAllData.Content.ReadAsStringAsync().Result);
            newTodos.Count().Should().BeLessThanOrEqualTo(4);
        }

        [Fact]
        public async Task TestingFlowGetAndUpdate()
        {
            await SeedData();

            var getData = await Client.GetAsync("/api/todoitems");
            getData.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);
            var todos = JsonConvert.DeserializeObject<IEnumerable<Todo>>(getData.Content.ReadAsStringAsync().Result);
            todos.Count().Should().BeGreaterThan(4);

            var updateTodo = todos.FirstOrDefault();
            var updateForm = GenerateUpdateForm(updateTodo.Task, true);
            var updateResponse = await Client.PutAsync($"/api/todoitems/{updateTodo.Id}", new StringContent(JsonConvert.SerializeObject(updateForm), Encoding.UTF8, "application/json"));
            updateResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);

            var confirmDataWasUpdated = await Client.GetAsync($"/api/todoitems/{updateTodo.Id}");
            confirmDataWasUpdated.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);
            Todo? todoUpdated = JsonConvert.DeserializeObject<Todo>(confirmDataWasUpdated.Content.ReadAsStringAsync().Result);
            todoUpdated.Done.Should().BeTrue();
           
        }

    }
}
