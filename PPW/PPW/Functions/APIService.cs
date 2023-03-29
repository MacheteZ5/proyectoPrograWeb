using Newtonsoft.Json;
using System.Text;
using PPW.Models;

namespace PPW.Functions
{
    public static class APIService
    {
        private static int timeout = 30;
        private static string url = "https://localhost:7025/";

        //Movie Controller
        public static async System.Threading.Tasks.Task<IEnumerable<User>> GetUsersList()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.GetAsync(url + "Movie/GetList");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<User>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public static async System.Threading.Tasks.Task<bool> GetValidationUser(string UserName)
        {
            var json_ = JsonConvert.SerializeObject(UserName);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Users/GetValidationUser", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return false;
            }
        }
        public static async System.Threading.Tasks.Task<User> GetUser(string UserName)
        {
            var json_ = JsonConvert.SerializeObject(UserName);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Users/GetUser", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
        public static async System.Threading.Tasks.Task<bool> SetUser(User user)
        {
            var json_ = JsonConvert.SerializeObject(user);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Users/SetUser", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
        public static async System.Threading.Tasks.Task<bool> UpdateMovie(User movie)
        {
            var json_ = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Movie/Update", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public static async System.Threading.Tasks.Task<bool> DeleteMovie(User movie)
        {
            var json_ = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Movie/Delete", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
