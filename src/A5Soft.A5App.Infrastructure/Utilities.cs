using A5Soft.CARMA.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace A5Soft.A5App.Infrastructure
{
    internal static class Utilities
    {

        private static readonly HttpClient client = new HttpClient(new HttpClientHandler()
        { UseCookies = false, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            , AllowAutoRedirect = false });
        private static readonly ConcurrentDictionary<string, string> dnsDictionary =
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);


        public static async Task<(string Response, Dictionary<string, string> Cookies)> MakeRequestAsync(
            HttpRequestMessage request, int retryWaitTime = 1000, int retries = 3, bool throwOnRedirect = false,
            CancellationToken ct = default)
        {
            if (request.IsNull()) throw new ArgumentNullException(nameof(request));

            ct.ThrowIfCancellationRequested();

            if (retryWaitTime < 10) retryWaitTime = 1000;
            if (retries < 0) retries = 0;

            EnsureDnsReset(request.RequestUri.ToString());

            bool unrecoverableException = false;
            string errorResponse = string.Empty;
            for (int i = 0; i < retries + 1; i++)
            {
                try
                {
                    if (i > 0) request = await request.CloneAsync();
                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead, ct)
                        .ConfigureAwait(false))
                    {
                        var isSuccess = response.IsSuccessStatusCode || 
                                        (!throwOnRedirect && response.StatusCode == HttpStatusCode.Found);
                        if (isSuccess)
                        {
                            var resString = await response.Content.ReadAsStringAsync();
                            return (resString, response.GetCookies());
                        }
                        else
                        {
                            try { errorResponse = await response.Content.ReadAsStringAsync(); }
                            catch (Exception) { }
                            if (i == retries) response.EnsureSuccessStatusCode();

                            if (!response.StatusCode.MightRecover())
                            {
                                unrecoverableException = true;
                                response.EnsureSuccessStatusCode();
                            }

                            ct.ThrowIfCancellationRequested();
                        }
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (ct.IsCancellationRequested || i == retries)
                    {
                        if (errorResponse.IsNullOrWhiteSpace()) throw;
                        throw new TaskCanceledException(ex.Message + " Response message: " + errorResponse, ex);
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (ct.IsCancellationRequested || i == retries || unrecoverableException)
                    {
                        if (errorResponse.IsNullOrWhiteSpace()) throw;
                        throw new HttpRequestException(ex.Message + "Response message: " + errorResponse,
                            ex);
                    }
                }
                catch (AggregateException ex)
                {
                    if (ct.IsCancellationRequested || i == retries || unrecoverableException)
                    {
                        if (errorResponse.IsNullOrWhiteSpace() || ex.Flatten().InnerExceptions.Count < 1) throw;
                        var actualEx = ex.Flatten().InnerExceptions[0];
                        if (actualEx is TaskCanceledException cEx)
                            throw new TaskCanceledException(cEx.Message + "Response message: " + errorResponse,
                                cEx);
                        else throw new HttpRequestException(ex.Message + "Response message: " + errorResponse,
                            ex);
                    }
                    ex.Handle((x) => x is HttpRequestException || x is TaskCanceledException);
                }

                await Task.Delay(retryWaitTime, ct);
                retryWaitTime = retryWaitTime * 3;
            }

            throw new InvalidOperationException("Impossible exception.");
        }

        public static async Task DownloadFileAsync(HttpRequestMessage request,
            Func<Stream, Task> saveFileMethod, int retryWaitTime = 1000, int retries = 3, 
            CancellationToken ct = default)
        {
            if (request.IsNull()) throw new ArgumentNullException(nameof(request));

            ct.ThrowIfCancellationRequested();

            if (retryWaitTime < 10) retryWaitTime = 1000;
            if (retries < 0) retries = 0;

            EnsureDnsReset(request.RequestUri.ToString());

            bool unrecoverableException = false;
            string errorResponse = string.Empty;
            for (int i = 0; i < retries + 1; i++)
            {
                try
                {
                    if (i > 0) request = await request.CloneAsync();
                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead, ct)
                        .ConfigureAwait(false))
                    {
                        var isSuccess = response.IsSuccessStatusCode;
                        
                        if (isSuccess)
                        {
                            var stream = await response.Content.ReadAsStreamAsync();
                            await saveFileMethod(stream);
                            return;
                        }

                        try { errorResponse = await response.Content.ReadAsStringAsync(); }
                        catch (Exception) { }
                        if (i == retries) response.EnsureSuccessStatusCode();

                        if (!response.StatusCode.MightRecover())
                        {
                            unrecoverableException = true;
                            response.EnsureSuccessStatusCode();
                        }

                        ct.ThrowIfCancellationRequested();
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (ct.IsCancellationRequested || i == retries)
                    {
                        if (errorResponse.IsNullOrWhiteSpace()) throw;
                        throw new TaskCanceledException(ex.Message + "Response message: " + errorResponse,
                            ex);
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (ct.IsCancellationRequested || i == retries || unrecoverableException)
                    {
                        if (errorResponse.IsNullOrWhiteSpace()) throw;
                        throw new HttpRequestException(ex.Message + "Response message: " + errorResponse,
                            ex);
                    }
                }
                catch (AggregateException ex)
                {
                    if (ct.IsCancellationRequested || i == retries || unrecoverableException)
                    {
                        if (errorResponse.IsNullOrWhiteSpace() || ex.Flatten().InnerExceptions.Count < 1) throw;
                        var actualEx = ex.Flatten().InnerExceptions[0];
                        if (actualEx is TaskCanceledException cEx)
                            throw new TaskCanceledException(cEx.Message + "Response message: " + errorResponse,
                                cEx);
                        else throw new HttpRequestException(ex.Message + "Response message: " + errorResponse,
                            ex);
                    }
                    ex.Handle((x) => x is HttpRequestException || x is TaskCanceledException);
                }

                await Task.Delay(retryWaitTime, ct);
                retryWaitTime = retryWaitTime * 3;
            }

            throw new InvalidOperationException("Impossible exception.");
        }

        /// <summary>
        /// https://blogs.msdn.microsoft.com/alazarev/2017/12/29/disposable-finalizers-and-httpclient/
        /// </summary>
        /// <param name="url"></param>
        private static void EnsureDnsReset(string url)
        {
            var uri = new Uri(url);
            var baseUrl = $"{uri.Scheme}://{uri.Host}{(uri.IsDefaultPort ? string.Empty : ":" + uri.Port.ToString())}";
            dnsDictionary.GetOrAdd(baseUrl, k =>
            {
                var sp = ServicePointManager.FindServicePoint(uri);
                sp.ConnectionLeaseTimeout = 60 * 1000;
                return string.Empty;
            });
        }

        private static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = await request.Content.CloneAsync().ConfigureAwait(false),
                Version = request.Version
            };
            foreach (KeyValuePair<string, object> prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        private static async Task<HttpContent> CloneAsync(this HttpContent content)
        {
            if (content == null) return null;

            var ms = new MemoryStream();
            await content.CopyToAsync(ms).ConfigureAwait(false);
            ms.Position = 0;

            var clone = new StreamContent(ms);
            foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }
            return clone;
        }

        private static Dictionary<string, string> GetCookies(this HttpResponseMessage response)
        {
            var result = new Dictionary<string, string>();

            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookieValues))
            {
                foreach (var cookieValue in cookieValues)
                {
                    var assignmentPos = cookieValue.IndexOf("=", StringComparison.OrdinalIgnoreCase);
                    if (assignmentPos > 0)
                    {
                        var key = cookieValue.Substring(0, assignmentPos);
                        var terminationPos = cookieValue.IndexOf(";", StringComparison.OrdinalIgnoreCase);
                        string value;
                        if (terminationPos > assignmentPos)
                            value = cookieValue.Substring(assignmentPos + 1,
                                terminationPos - assignmentPos - 1);
                        else value = cookieValue.Substring(assignmentPos + 1);
                        result.Add(key, value);
                    }
                }
            }

            return result;
        }

        private static bool MightRecover(this HttpStatusCode responseStatus)
        {
            if (responseStatus == HttpStatusCode.Forbidden
                || responseStatus == HttpStatusCode.Unauthorized
                || responseStatus == HttpStatusCode.Found
                || responseStatus == HttpStatusCode.BadRequest) return false;
            return true;
        }

    }
}
