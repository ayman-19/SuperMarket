using DB_Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperMarket.Controllers
{
	[Route("api/Students")]
	[ApiController]
	public class StudentsController : ControllerBase
	{

		static List<Student> students = new List<Student>() { new Student { Id = 1,Name = "Ayman"},
		new Student { Id = 2,Name = "Ahmed"},new Student { Id = 3,Name = "Eslam"},new Student { Id = 4,Name = "Moamen"}};
		[HttpPost]
		public IActionResult GetAll(Student s)
		{
			return Ok(students);
		}
		[HttpGet]
		public IEnumerable<Student> GetStudent([FromHeader] int studentamount = 2)
		{
			return students.Take(studentamount);
		}
		[HttpPost("Create")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult Post([FromBody] Student student)
		{
			students.Add(student);
			return Ok(student);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Student student)
		{
			if (!students.Any(x => x.Id == id))
				return NotFound();
			var std = students.FirstOrDefault(x => x.Id == id);
			if (std != null)
			{
				std.Name = student.Name;
				std.Id = student.Id;
			}
			return Ok(students);
		}
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			if (!students.Any(x => x.Id == id))
				return NotFound();

			var std = students.FirstOrDefault(y => y.Id == id);
			if (std != null)
				students.Remove(std);
			return Ok(students);
		}
	}
}
