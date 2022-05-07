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
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

public class CustomCredentials : ClientCredentials
{
    public CustomCredentials()
    {
    }

    protected CustomCredentials(CustomCredentials cc) : base(cc)
    {
    }

    public override System.IdentityModel.Selectors.SecurityTokenManager CreateSecurityTokenManager()
    {
        return new CustomSecurityTokenManager(this);
    }

    protected override ClientCredentials CloneCore()
    {
        return new CustomCredentials(this);
    }
}

public class CustomSecurityTokenManager : ClientCredentialsSecurityTokenManager
{
    public CustomSecurityTokenManager(CustomCredentials cred) : base(cred)
    {
    }

    public override System.IdentityModel.Selectors.SecurityTokenSerializer CreateSecurityTokenSerializer(System.IdentityModel.Selectors.SecurityTokenVersion version)
    {
        return new CustomTokenSerializer(System.ServiceModel.Security.SecurityVersion.WSSecurity11);
    }
}

public class CustomTokenSerializer : WSSecurityTokenSerializer
{
    public CustomTokenSerializer(SecurityVersion sv) : base(sv)
    {
    }

    protected override void WriteTokenCore(System.Xml.XmlWriter writer, System.IdentityModel.Tokens.SecurityToken token)
    {
        UserNameSecurityToken userToken = token as UserNameSecurityToken;
        string tokennamespace = "wsse";
        DateTime created = DateTime.Now;
        string createdDateTimeStr = created.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

        // unique Nonce value - encode with SHA-1 for 'randomness'
        string phrase = Guid.NewGuid().ToString();
        var nonce = GetSHA1String(phrase);
        var password = GetPasswordDigest(Convert.FromBase64String(nonce), createdDateTimeStr, userToken.Password);
        writer.WriteRaw(string.Format("<{0}:UsernameToken wsu:Id=\"" + token.Id + "\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" + "<{0}:Username>" + userToken.UserName + "</{0}:Username>" + "<{0}:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">" + password + "</{0}:Password>" + "<{0}:Nonce>" + nonce + "</{0}:Nonce>" + "<u:Created>" + createdDateTimeStr + "</u:Created></{0}:UsernameToken>", tokennamespace));
    }

    // Implementation of GetPasswordDigest from Gordic
    public static string GetPasswordDigest(byte[] nonce, string createdStr, string password)
    {
        byte[] l_abyCreated = System.Text.Encoding.UTF8.GetBytes(createdStr);
        byte[] l_abyPassword = System.Text.Encoding.UTF8.GetBytes(password);
        byte[] l_abyBuffer = new byte[nonce.Length + l_abyCreated.Length + l_abyPassword.Length - 1 + 1];
        Array.Copy(nonce, l_abyBuffer, nonce.Length);
        Array.Copy(l_abyCreated, 0, l_abyBuffer, nonce.Length, l_abyCreated.Length);
        Array.Copy(l_abyPassword, 0, l_abyBuffer, nonce.Length + l_abyCreated.Length, l_abyPassword.Length);
        SHA1 l_oSha1 = SHA1.Create();
        return Convert.ToBase64String(l_oSha1.ComputeHash(l_abyBuffer));
    }


    protected string GetSHA1String(string phrase)
    {
        SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
        byte[] hashedDataBytes = sha1Hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(phrase));
        return Convert.ToBase64String(hashedDataBytes);
    }
}
