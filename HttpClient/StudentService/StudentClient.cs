using DB_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClients.StudentService
{
	public interface IStudentClient
	{
		Task<List<Student>> GetStudent();
	}

	public class StudentClient : IStudentClient
	{
		private readonly HttpClient _httpClient;
		private readonly string url = "https://localhost:7255/api/Students";

		public StudentClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<List<Student>> GetStudent()
		{
			return await _httpClient.GetFromJsonAsync<List<Student>>(url);
		}
	}
}
