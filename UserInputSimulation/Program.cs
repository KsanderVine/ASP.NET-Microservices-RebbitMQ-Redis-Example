using System.Text;
using System.Text.Json;

namespace UserInputSimulation
{
    internal class Program
    {
        public class User
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;

        }

        public class Film
        {
            public string Id { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
        }

        public const string UsersServiceUrl = "http://users/api/users";
        public const string FilmsServiceUrl = "http://films/api/films";
        public const string CommentsServiceUrl = "http://comments/api/comments";
        public const string RatingsServiceUrl = "http://ratings/api/ratings";

        public const int PreSimulationDelayMs = 5000;
        public const int SimulationDelayMs = 1000;
        public const int RetryRequestDelayMs = 1000;

        static async Task Main(string[] args)
        {
            using HttpClient httpClient = new();

            string? userId, firstFilmId, secondFilmId;
            userId = firstFilmId = secondFilmId = string.Empty;

            JsonSerializerOptions serializerOptions = new()
            {
                PropertyNameCaseInsensitive = true,
            };

            Console.WriteLine("--> Simulation started...");
            await Task.Delay(PreSimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> User create request...");

                while (isContinued)
                {
                    var serialized = JsonSerializer.Serialize(new { Name = "Elon", Surname = "Musk" }, serializerOptions);
                    var httpContent = new StringContent(serialized, Encoding.UTF8, "application/json"); 
                    var response = await httpClient.PostAsync($"{UsersServiceUrl}/create", httpContent);
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"--> Content as string: {content} ");

                        var user = JsonSerializer.Deserialize<User>(content, serializerOptions);
                        userId = user!.Id;
                        isContinued = false;

                        Console.WriteLine($"--> User Id for this simulation: {userId}");
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> First film create request...");

                while (isContinued)
                {
                    var serialized = JsonSerializer.Serialize(new { Title = "Simulation: 2023" }, serializerOptions);
                    var httpContent = new StringContent(serialized, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{FilmsServiceUrl}/create", httpContent);
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"--> Content as string: {content} ");

                        var film = JsonSerializer.Deserialize<Film>(content, serializerOptions);
                        firstFilmId = film!.Id;
                        isContinued = false;

                        Console.WriteLine($"--> First Film Id for this simulation: {firstFilmId}");
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> Second film create request...");

                while (isContinued)
                {
                    var serialized = JsonSerializer.Serialize(new { Title = "Simulation: 2023 - Aftermath" }, serializerOptions);
                    var httpContent = new StringContent(serialized, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{FilmsServiceUrl}/create", httpContent);
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"--> Content as string: {content} ");

                        var film = JsonSerializer.Deserialize<Film>(content, serializerOptions);
                        secondFilmId = film!.Id;
                        isContinued = false;

                        Console.WriteLine($"--> Second Film Id for this simulation: {secondFilmId}");
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> First comment create request...");

                while (isContinued)
                {
                    var serialized = JsonSerializer.Serialize(new { FilmId = firstFilmId.ToString(), UserId = userId.ToString(), Text = "Great movie!" }, serializerOptions);
                    var httpContent = new StringContent(serialized, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{CommentsServiceUrl}/create", httpContent);
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"--> Content as string: {content} ");
                        isContinued = false;
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> Second comment create request...");

                while (isContinued)
                {
                    var serialized = JsonSerializer.Serialize(new { FilmId = secondFilmId.ToString(), UserId = userId.ToString(), Text = "No so great, but mostly great movie!" }, serializerOptions);
                    var httpContent = new StringContent(serialized, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{CommentsServiceUrl}/create", httpContent);
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"--> Content as string: {content} ");
                        isContinued = false;
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> First rating create request...");

                while (isContinued)
                {
                    var serialized = JsonSerializer.Serialize(new { FilmId = firstFilmId.ToString(), UserId = userId.ToString(), Rate = 99 }, serializerOptions);
                    var httpContent = new StringContent(serialized, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{RatingsServiceUrl}/create", httpContent);
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"--> Content as string: {content} ");
                        isContinued = false;
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> Second rating create request...");

                while (isContinued)
                {
                    var serialized = JsonSerializer.Serialize(new { FilmId = secondFilmId.ToString(), UserId = userId.ToString(), Rate = 78 }, serializerOptions);
                    var httpContent = new StringContent(serialized, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{RatingsServiceUrl}/create", httpContent);
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"--> Content as string: {content} ");
                        isContinued = false;
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> Deleting first film...");

                while (isContinued)
                {
                    var response = await httpClient.DeleteAsync($"{FilmsServiceUrl}/delete/{firstFilmId}");
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        isContinued = false;
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> Deleting user...");

                while (isContinued)
                {
                    var response = await httpClient.DeleteAsync($"{UsersServiceUrl}/delete/{userId}");
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        isContinued = false;
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> Getting only one film left...");

                while (isContinued)
                {
                    var response = await httpClient.GetAsync($"{FilmsServiceUrl}/{secondFilmId}");
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"--> Content as string: {content} ");
                        isContinued = false;
                    }
                    else
                    {
                        Console.WriteLine($"--> Attemptt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            await Task.Delay(SimulationDelayMs);
            Console.WriteLine();

            {
                bool isContinued = true;
                int attempt = 0;
                Console.WriteLine("--> Deleting second film...");

                while (isContinued)
                {
                    var response = await httpClient.DeleteAsync($"{FilmsServiceUrl}/delete/{secondFilmId}");
                    Console.WriteLine($"--> Response: [{response.StatusCode}] ");

                    if (response.IsSuccessStatusCode)
                    {
                        isContinued = false;
                    }
                    else
                    {
                        Console.WriteLine($"--> Attempt [{++attempt}]...");
                        await Task.Delay(RetryRequestDelayMs);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("--> Simulation ended...");
        }
    }
}