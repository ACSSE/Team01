using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IcebreakServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IIEmployeeService" in both code and config file together.
    [ServiceContract]
    public interface IEmployeeService
    {
        [WebGet(UriTemplate = "Employee", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Employee> GetAllEmployeeDetails();

        [WebGet(UriTemplate = "Employee?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Employee GetEmployee(int Id);

        [WebInvoke(Method = "POST", UriTemplate = "EmployeePOST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        void AddEmployee(Employee newEmp);

        [WebInvoke(Method = "PUT", UriTemplate = "EmployeePUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        void UpdateEmployee(Employee newEmp);

        [WebInvoke(Method = "DELETE", UriTemplate = "Employee/{empId}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        void DeleteEmployee(string empId);
    }
}
