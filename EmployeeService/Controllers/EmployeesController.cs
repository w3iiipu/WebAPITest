namespace EmployeeService.Controllers
{
    using EmployeeDataAccess;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Cors;

    // --enables cors for all methods
    [EnableCorsAttribute("http://localhost:64377", "*", "*")]

    // --enable specific controller or method that wants HTTPS
    // [RequireHttps]
    public class EmployeesController : ApiController
    {
        // --use to disable cors for specific methods
        // [DisableCors]
        public HttpResponseMessage Get(string gender = "All")
        {
            using (var entities = new EmployeeDBEntities())
            {
                switch (gender.ToLower())
                {
                    case "all": return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(
                            HttpStatusCode.OK,
                            entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(
                            HttpStatusCode.OK,
                            entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateErrorResponse(
                            HttpStatusCode.BadRequest,
                            "value for gender must be All, Male or Female" + gender + " is invalid.");
                }
            }
        }

        // --decorate with http verb is using custom method names
        // [HttpGet]
        // public IEnumerable<Employee> LoadAllEmployees()
        // {
        // using (var entities = new EmployeeDBEntities())
        // {
        // return entities.Employees.ToList();
        // }
        // }
        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
        {
            using (var entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    // -- returns 404 staus code and relevant message
                    return Request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        "Employee with ID = " + id.ToString() + " not found.");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());

                    // --returns status code 201 + uri of new item
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            using (var entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                try
                {
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        // --[FromBody] and [FromUri] specify where to get the parameters from.
        // --default simple type will get value from request URI while complex type will get value from requestbody
        public HttpResponseMessage Put([FromBody]int id, [FromUri]Employee employee)
        {
            using (var entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                try
                {
                    if (entity != null)
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(
                            HttpStatusCode.NotFound,
                            "Employee with ID = " + id.ToString() + " not found to update ");
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }
    }
}
