using TechMove_Global_Logistic_Management_System.Models;

namespace TechMove_Global_Logistic_Management_System.Services
{
    public class ContractBusinessService
    {
        public bool CanCreateServiceRequest(ContractModel contract)
        {
            return contract != null && contract.Status == "Active";
        }
    }
}