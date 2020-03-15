using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class ExchangeTask : Singleton<ExchangeTask>
    {

        public void Exchange(Panel origin, Panel target, string taskName) {

            if (origin == target) return;

            TaskData taskData = origin.ExtractTask(taskName);

            target.AddTask(taskData);

            origin.Relayout();

            target.Relayout();

        }

    }
}
