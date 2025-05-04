using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Dispatcher;
using System;
using System.Linq;
using CinemaTicketServer.Services;
using CinemaTicketServer.Classes;

public class ServerHandlerInspector : IDispatchMessageInspector
{
    private readonly IDatabaseService _databaseService;

    public ServerHandlerInspector(IDatabaseService databaseService)
    {
        Console.WriteLine("SI0: ServerHandlerInspector initialized");
        _databaseService = databaseService;
    }

    public object AfterReceiveRequest(
        ref Message request,
        IClientChannel channel,
        InstanceContext instanceContext)
    {
        const string hdrName = "Credentials";
        const string hdrNs = "http://tempuri.org/soapheaders";
        int credsIdx = request.Headers.FindHeader(hdrName, hdrNs);
        if (credsIdx < 0)
        {
            Console.WriteLine("SI1: Credentials header not found");
            return false;
        }

        var reader = request.Headers.GetReaderAtHeader(credsIdx);
        string credsXml = reader.ReadOuterXml();
        Console.WriteLine($"SI1: Full Credentials XML:\n{credsXml}");

        var doc = new System.Xml.XmlDocument();
        doc.LoadXml(credsXml);

        string type = doc.DocumentElement?["Type"]?.InnerText ?? string.Empty;
        string username = doc.DocumentElement?["Username"]?.InnerText ?? string.Empty;
        string password = doc.DocumentElement?["Password"]?.InnerText ?? string.Empty;

        Console.WriteLine($"SI1: Extracted Type: {type}");
        Console.WriteLine($"SI1: Extracted Username: {username}");
        Console.WriteLine($"SI1: Extracted Password: {password}");

        bool result;
        if (type.Equals("Registration", StringComparison.OrdinalIgnoreCase))
        {
            result = HandleRegistration(username, password);
        }
        else if (type.Equals("Login", StringComparison.OrdinalIgnoreCase))
        {
            result = HandleLogin(username, password);
        }
        else
        {
            Console.WriteLine($"SI1: Unknown Type: {type}");
            result = false;
        }

        Console.WriteLine(result
            ? "SI1: Operation succeeded."
            : "SI1: Operation failed.");
        return result;
    }

    public void BeforeSendReply(ref Message reply, object correlationState)
    {
        Console.WriteLine("SI2: Preparing to send SOAP reply");
        Console.WriteLine(reply.ToString());

        if (correlationState is bool isSuccess)
        {
            Console.WriteLine(isSuccess
                ? "SI2: Reply for successful operation"
                : "SI2: Reply for failed operation");

            // Dodanie niestandardowego nagłówka SOAP
            var header = MessageHeader.CreateHeader(
                "OperationStatus", // Nazwa nagłówka
                "http://tempuri.org/soapheaders", // Przestrzeń nazw
                isSuccess ? "Success" : "Failure" // Wartość nagłówka
            );
            reply.Headers.Add(header);
        }
    }

    private bool HandleRegistration(string username, string password)
    {
        Console.WriteLine("SI3: Handling registration");
        if (_databaseService.GetAccounts().Any(a => a.Username == username))
        {
            Console.WriteLine("SI3: Registration failed - user exists");
            return false;
        }

        var newAccount = new Account(username, password, AccountType.User);
        _databaseService.AddAccount(newAccount);
        Console.WriteLine("SI3: Registration succeeded - user added");
        return true;
    }

    private bool HandleLogin(string username, string password)
    {
        Console.WriteLine("SI3: Handling login");
        bool valid = _databaseService.GetAccounts()
            .Any(a => a.Username == username && a.Password == password);
        Console.WriteLine(valid
            ? "SI3: Login succeeded"
            : "SI3: Login failed - invalid credentials");
        return valid;
    }
}
