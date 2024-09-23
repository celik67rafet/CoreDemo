using BlogApiDemo.DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]   
        public IActionResult Index()
        {
            return Content("Index Here");
        }

        [HttpGet]
        public IActionResult EmployeeList()
        {
            using var c = new Context();

            var values = c.Employees.ToList();

            return Ok(values);
            
        }

        [HttpPost]
        public IActionResult EmployeeAdd( Employee employee ) 
        {
            using var c = new Context();

            Console.WriteLine( "Gelen ID sıfır mı?: " + employee.Id );

            if( employee.Id != 0 ){

                return BadRequest(new { message = "Lütfen Id değeri göndermeyiniz, Id sıfır olmalı." });

            }
            else
            {
                c.Add(employee);

                int result = c.SaveChanges();

                if (result > 0)
                {

                    return Ok(new { message = "Ekleme Başarılı" });

                }
                else
                {
                    return BadRequest(new { message = "Ekleme Başarısız" });
                }
            }

        }

        [HttpGet("{id}")]
        public IActionResult EmployeeGet( int id)
        {
            using var c = new Context();

            var employee = c.Employees.Find(id);

            if( employee != null) 
            {
            
                return Ok( employee );

            }
            else
            {

                return NotFound();

            }

        }

        [HttpDelete("{id}")]
        public IActionResult EmployeeDelete( int id ) 
        {
            using var c = new Context();

            var employee = c.Employees.Find(id);

            if( employee != null ) 
            {

                c.Remove(employee);

                int result = c.SaveChanges();

                return Ok(new { message = "Silme işlemi başarılı, kontrol edin." });

            }
            else
            {
                return NotFound();
            }

        }

        [HttpPut]
        public IActionResult EmployeeUpdate( Employee employee ) 
        {
            using var c = new Context();

            var emp = c.Find<Employee>(employee.Id);

            if( emp != null) 
            {
                emp.Name = employee.Name;
                
                c.Update( emp );

                c.SaveChanges();

                return Ok( new { message = "Güncelleme başarılı, kontrol edin." } );
            }
            else
            {
                return NotFound();
            }
        }
    }
}
