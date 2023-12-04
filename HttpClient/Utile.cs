using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClients
{
	public class Utile
	{
		public static Dictionary<string, List<string>> ExtractErrosFromResponse(string body)
		{
			var response = new Dictionary<string, List<string>>();
			var jsonElement = JsonSerializer.Deserialize<JsonElement>(body);
			var errorsJsonElement = jsonElement.GetProperty("errors");
			foreach (JsonProperty Studentwitherror in errorsJsonElement.EnumerateObject()) 
			{
				var field = Studentwitherror.Name;
				var errors = new List<string>();
				foreach (var errorKind in Studentwitherror.Value.EnumerateArray()) 
                {
					var error = errorKind.GetString();
					errors.Add(error!);
                }

				response.Add(field, errors);
            }
			return response;
		}
	}
}
