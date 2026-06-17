using TechMove_Global_Logistic_Management_System_ServiceLayer_API.Models;

namespace TechMove_Global_Logistic_Management_System_ServiceLayer_API.Services
{
    public class ContractBusinessService
    {
        public bool CanCreateServiceRequest(ContractModel contract)
        {
            return contract != null && contract.Status == "Active";
        }
    }
}