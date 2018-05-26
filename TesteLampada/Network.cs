using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace STF.SI.Common
{
    public static class Network
    {
        public static class HTTP
        {
            const string METHOD_POST = "POST";
            const string METHOD_PUT = "PUT";
            const string METHOD_DELETE = "DELETE";

            const string CONTENT_TYPE_JSON = "application/json";
            const string CONTENT_TYPE_XML = "application/xml;charset=ISO-8859-1";

            #region .: GET :.
            /// <summary>
            /// Realiza uma chamada GET na URL informada.
            /// É considerado um Timeout de 5min.
            /// </summary>
            /// <param name="url">URL de destino</param>
            /// <param name="parametrosCabecalho">Parâmetros do cabeçalho</param>
            /// <returns>Resposta da chamada como texto literal.</returns>
            public static string GET(string url, IDictionary<string, string> parametrosCabecalho = null)
                => GET(url, parametrosCabecalho: parametrosCabecalho, timeOut: new TimeSpan(0, 5, 0));
            /// <summary>
            /// Realiza uma chamada GET na URL informada.
            /// Não é gerado timeout
            /// </summary>
            /// <param name="url">URL de destino</param>
            /// <param name="parametrosQueryString">Parâmetros de QueryString</param>
            /// <param name="parametrosCabecalho">Parâmetros do cabeçalho</param>
            /// <param name="timeOut">Timeout</param>
            /// <returns>Resposta da chamada como texto literal.</returns>
            public static string GET(string url, IDictionary<string, string> parametrosQueryString = null, IDictionary<string, string> parametrosCabecalho = null, TimeSpan? timeOut = null)
            {
                try
                {
                    if (parametrosQueryString != null)
                    {
                        foreach (var parametro in parametrosQueryString)
                        {
                            if (!url.Contains("?"))
                            {
                                url += "?";
                            }
                            else
                            {
                                url += "&";
                            }

                            url += string.Format("{0}={1}", parametro.Key, parametro.Value);
                        }
                    }

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                    if (parametrosCabecalho != null)
                    {
                        foreach (var item in parametrosCabecalho)
                        {
                            request.Headers.Add(item.Key, item.Value);
                        }
                    }

                    if (timeOut.HasValue)
                        request.Timeout = Convert.ToInt32(timeOut.Value.TotalMilliseconds);

                    using (WebResponse response = request.GetResponse())
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (Stream responseStream = ex.Response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                        {
                            throw new Exception(reader.ReadToEnd(), ex);
                        }
                    }
                    throw ex;
                }
            }
            #endregion

            #region .: POST :.
            /// <summary>
            /// Realiza uma chamada POST na URL informada.
            /// </summary>
            /// <param name="url">Url de chamada</param>
            /// <param name="parametros">Parametros que serão postados na chamada</param>
            /// <param name="parametrosCabecalho">Parametros que serão adicionados no cabeçalho</param>
            /// <param name="parametrosQueryString">Parametros que serão adicionados na</param>
            /// <returns></returns>
            public static string POST(string url, IDictionary<string, string> parametros = null, IDictionary<string, string> parametrosCabecalho = null, IDictionary<string, string> parametrosQueryString = null)
                => SEND_Values(METHOD_POST, url, parametros, parametrosCabecalho, parametrosQueryString);
            /// <summary>
            /// Realiza uma chamada POST na URL informada.
            /// </summary>
            /// <param name="url">Url de chamada</param>
            /// <param name="conteudo">Conteúdo que será serializado para Json</param>
            /// <param name="parametrosCabecalho">Parametros que serão adicionados no cabeçalho</param>
            /// <returns></returns>
            public static string POST_Json(string url, object conteudo, IDictionary<string, string> parametrosCabecalho = null)
                => SEND_Content(METHOD_POST, url, CONTENT_TYPE_JSON, Serialization.ObjectToJson(conteudo), parametrosCabecalho);
            /// <summary>
            /// Realiza uma chamada POST na URL informada.
            /// </summary>
            /// <param name="url">Url de chamada</param>
            /// <param name="conteudo">Conteúdo que será serializado para XML</param>
            /// <param name="parametrosQueryString">Parametros que serão adicionados na URL</param>
            /// <returns></returns>
            public static string POST_Xml(string url, object conteudo, IDictionary<string, string> parametrosQueryString = null)
                => SEND_Content(METHOD_POST, url, CONTENT_TYPE_XML, Serialization.ObjectToXml(conteudo), parametrosQueryString: parametrosQueryString);
            #endregion POST

            #region .: PUT :.
            /// <summary>
            /// Realiza uma chamada PUT na URL informada.
            /// </summary>
            /// <param name="url">Url de chamada</param>
            /// <param name="parametros">Parametros que serão postados na chamada</param>
            /// <param name="parametrosCabecalho">Parametros que serão adicionados no cabeçalho</param>
            /// <param name="parametrosQueryString">Parametros que serão adicionados na</param>
            /// <returns></returns>
            public static string PUT(string url, IDictionary<string, string> parametros = null, IDictionary<string, string> parametrosCabecalho = null, IDictionary<string, string> parametrosQueryString = null)
                => SEND_Values(METHOD_PUT, url, parametros, parametrosCabecalho, parametrosQueryString);
            /// <summary>
            /// Realiza uma chamada PUT na URL informada.
            /// </summary>
            /// <param name="url">Url de chamada</param>
            /// <param name="conteudo">Conteúdo que será serializado em JSON</param>
            /// <param name="parametrosCabecalho">Parametros que serão adicionados no cabeçalho</param>
            /// <returns></returns>
            public static string PUT_Json(string url, object conteudo, IDictionary<string, string> parametrosCabecalho = null)
                => SEND_Content(METHOD_PUT, url, CONTENT_TYPE_JSON, Serialization.ObjectToJson(conteudo), parametrosCabecalho: parametrosCabecalho);
            #endregion PUT

            #region .: DELETE :.
            /// <summary>
            /// Realiza uma chamada DELETE na URL informada.
            /// </summary>
            /// <param name="url">Url de chamada</param>
            /// <param name="parametros">Parametros que serão postados na chamada</param>
            /// <param name="parametrosCabecalho">Parametros que serão adicionados no cabeçalho</param>
            /// <param name="parametrosQueryString">Parametros que serão adicionados na</param>
            /// <returns></returns>
            public static string DELETE(string url, IDictionary<string, string> parametros = null, IDictionary<string, string> parametrosCabecalho = null, IDictionary<string, string> parametrosQueryString = null)
                => SEND_Values(METHOD_DELETE, url, parametros, parametrosCabecalho, parametrosQueryString);
            /// <summary>
            /// Realiza uma chamada DELETE na URL informada.
            /// </summary>
            /// <param name="url">Url de chamada</param>
            /// <param name="conteudo">Conteúdo que será serializado em JSON</param>
            /// <param name="parametrosCabecalho">Parametros que serão adicionados no cabeçalho</param>
            /// <returns></returns>
            public static string DELETE_Json(string url, object conteudo, IDictionary<string, string> parametrosCabecalho = null)
                => SEND_Content(METHOD_DELETE, url, CONTENT_TYPE_JSON, Serialization.ObjectToJson(conteudo), parametrosCabecalho: parametrosCabecalho);
            #endregion DELETE

            private static string SEND_Values(string method, string url, IDictionary<string, string> parametros = null, IDictionary<string, string> parametrosCabecalho = null, IDictionary<string, string> parametrosQueryString = null, NetworkCredential credential = null)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        if (credential != null)
                            client.Credentials = credential;

                        var paramtersToPost = new System.Collections.Specialized.NameValueCollection();

                        if (parametrosCabecalho != null)
                        {
                            foreach (var item in parametrosCabecalho)
                            {
                                if (client.Headers[item.Key] == null)
                                    client.Headers.Add(item.Key, item.Value);
                            }
                        }

                        if (parametrosQueryString != null)
                        {
                            foreach (var item in parametrosQueryString)
                            {
                                if (client.QueryString[item.Key] == null)
                                    client.QueryString.Add(item.Key, item.Value);
                            }
                        }

                        if (parametros != null)
                        {
                            foreach (var item in parametros)
                            {
                                paramtersToPost.Add(item.Key, item.Value);
                            }
                        }

                        return Encoding.UTF8.GetString(client.UploadValues(url, method, paramtersToPost));
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (Stream responseStream = ex.Response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                        {
                            var response = (HttpWebResponse)ex.Response;

                            throw new HttpRequestException(response.StatusCode, reader.ReadToEnd(), ex);
                        }
                    }
                    throw ex;
                }
            }
            private static string SEND_Content(string method, string url, string contentType, string content, IDictionary<string, string> parametrosCabecalho = null, IDictionary<string, string> parametrosQueryString = null, NetworkCredential credential = null)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        if (credential != null)
                            client.Credentials = credential;

                        var paramtersToPost = new System.Collections.Specialized.NameValueCollection();

                        client.Headers.Add(HttpRequestHeader.ContentType, contentType);

                        if (parametrosCabecalho != null)
                        {
                            foreach (var item in parametrosCabecalho)
                            {
                                if (client.Headers[item.Key] == null)
                                    client.Headers.Add(item.Key, item.Value);
                            }
                        }

                        if (parametrosQueryString != null)
                        {
                            foreach (var item in parametrosQueryString)
                            {
                                if (client.QueryString[item.Key] == null)
                                    client.QueryString.Add(item.Key, item.Value);
                            }
                        }

                        return client.UploadString(url, method, content);
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (Stream responseStream = ex.Response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                        {
                            var response = (HttpWebResponse)ex.Response;

                            throw new HttpRequestException(response.StatusCode, reader.ReadToEnd(), ex);
                        }
                    }
                    throw ex;
                }
            }

            public class HttpRequestException : Exception
            {
                public readonly string Response;
                public readonly HttpStatusCode Status;

                public HttpRequestException(HttpStatusCode status, string response, Exception innerException) : base("Falha ao requisitar o serviço", innerException)
                {
                    Status = status;
                    Response = response;
                }
            }
        }
    }
}