using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Data.Entities;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TasksListsController : Controller
    {
        private readonly TdlDbContext _context;
        private readonly IMapper _mapper;

        public TasksListsController(TdlDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(DateOnly? date)
        {
            if (date == null)
            {
                date = DateOnly.FromDateTime(DateTime.Now);
            }

            ViewBag.Date = date;

            var tasks = await _context.TasksLists.Where(x => x.Date == date).ToListAsync();
            return View(_mapper.Map<List<TasksListViewModel>>(tasks));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksList = await _context.TasksLists.Include(x => x.Tasks).FirstOrDefaultAsync(m => m.Id == id);
            if (tasksList == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<TasksListViewModel>(tasksList));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TasksListViewModel tasksList)
        {
            if (ModelState.IsValid)
            {
                var tasksListEntity = _mapper.Map<TasksList>(tasksList);
                tasksListEntity.OrderTasks();

                _context.TasksLists.Add(tasksListEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tasksList);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksList = await _context.TasksLists.Where(x => x.Id == id).
                Include(x => x.Tasks.OrderBy(x => x.Order)).FirstOrDefaultAsync();

            if (tasksList == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<TasksListViewModel>(tasksList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TasksListViewModel tasksListViewModel)
        {
            if (id != tasksListViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var tasksList = _mapper.Map<TasksList>(tasksListViewModel);
                tasksList.OrderTasks();

                var existingTaskListEntity = await _context.TasksLists.FirstOrDefaultAsync(x => x.Id == tasksList.Id);

                if (existingTaskListEntity == null)
                {
                    return NotFound();
                }

                existingTaskListEntity.Name = tasksList.Name;
                existingTaskListEntity.Date = tasksList.Date;

                _context.TasksLists.Update(existingTaskListEntity);

                if (tasksList.Tasks != null && tasksList.Tasks.Count > 0)
                {
                    var tasksToRemove = _context.Tasks.Where(x => x.TasksListId == existingTaskListEntity.Id);
                    _context.Tasks.RemoveRange(tasksToRemove);
                    _context.SaveChanges();

                    foreach (var taskToAdd in tasksList.Tasks)
                    {
                        taskToAdd.TasksListId = existingTaskListEntity.Id;
                        _context.Tasks.Add(taskToAdd);
                    }
                }
                
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(tasksListViewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksList = await _context.TasksLists
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tasksList == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<TasksListViewModel>(tasksList));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tasksList = await _context.TasksLists.FindAsync(id);
            if (tasksList != null)
            {
                _context.TasksLists.Remove(tasksList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public async Task<IActionResult> AddTaskToTasksList(TasksListViewModel tasksList)
        {
            tasksList.Tasks.Add(new TaskViewModel() { Id = Guid.NewGuid()});
            return PartialView("_TasksList", tasksList);
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveTaskFromTasksList([FromForm]Guid taskId,[FromForm] TasksListViewModel tasksList)
        {
            var taskToRemove = tasksList.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (taskToRemove != null)
            {
                tasksList.Tasks.Remove(taskToRemove);
            }

            return PartialView("_TasksList", tasksList);
        }
    }
}
