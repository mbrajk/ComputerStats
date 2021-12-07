namespace CpuStats.Models
{
    public class CpuTemperature
    {
        public double CurrentValueCelsius { get; init; }
        public string InstanceName { get; init; } = "";
        public bool HasError { get; init; } = false;
        public string Error { get; init; } = "";
    }
}