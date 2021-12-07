using CpuStats.Models;
using System.Management;

namespace CpuStats.Data
{
    public class WmiCpuTemperatureRepository
    {
        private const string CurrentTemperature = "CurrentTemperature";
        private const double MinimumTemperatureCelsius = -273.15;

        public IEnumerable<CpuTemperature> Get()
        {
            var managementObjectSearcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
            var temperatures = new List<CpuTemperature>();

            ManagementObjectCollection managementObjects;
            try
            {
                managementObjects = managementObjectSearcher.Get();

                foreach (var managementObject in managementObjects)
                {
                    var instanceName = "";

                    try
                    {
                        instanceName = managementObject["InstanceName"]?.ToString() ?? "";
                    }
                    catch { };

                    try
                    {
                        var managementObjectString = managementObject[CurrentTemperature].ToString();
                        var temperatureKelvin = Convert.ToDouble(managementObjectString);

                        var temperatureCelsius = (temperatureKelvin - 2732) / 10.0;

                        temperatures.Add(new CpuTemperature
                        {
                            CurrentValueCelsius = temperatureCelsius,
                            InstanceName = instanceName
                        });
                    }
                    catch (Exception ex)
                    {
                        temperatures.Add(new CpuTemperature
                        {
                            CurrentValueCelsius = MinimumTemperatureCelsius,
                            HasError = true,
                            Error = $"Unable to process CPU Temperature: {ex.Message}",
                            InstanceName = instanceName
                        });
                    }
                }

                return temperatures;
            }
            catch(Exception ex)
            {
                temperatures.Add(new CpuTemperature
                {
                    CurrentValueCelsius = MinimumTemperatureCelsius,
                    Error = $"Unable to access CPU Temperature: {ex.Message}",
                    HasError = true
                });

                return temperatures;
            }
        }
    }
}
