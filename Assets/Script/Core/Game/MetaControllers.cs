using System;
using cfEngine.Core;
using cfEngine.Meta.Statistic;

public class MetaControllers : IDisposable
{
    public readonly StatisticController Statistic;

    public MetaControllers(UserDataManager userData)
    {
        Statistic = new StatisticController(userData);
    }

    public void Dispose()
    {
        Statistic?.Dispose();
    }
}