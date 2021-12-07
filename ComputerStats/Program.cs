using CpuStats.Data;

while (true)
{
    var temperatureRepository = new WmiCpuTemperatureRepository();
    var temps = temperatureRepository.Get();

    foreach (var temp in temps)
    {
        var instanceName = string.IsNullOrWhiteSpace(temp.InstanceName) ? "Unknown" : temp.InstanceName;

        Console.WriteLine($"{DateTime.Now:T} :: Instance {instanceName}, " +
            $"Temperature C°: {temp.CurrentValueCelsius}, " +
            $"Temperature F°: {temp.CurrentValueCelsius * 5 / 9 + 32}");

        if (temp.HasError)
        {
            Console.WriteLine($"  {temp.Error}");
        }
    }

    await Task.Delay(10_000);
}
