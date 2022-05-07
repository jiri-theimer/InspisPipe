using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;


public class GinisClientFactory
{
    public static InspisPipe.GIN1.SslPortTypeClient CreateGinSslClientProxy(string url, string username, string password)
    {
        CustomBinding binding = new CustomBinding();
        var security = TransportSecurityBindingElement.CreateUserNameOverTransportBindingElement();
        security.AllowInsecureTransport = true;
        security.IncludeTimestamp = false;
        security.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
        security.MessageSecurityVersion = MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;
        var encoding = new TextMessageEncodingBindingElement();
        encoding.MessageVersion = MessageVersion.Soap11;
        var transport = new HttpTransportBindingElement();
        transport.MaxReceivedMessageSize = 20000000; // 20 megs
        binding.Elements.Add(security);
        binding.Elements.Add(encoding);
        binding.Elements.Add(transport);
      
        var client = new InspisPipe.GIN1.SslPortTypeClient(binding, new EndpointAddress(url));

        client.ChannelFactory.Endpoint.Behaviors.Remove<System.ServiceModel.Description.ClientCredentials>();
        client.ChannelFactory.Endpoint.Behaviors.Add(new CustomCredentials());
        client.ClientCredentials.UserName.UserName = username;
        client.ClientCredentials.UserName.Password = password;
        return client;
    }

    public static InspisPipe.GIN2.GinPortTypeClient CreateGinGinClientProxy(string url, string username, string password)
    {
        CustomBinding binding = new CustomBinding();
        var security = TransportSecurityBindingElement.CreateUserNameOverTransportBindingElement();
        security.AllowInsecureTransport = true;
        security.IncludeTimestamp = false;
        security.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
        security.MessageSecurityVersion = MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;
        var encoding = new TextMessageEncodingBindingElement();
        encoding.MessageVersion = MessageVersion.Soap11;
        var transport = new HttpTransportBindingElement();
        transport.MaxReceivedMessageSize = 20000000; // 20 megs
        binding.Elements.Add(security);
        binding.Elements.Add(encoding);
        binding.Elements.Add(transport);
        InspisPipe.GIN2.GinPortTypeClient client = new InspisPipe.GIN2.GinPortTypeClient(binding, new EndpointAddress(url));

        client.ChannelFactory.Endpoint.Behaviors.Remove<System.ServiceModel.Description.ClientCredentials>();
        client.ChannelFactory.Endpoint.Behaviors.Add(new CustomCredentials());
        client.ClientCredentials.UserName.UserName = username;
        client.ClientCredentials.UserName.Password = password;
        return client;
    }
}
