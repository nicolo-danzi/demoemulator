#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using QPlatform.NativeUI;
using QPlatform.HMIProject;
using QPlatform.UI;
using QPlatform.Core;
using QPlatform.CoreBase;
using QPlatform.NetLogic;
using QPlatform.Store;
using QPlatform.Datalogger;
using QPlatform.SQLiteStore;
#endregion

public class RuntimeNetLogic1 : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        variable = LogicObject.GetVariable("Variable");
        task = new PeriodicTask(UpdateVariable, 100, LogicObject);
        task.Start();
        task2 = new PeriodicTask(UpdateCharts, 5000, LogicObject);
        task2.Start();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    private void UpdateVariable()
    {
        int value = variable.Value;
        if (!descending)
        {
            if (value < max)
                variable.Value = value + step;
            else
            {
                variable.Value = value - step;
                descending = true;
            }
        }
        else
        {
            if (value > min)
                variable.Value = value - step;
            else
            {
                variable.Value = value + step;
                descending = false;
            }
        }
    }

    private void UpdateCharts()
    {
        var model = Project.Current.Get("Model");
        model.Children.Clear();
        var n = r.Next(3, 7);
        for (var i = 0; i < n; ++i)
        {
            var v = InformationModel.MakeVariable("value" + i, OpcUa.DataTypes.Int32);
            v.Value = r.Next(5, 100);
            model.Add(v);
        }
    }

    bool descending = false;
    int max = 100;
    int min = -100;
    int step = 4;
    IUAVariable variable;
    PeriodicTask task;
    PeriodicTask task2;
    Random r = new Random();
}
