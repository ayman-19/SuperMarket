using DB_Core.Models;
using HttpClients.StudentService;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace HttpClients
{

	internal class Program
	{
		static async Task Main(string[] args)
		{
			//await Get();
			//await Post();
			//await SendAsync();
			//await Update();
			//await Delete();
			//await ClientFactory();
			await ServiceClient();


		}
		static async Task Get()
		{
			var url = "https://localhost:7255/api/Categories/GetAll";
			using (var httpclient = new HttpClient())
			{
				var response = await httpclient.GetAsync(url);
				var response2 = await httpclient.GetStringAsync(url);
				response.EnsureSuccessStatusCode();

				if (response.IsSuccessStatusCode)
				{
					var responseQuery = await response.Content.ReadAsStringAsync();
					var categories = JsonSerializer.Deserialize<List<Category>>(responseQuery,
						new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

					var categories2 = JsonSerializer.Deserialize<List<Category>>(response2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

					var categories1 = JsonSerializer.Serialize(categories);
				}

			}
		}
		static async Task Post()
		{
			var url = "https://localhost:7255/api/Students";
			using (var httpclient = new HttpClient())
			{
				var res = await httpclient.PostAsJsonAsync(url, new Student
				{
					Name = "Ayman",
					Id = 5
				});
				if(res.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					var body = await res.Content.ReadAsStringAsync();
					var errors = Utile.ExtractErrosFromResponse(body);
					foreach (var error in errors)
					{
						await Console.Out.WriteAsync($"{error.Key} => ");
                        foreach (var errorsList in error.Value)
                        {
						await Console.Out.WriteAsync($"{errorsList}, ");
						}
                        await Console.Out.WriteLineAsync();
                    }

                }
				else if(res.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var response = await res.Content.ReadAsStringAsync();
					var students = JsonSerializer.Deserialize<List<Student>>(response,
						new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    foreach (var student in students!)
                    {
                        await Console.Out.WriteLineAsync($"{student.Id} => {student.Name}");
                    }
                }
			}
		}
		static async Task SendAsync()
		{
			var url = "https://localhost:7255/api/Students";
			using (var httpclient = new HttpClient())
			{
				using(var reqmassage = new HttpRequestMessage(HttpMethod.Get, url))
				{
					reqmassage.Headers.Add("studentamount", "3");

					var response = await httpclient.SendAsync(reqmassage);
					var readasync = await response.Content.ReadAsStringAsync();
					var sts = JsonSerializer.Deserialize<List<Student>>(readasync, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				}
				var suds = await httpclient.GetFromJsonAsync<List<Student>>(url, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


				httpclient.DefaultRequestHeaders.Add("studentamount", "4");
				var suds2 = await httpclient.GetFromJsonAsync<List<Student>>(url, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			}
		}
		static async Task Update()
		{
			var url = "https://localhost:7255/api/Students";
			using (var httpclient = new HttpClient())
			{
				var std = new Student { Id = 19, Name = "Aymoon" };
				await httpclient.PutAsJsonAsync($"{url}/{1}", std);

				var stds = await httpclient.GetFromJsonAsync<List<Student>>(url);
			}
		}
		static async Task Delete()
		{
			var url = "https://localhost:7255/api/Students";
			using (var httpclient = new HttpClient())
			{
				await httpclient.DeleteAsync($"{url}/{19}");
				var stds = await httpclient.GetFromJsonAsync<List<Student>>(url);

			}
		}


		static async Task ClientFactory()
		{
			var url = "https://localhost:7255/api/Students";

			var serviceCollection = new ServiceCollection();
			ConfigureService(serviceCollection);
			var service = serviceCollection.BuildServiceProvider();
			var httpclientFactory = service.GetRequiredService<IHttpClientFactory>();
			var httpClient = httpclientFactory.CreateClient();
			var response = await httpClient.GetStringAsync(url);
			var stds = JsonSerializer.Deserialize<List<Student>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			var httpclient2 = httpclientFactory.CreateClient("Student");
			var res = await httpclient2.GetStringAsync("");
			var stds2 = JsonSerializer.Deserialize<List<Student>>(res, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			var httpclient3 = httpclientFactory.CreateClient("Student");
			var res2 = await httpclient2.GetStringAsync("");
			var stds3 = await httpclient3.GetFromJsonAsync<List<Student>>("", new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			var http = service.GetRequiredService<IStudentClient>();
			var res5 = await http.GetStudent();


		}
		static async Task ServiceClient()
		{
			var servicecollection = new ServiceCollection();
			ConfigureService(servicecollection);
			var services = servicecollection.BuildServiceProvider();
			var httpclient = services.GetRequiredService<IStudentClient>();
			var stds = await httpclient.GetStudent();
		}
		private static void ConfigureService(ServiceCollection service)
		{
			service.AddHttpClient();
			service.AddHttpClient("Student", o =>
			{
				o.BaseAddress = new Uri("https://localhost:7255/api/Students");
				o.DefaultRequestHeaders.Add("studentamount", "1");
			});
			service.AddHttpClient<IStudentClient, StudentClient>();
		}
	}
}