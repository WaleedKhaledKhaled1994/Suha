using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AD.Client.Helpers
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;
        public HttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        private JsonSerializerOptions defaultJsonSerializerOptions =>
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        #region old
        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHTTP = await httpClient.GetAsync(url);

            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(responseHTTP, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<T>(response, true, responseHTTP);
            }
            else
            {
                var response = await Deserialize<T>(responseHTTP, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<T>(default, false, responseHTTP);
            }
        }
        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {
            var dataJson = System.Text.Json.JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }
        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            var dataJson = System.Text.Json.JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<TResponse>(responseDeserialized, true, response);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, response);
            }
        }
        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data)
        {
            var dataJson = System.Text.Json.JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }
        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var responseHttp = await httpClient.DeleteAsync(url); ;
            return new HttpResponseWrapper<object>(null, responseHttp.IsSuccessStatusCode, responseHttp);
        }
        #endregion

        private static async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(responseString, options);
        }

        #region new
        public async Task<HttpResponseWrapper<T>> Get2<T>(string url)
        {
            var responseHTTP = await httpClient.GetAsync(url);

            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await Deserialize<ApiResponse>(responseHTTP, defaultJsonSerializerOptions);
                var data = JsonConvert.DeserializeObject<T>(response.Data.ToString());

                return new HttpResponseWrapper<T>(data, true, responseHTTP, response.Meta.Message);
            }
            else
            {
                var response = await Deserialize<ApiResponse>(responseHTTP, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<T>(default, false, responseHTTP, response.Meta.Message);
            }
        }

        public async Task<HttpResponseWrapper<TResponse>> Post2<T, TResponse>(string url, T data)
        {
            var dataJson = System.Text.Json.JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                var responseDeserialized = await Deserialize<ApiResponse>(response, defaultJsonSerializerOptions);
                var responseDataDeserialized = JsonConvert.DeserializeObject<TResponse>(responseDeserialized.Data.ToString());

                return new HttpResponseWrapper<TResponse>(responseDataDeserialized, true, response, responseDeserialized.Meta.Message);
            }
            else
                return new HttpResponseWrapper<TResponse>(default, false, response);
        }

        public async Task<HttpResponseWrapper<T>> GetPagination<T>(string url)
        {
            var responseHTTP = await httpClient.GetAsync(url);

            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await Deserialize<ApiPaginationResponse>(responseHTTP, defaultJsonSerializerOptions);
                var data = JsonConvert.DeserializeObject<T>(response.Data.ToString());

                return new HttpResponseWrapper<T>(data, true, responseHTTP, response.Meta.Message);
            }
            else
            {
                var response = await Deserialize<ApiResponse>(responseHTTP, defaultJsonSerializerOptions);
                return new HttpResponseWrapper<T>(default, false, responseHTTP, response.Meta.Message);
            }
        }
        #endregion
    }
}
