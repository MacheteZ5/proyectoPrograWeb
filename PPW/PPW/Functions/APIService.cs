using Newtonsoft.Json;
using PPW.Models;
using System.Text;

namespace PPW.Functions
{
    public class APIService
    {
        private static int timeout = 30;
        private static string url = "https://localhost:7145/";

        //User Controller
        public static async System.Threading.Tasks.Task<bool> GetValidationUser(string UserName, int opcion)
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
                return (opcion == 1) ? true : false;
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
                return null;
            }
        }
        public static async System.Threading.Tasks.Task<User> GetUserbyID(int ID)
        {
            var json_ = JsonConvert.SerializeObject(ID);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Users/GetUserbyID", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }
        public static async System.Threading.Tasks.Task<IEnumerable<User>> GetAllUsers()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.GetAsync(url + "Users/GetAllUsers");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<User>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }
        public static async System.Threading.Tasks.Task<bool> SetUser(User user)
        {
            var json_ = JsonConvert.SerializeObject(user);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Users/CreateUser", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return false;
            }
        }
        public static async System.Threading.Tasks.Task<bool> updateUser(User user)
        {
            var json_ = JsonConvert.SerializeObject(user);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PutAsync(url + "Users/UpdateUser", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return false;
            }
        }
        public static async System.Threading.Tasks.Task<bool> DisableUser(User user)
        {
            var json_ = JsonConvert.SerializeObject(user);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PutAsync(url + "Users/DisableUser", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return false;
            }
        }

        //Contact Controller
        public static async System.Threading.Tasks.Task<IEnumerable<Contact>> GetContactsList()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.GetAsync(url + "Contacts/GetAllContacts");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Contact>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
        public static async System.Threading.Tasks.Task<Contact> GetContact(Contact contact)
        {
            var json_ = JsonConvert.SerializeObject(contact);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Contacts/GetContact", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Contact>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }
        public static async System.Threading.Tasks.Task<Contact> GetContactById(int contactId)
        {
            var json_ = JsonConvert.SerializeObject(contactId);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Contacts/GetContactById", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Contact>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }
        public static async System.Threading.Tasks.Task<bool> SetContact(Contact contact)
        {
            var json_ = JsonConvert.SerializeObject(contact);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Contacts/SetContact", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return false;
            }
        }
        public static async System.Threading.Tasks.Task<bool> DeleteContact(Contact contact)
        {
            var json_ = JsonConvert.SerializeObject(contact);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Contacts/DeleteContact", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return false;
            }
        }

        //Chat Controller
        public static async System.Threading.Tasks.Task<bool> SetChat(Chat chat)
        {
            var json_ = JsonConvert.SerializeObject(chat);
            var content = new StringContent(json_, Encoding.UTF8, "application/json");
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.PostAsync(url + "Chats/SetChat", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return false;
            }
        }
    }
}
