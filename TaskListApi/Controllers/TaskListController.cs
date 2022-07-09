using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace TaskListApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TaskListController : ControllerBase {
        private static readonly string filePath = Directory.GetCurrentDirectory()+"\\taskdb.json";
        public TaskListController() {
            List<TaskData> tasklist = new List<TaskData>();
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Length == 0) {
                for (int i = 1; i <= 10; i++) {
                    tasklist.Add(new TaskData() {
                        id = i,
                        text = "Sample Text_" + i.ToString(),
                        day = DateTime.Now.ToString("MMM dd hh:mm tt"),
                        reminder = false
                    });

                }
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    writer.Write(JsonConvert.SerializeObject(tasklist));
                }
            }
        }
        [HttpGet("getTaskList")]
        public async Task<List<TaskData>> getTaskList() {
            List<TaskData> tasklist = new List<TaskData>();
            using (StreamReader reader = new StreamReader(filePath)) {
                tasklist = JsonConvert.DeserializeObject<List<TaskData>>(await reader.ReadToEndAsync());
            }
            return tasklist;
        }
        [HttpDelete("deleteTask/{id}")]
        public async Task deleteTask(int id) {
            var listTask = await this.getTaskList();
            listTask = listTask.Where(c => c.id != id).ToList();

            using (StreamWriter writer = new StreamWriter(filePath, false)) {
               await writer.WriteAsync(JsonConvert.SerializeObject(listTask));
            }
        }
        [HttpPut("toggleReminder")]
        public async Task toggleReminder([FromBody] TaskData task) {
            var listTask = await this.getTaskList();
            var currentTask = listTask.Where(t => t.id == task.id).FirstOrDefault();
            currentTask.reminder = !currentTask.reminder;
            
            using (StreamWriter writer = new StreamWriter(filePath, false)) {
                await writer.WriteAsync(JsonConvert.SerializeObject(listTask));
            }
        }
        [HttpPost("insertTask")]
        public async Task<TaskData> insertTask([FromBody] TaskData task) {
            var listTask = await this.getTaskList();
            task.id = listTask.LastOrDefault().id + 1;
            listTask.Add(task);
            using (StreamWriter writer = new StreamWriter(filePath, false)) {
                await writer.WriteAsync(JsonConvert.SerializeObject(listTask));
            }
            return task;
        }
    }
}
