using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TsinghuaNet
{
    /// <summary>
    /// Exposes methods to login, logout and get flux from the login page.
    /// </summary>
    public interface IConnect : IDisposable
    {
        /// <summary>
        /// Login to the network.
        /// </summary>
        /// <returns>The response of the website, may be a sentense or a html page.</returns>
        Task<LogResponse> LoginAsync();
        /// <summary>
        /// Logout from the network.
        /// </summary>
        /// <returns>The response of the website, may be a sentense or a html page.</returns>
        Task<LogResponse> LogoutAsync();
        /// <summary>
        /// Get information of the user online.
        /// </summary>
        /// <returns>An instance of <see cref="FluxUser"/> class of the current user.</returns>
        Task<FluxUser> GetFluxAsync();
    }
    /// <summary>
    /// Base class of net helpers.
    /// </summary>
    public abstract class NetHelperBase : IDisposable
    {
        private HttpClient client;
        /// <summary>
        /// The username to login.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The password to login.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="NetHelperBase"/> class.
        /// </summary>
        public NetHelperBase()
            : this(string.Empty, string.Empty, null)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NetHelperBase"/> class.
        /// </summary>
        /// <param name="client">A user-specified instance of <see cref="HttpClient"/>.</param>
        public NetHelperBase(HttpClient client)
            : this(string.Empty, string.Empty, client)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NetHelperBase"/> class.
        /// </summary>
        /// <param name="username">The username to login.</param>
        /// <param name="password">The password to login.</param>
        public NetHelperBase(string username, string password)
            : this(username, password, null)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NetHelperBase"/> class.
        /// </summary>
        /// <param name="username">The username to login.</param>
        /// <param name="password">The password to login.</param>
        /// <param name="client">A user-specified instance of <see cref="HttpClient"/>.</param>
        public NetHelperBase(string username, string password, HttpClient client)
        {
            Username = username;
            Password = password;
            this.client = client ?? new HttpClient();
        }
        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected async Task<string> PostAsync(string uri)
        {
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                using (HttpResponseMessage response = await client.SendAsync(message))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="data">The HTTP request string content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected async Task<string> PostAsync(string uri, string data)
        {
            using (StringContent content = new StringContent(data ?? string.Empty, Encoding.ASCII, "application/x-www-form-urlencoded"))
            {
                using (HttpResponseMessage response = await client.PostAsync(uri, content))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <param name="data">The HTTP request dictionary content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected async Task<string> PostAsync(string uri, Dictionary<string, string> data)
        {
            using (HttpContent content = new FormUrlEncodedContent(data))
            {
                using (HttpResponseMessage response = await client.PostAsync(uri, content))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="uri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected Task<string> GetAsync(string uri)
        {
            return client.GetStringAsync(uri);
        }
        #region IDisposable Support
        private bool disposedValue = false;
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="HttpClient"/> and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to releases only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }
                disposedValue = true;
            }
        }
        /// <summary>
        /// Releases the unmanaged resources and disposes of the managed resources used by the <see cref="HttpClient"/>.
        /// </summary>
        public void Dispose() => Dispose(true);
        #endregion
    }
}
