using System;
namespace Pekistirec.Engine.EventLog
{
    public interface IEventLogEntity
    {
        string Action { get; set; }
        string Controller { get; set; }
        string HttpBody { get; set; }
        string HttpMethod { get; set; }
        string Mesaj { get; set; }
        string RawUrl { get; set; }
        string TarihZaman { get; set; }
        string Tip { get; set; }
        string UrlReferrer { get; set; }
        string UserIP { get; set; }
        string UserName { get; set; }
    }
}
