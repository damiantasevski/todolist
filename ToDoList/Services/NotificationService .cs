using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using ToDoList.Models;
using ToDoList.SignalR;

namespace ToDoList.Services
{
    public class NotificationService : IHostedService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private Timer _timer;

        public NotificationService(IHubContext<NotificationHub> hubContext, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendNotifications, null, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void SendNotifications(object state)
        {
            var tasks = GetTasksForToday();
            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    var message = $"TasksList for today: {task.Name}";
                    await _hubContext.Clients.All.SendAsync("NewNotification", message);
                }
            }
        }

        private List<TasksListViewModel> GetTasksForToday()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Data.TdlDbContext>();

            var tasks = dbContext.TasksLists
                .Where(x => x.Date == today && x.Tasks.Any(task => !task.IsDone))
                .Include(x => x.Tasks)
                .ToList();

            return _mapper.Map<List<TasksListViewModel>>(tasks);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
