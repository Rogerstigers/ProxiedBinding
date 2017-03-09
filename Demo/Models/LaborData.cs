using Newtonsoft.Json;

namespace Demo.Models
{
    public class LaborData
    {
        public LaborData()
        {
        }

        public LaborData(string company, string department, decimal averageSalary, int numberOfEmployees)
        {
            Company = company;
            Department = department;
            AverageSalary = averageSalary;
            NumberOfEmployees = numberOfEmployees;
        }

        [JsonProperty]
        public string Company { get; private set; }
        [JsonProperty]
        public string Department { get; private set; }
        [JsonProperty]
        public decimal AverageSalary { get; private set; }
        [JsonProperty]
        public int NumberOfEmployees { get; private set; }
    }
}
