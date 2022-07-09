using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskListApi {
    public class TaskData {
        public int id { get; set; }
        public string text { get; set; }
        public string day { get; set; }
        public bool reminder { get; set; }
    }
}
