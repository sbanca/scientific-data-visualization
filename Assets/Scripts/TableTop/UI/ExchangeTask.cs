using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class ExchangeTask : Singleton<ExchangeTask>
    {

        public void Exchange(Pannel origin, Pannel target, string taskName) {

            PannelTask task = origin.ExtractTask(taskName);

            target.AddTask(task);

            origin.Relayout();

            target.Relayout();

        }

    }
}
