using RestSharp;
using System.Runtime.Serialization;

public class Network
{
    private readonly int codEquipe;
    private readonly RestClient httpClient;

    public Network()
    {
        codEquipe = 0;
        // httpClient = new RestSharp.RestClient("http://localhost:17060");
        httpClient = new RestSharp.RestClient("http://SOALV3DFGC01:8080/hack");
    }

    public LampadaPutResponseBody AtualizaStatus(byte codLampada, bool status)
    {
        return httpClient.Put(new JsonRequest<LampadaPutRequestBody, LampadaPutResponseBody>(
            resource: $"/api/lampada/{codLampada}/status", 
            request: new LampadaPutRequestBody
            {
                IdEquipe = codEquipe,
                Status = status, 
            })).Data;
    }

    [DataContract]
    public struct LampadaPutRequestBody
    {
        [DataMember(Name = "status")]
        public bool Status { get; internal set; }

        [DataMember(Name = "idEquipe")]
        public int IdEquipe { get; internal set; }
    }

    [DataContract]
    public struct LampadaPutResponseBody
    {
        [DataMember(Name = "lampada")]
        public byte Lampada { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        public override string ToString()
        {
            return $"Lampada {Lampada} -> {Status}";
        }
    }
}