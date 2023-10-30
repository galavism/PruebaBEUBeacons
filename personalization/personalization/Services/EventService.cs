using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace personalization.Services
{
    public class EventService
    {
        private readonly HttpClient client;

        public EventService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.0.11:8081/eventosPUJ/")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<Events>> GetEvents()
        {
            List<Events> result = new List<Events>();

            try
            {
                Console.WriteLine("Haciendo solicitud a la API..."); // Mensaje de depuración

                HttpResponseMessage response = await client.GetAsync("eventos");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Respuesta exitosa de la API."); // Mensaje de depuración

                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Contenido de la respuesta de la API:");
                    Console.WriteLine(content);
                    result = JsonConvert.DeserializeObject<List<Events>>(content);
                }
                else
                {
                    Console.WriteLine("Error al obtener eventos: " + response.StatusCode); // Mensaje de depuración
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener eventos: " + ex.Message);
            }

            return result;
        }
    }
}