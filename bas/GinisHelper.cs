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
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Configuration;

using System.Security.Principal;

using System.Web.Services.Protocols;
using System.Security.Cryptography;
using WebGrease.Activities;

public class GinisHelper
{
    //private string m_sXmlTemplatesPath = Directory.GetCurrentDirectory() + @"\XmlTemplates";  // "C:\Temp\XmlTemplates"

    private string m_sXmlTemplatesPath = @ConfigurationManager.AppSettings["XmlTemplates"];
    
    private string USERNAME { get; set; } = "inspis";
    private string PASSWORD { get; set; } = "Pwd4Gin!";
    private string EXT_DOMAIN { get; set; } = "CSI0AIE0A046"; // Identifikator externiho systemu v Ginis
    private string SSLURL { get; set; } = "http://wmx06.csi.local/Gordic/Ginis/Ws/SSL01/Ssl.svc";
    private string GINURL { get; set; } = "http://wmx06.csi.local/Gordic/Ginis/Ws/GIN01/Gin.svc";

    private InspisPipe.GIN1.SslPortTypeClient m_oSslRef;
    private InspisPipe.GIN2.GinPortTypeClient m_oGinRef;

    public event OnErrorEventHandler OnError;

    public delegate void OnErrorEventHandler(string strError);


    /// <summary>
    ///     ''' constructor
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public GinisHelper(string strLogin)
    {
        switch (strLogin)
        {
            case "lamos":
            case "hovado":
            case "robot":
            case "kuchar":
                {
                    this.USERNAME = "inspis";    // vyjímka
                    break;
                }

            default:
                {
                    this.USERNAME = strLogin;    // "CSI0AIE0A046\" & funguje pouze pro doménové uživatele
                    break;
                }
        }
        //if (this.USERNAME.IndexOf("(lamos)") > 0 | this.USERNAME.IndexOf("(kuchar)") > 0)
        //{
        //    var arr = this.USERNAME.Split("(");

        //}

        if (strLogin == "hovado")
        {
            //testovací server
            this.GINURL = "http://wmx21.csi.local/Gordic/Ginis/Ws/GIN01/Gin.svc"; ; //testování
            this.SSLURL = "http://wmx21.csi.local/Gordic/Ginis/Ws/SSL01/Ssl.svc"; ; //testování
            this.PASSWORD = "Pwd4Gin!"; //testování
        }
        

        try
        {
            var domainUsername = EXT_DOMAIN + @"\" + USERNAME;
            m_oGinRef = GinisClientFactory.CreateGinGinClientProxy(GINURL, domainUsername, PASSWORD);
            m_oSslRef = GinisClientFactory.CreateGinSslClientProxy(SSLURL, domainUsername, PASSWORD);
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("Chyba při inicializaci komunikačního rozhraní s GINIS:" + ex.Message.ToString());
        }
    }

    public static XmlNode ToXmlNode(XElement element)
    {
        using (XmlReader xmlReader = element.CreateReader())
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlReader);
            return xmlDoc;
        }
    }

    public static XElement ToXElement(XmlDocument doc)
    {
        return XElement.Load(new XmlNodeReader(doc));
    }

    // Digest auth requires ixsExt attribute
    // Public Sub AppendIxsExtAttribute(oXml As XmlDocument)
    // Dim xrg = oXml.GetElementsByTagName("Xrg")(0)
    // Dim ixsExtAttr = oXml.CreateAttribute("ixsExt")
    // xrg.Attributes.Append(ixsExtAttr)
    // ixsExtAttr.Value = EXT_DOMAIN
    // End Sub

    public string NajdiEsuIco(string strICO)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;

        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/gin/esu/najdi-esu-ico/response/v_1.0.0.0");

        oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Najdi-esu-ico.xml"));
        oXml.GetElementsByTagName("Ico")[0].InnerText = strICO;

        oResult = ToXmlNode(m_oGinRef.Najdiesuico(ToXElement(oXml)));

        if (oResult == null)
        {

            throw new Exception("IČO " + strICO + " chyba");
        }



        return oResult.SelectSingleNode("//ns:Id-esu", ns).InnerText;
    }

    public string OdeslatDatovku(string GinisDocPid,string GinisFilePid, string IdEsu,string IdDS,string MessageSubject)        //JT, fyzická osoba: df6w7m5, CleverApp: nhtn8nh, OSVČ: xqfz92m
    {
        //v GinisFilePid může být více názvů souborů oddělených středníkem

        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        List<GinisDocument> oDocs = new List<GinisDocument>();

        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/odeslani/response/v_1.0.0.0");

        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Odeslani.xml"));            
            oXml.GetElementsByTagName("Id-dokumentu")[0].InnerText = GinisDocPid;
            oXml.GetElementsByTagName("Zpusob-doruceni")[0].InnerText = "ds";
            oXml.GetElementsByTagName("Id-adresata")[0].InnerText = IdEsu;
            
            oXml.GetElementsByTagName("Rezim")[0].InnerText = "odeslani";       //může být i: priprava nebo priprava

            
            oXml.GetElementsByTagName("Odes-od")[0].InnerText = "g7zais9";    //odesílatel

            oXml.GetElementsByTagName("Odes-komu")[0].InnerText = IdDS;
            oXml.GetElementsByTagName("Mail-predmet")[0].InnerText = MessageSubject;
            oXml.GetElementsByTagName("Sezn-id-priloh")[0].InnerText = GinisFilePid;



            oResult = ToXmlNode(m_oSslRef.Odeslani(ToXElement(oXml)));

            
            if (oResult == null)
            {

                throw new Exception($"Dokument {GinisDocPid}/{GinisFilePid} nebyl odeslán, komu: {IdEsu}/{IdDS}, předmět zprávy: {MessageSubject}.");
            }


            var rXml = new XmlDocument();
            rXml.LoadXml(oResult.InnerXml);
            
            
            return rXml.GetElementsByTagName("Id-odeslani")[0].InnerText;

            
        }
        catch (Exception ex)
        {
            bas.LogError($"Dokument {GinisDocPid}/{GinisFilePid} nebyl odeslán, komu: {IdEsu}/{IdDS}, předmět zprávy: {MessageSubject}","datovka","OdeslatDatovku");
            bas.LogError(ex.Message, "datovka", "OdeslatDatovku");
            
            
            
            OnError?.Invoke(ex.Message);
            throw new Exception("UploadFile" + ex.Message.ToString());
        }

    }

    /// <summary>
    ///     ''' nacte detail o dokumentu v GINIS
    ///     ''' </summary>
    ///     ''' <param name="PID">PID dokumentu v GINIS</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public GinisDocument DetailDokumentu(string PID)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNodeList oXmlNodeList;
        XmlNode oResult;
        GinisDocument oGinisDocument = null/* TODO Change to default(_) if this is not a reference type */;
        GinisFile oSoubor;

        // Dim ns As XmlNamespaceManager = New XmlNamespaceManager(oResult.NameTable)
        // ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/detail-dokumentu/response/v_1.0.0.0")
        // Try
        // oXml = XElement.Load(Path.Combine(m_sXmlTemplatesPath, "Detail-dokumentu.xml"))
        // oXml.Descendants("Id-dokumentu").First.Value = PID

        // oResult = ToXmlNode(m_oSslRef.Detaildokumentu(oXml))
        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/detail-dokumentu/response/v_1.0.0.0");
        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Detail-dokumentu.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Id-dokumentu")[0].InnerText = PID;

            oResult = ToXmlNode(m_oSslRef.Detaildokumentu(ToXElement(oXml)));

            if (oResult != null)
            {
                oGinisDocument = new GinisDocument();
                oGinisDocument.IdDokumentu = oResult.SelectSingleNode("//ns:Id-dokumentu", ns).InnerText;
                oGinisDocument.IdSpisu = oResult.SelectSingleNode("//ns:Id-spisu", ns).InnerText != null ? oResult.SelectSingleNode("//ns:Id-spisu", ns).InnerText : null;
                oGinisDocument.PriznakSpisu = oResult.SelectSingleNode("//ns:Priznak-spisu", ns).InnerText == "spis" ? true : false;
                oGinisDocument.PriznakCj = oResult.SelectSingleNode("//ns:Priznak-cj", ns).InnerText;
                oGinisDocument.PriznakFyzExistence = oResult.SelectSingleNode("//ns:Priznak-fyz-existence", ns).InnerText;
                oGinisDocument.PriznakElObrazu = oResult.SelectSingleNode("//ns:Priznak-el-obrazu", ns).InnerText;
                oGinisDocument.IdSouboru = oResult.SelectSingleNode("//ns:Id-souboru", ns) != null ? oResult.SelectSingleNode("//ns:Id-souboru", ns).InnerText : null;
                oGinisDocument.JmenoSouboru = oResult.SelectSingleNode("//ns:Jmeno-souboru", ns) != null ? oResult.SelectSingleNode("//ns:Jmeno-souboru", ns).InnerText : null;
                oGinisDocument.PopisSouboru = oResult.SelectSingleNode("//ns:Popis-souboru", ns) != null ? oResult.SelectSingleNode("//ns:Popis-souboru", ns).InnerText : null;
                oGinisDocument.IdTypuDokumentu = oResult.SelectSingleNode("//ns:Id-typu-dokumentu", ns) != null ? oResult.SelectSingleNode("//ns:Id-typu-dokumentu", ns).InnerText : null;
                oGinisDocument.Vec = oResult.SelectSingleNode("//ns:Vec", ns) != null ? oResult.SelectSingleNode("//ns:Vec", ns).InnerText : null;
                oGinisDocument.Znacka = oResult.SelectSingleNode("//ns:Znacka", ns) != null ? oResult.SelectSingleNode("//ns:Znacka", ns).InnerText : null;

                oXmlNodeList = oResult.SelectNodes("//ns:Prilohy-dokumentu", ns);
                for (var index = 0; index <= oXmlNodeList.Count - 1; index++)
                {
                    oSoubor = new GinisFile();
                    oSoubor.IdDokumentu = oGinisDocument.IdDokumentu;
                    oSoubor.IdSouboru = oXmlNodeList[index].SelectSingleNode("ns:Id-souboru", ns) != null ? oXmlNodeList[index].SelectSingleNode("ns:Id-souboru", ns).InnerText : null;
                    oSoubor.PopisSouboru = oXmlNodeList[index].SelectSingleNode("ns:Titulek", ns) != null ? oXmlNodeList[index].SelectSingleNode("ns:Titulek", ns).InnerText : null;
                    oSoubor.PodrobnyPopisSouboru = oXmlNodeList[index].SelectSingleNode("ns:Popis", ns) != null ? oXmlNodeList[index].SelectSingleNode("ns:Popis", ns).InnerText : null;
                    oSoubor.Poznamka = oXmlNodeList[index].SelectSingleNode("ns:Poznamka", ns) != null ? oXmlNodeList[index].SelectSingleNode("ns:Poznamka", ns).InnerText : null;
                    oSoubor.VerzeSouboru = oXmlNodeList[index].SelectSingleNode("ns:Verze-souboru", ns) != null ? oXmlNodeList[index].SelectSingleNode("ns:Verze-souboru", ns).InnerText : null;
                    oSoubor.TypVazby = "elektronicka-priloha";

                    oGinisDocument.SeznamPriloh.Add(oSoubor);
                }
            }

            return oGinisDocument;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("GetDocumentDetail:" + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     '''  založí nový dokument a vloží ho do spisu s daným PID
    ///     ''' </summary>
    ///     ''' <param name="PID_Typu_Dokumentu">PID Typu Dokumentu</param>
    ///     ''' <param name="PID_Spisu">PID spisu</param>
    ///     ''' <returns>PID nově založeného dokumentu</returns>
    ///     ''' <remarks></remarks>
    public string NovyDokument(string PID_Typu_Dokumentu, string PID_Spisu, string Vec)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        GinisDocument oGinisDocument = null/* TODO Change to default(_) if this is not a reference type */;

        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/zaloz-pisemnost/response/v_1.0.0.0");
        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Zaloz-pisemnost0.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Id-typu-dokumentu")[0].InnerText = PID_Typu_Dokumentu;
            oXml.GetElementsByTagName("Vec")[0].InnerText = Vec;

            oResult = ToXmlNode(m_oSslRef.Zalozpisemnost(ToXElement(oXml)));

            if (oResult != null)
            {
                oGinisDocument = new GinisDocument();
                oGinisDocument.IdDokumentu = oResult.SelectSingleNode("//ns:Id-dokumentu", ns).InnerText;
            }

            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Vloz-do-spisu.xml"));
            oXml.GetElementsByTagName("Id-dokumentu")[0].InnerText = oGinisDocument.IdDokumentu;
            oXml.GetElementsByTagName("Id-spisu")[0].InnerText = PID_Spisu;

            m_oSslRef.Vlozdospisu(ToXElement(oXml));

            return oGinisDocument.IdDokumentu;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("NovyDokument:" + ex.Message.ToString());
        }
    }


    /// <summary>
    ///     ''' vrátí seznam dokumentů GINIS
    ///     ''' </summary>
    ///     ''' <param name="DatumOd">datum od</param>
    ///     ''' <param name="DatumDo">datum do</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public List<GinisDocument> SeznamDokumentuSpisu(DateTime DatumOd, DateTime DatumDo, string PID_Spisu)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNodeList oXmlNodeList;
        XmlNode oResult;
        GinisDocument oGinisDocument = null/* TODO Change to default(_) if this is not a reference type */;
        List<GinisDocument> oDocuments = new List<GinisDocument>();

        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/detail-dokumentu/response/v_1.0.0.0");
        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Detail-dokumentu.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Id-dokumentu")[0].InnerText = PID_Spisu;

            oResult = ToXmlNode(m_oSslRef.Detaildokumentu(ToXElement(oXml)));

            if (oResult != null)
            {
                if (oResult.SelectSingleNode("//ns:Id-dokumentu", ns) == null)
                    return oDocuments;
                oXmlNodeList = oResult.SelectNodes("//ns:Ssl-obsah-spis", ns);
                for (var index = 0; index <= oXmlNodeList.Count - 1; index++)
                {
                    string sIdDokumentu;
                    sIdDokumentu = oXmlNodeList[index].SelectSingleNode("ns:Id-vlozeneho-dokumentu", ns).InnerText != null ? oXmlNodeList[index].SelectSingleNode("ns:Id-vlozeneho-dokumentu", ns).InnerText : null;
                    oGinisDocument = DetailDokumentu(sIdDokumentu);
                    oDocuments.Add(oGinisDocument);
                }
            }

            return oDocuments;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("GetDocuments:" + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' vrátí seznam dokumentů podle č.j.
    ///     ''' </summary>
    ///     ''' <param name="DatumOd">datum od</param>
    ///     ''' <param name="DatumDo">datum do</param>
    ///     ''' <param name="DenikCj">deník čj (ČŠIG)</param>
    ///     ''' <param name="RokCj">rok čj (2014)</param>
    ///     ''' <param name="PoradoveCisloCj">pořadové číslo čj. (3338)</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public List<GinisDocument> NajdiDokumentPodleCj(DateTime DatumOd, DateTime DatumDo, string DenikCj, string RokCj, int PoradoveCisloCj)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        GinisDocument oGinisDocument;
        List<GinisDocument> oDocs = new List<GinisDocument>();
        XmlElement oIdSpisuElem;
        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-cj/response/v_1.0.0.0");

        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Prehled-cj.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Datum-podani-od")[0].InnerText = DatumOd.ToString("yyyy-MM-dd");
            oXml.GetElementsByTagName("Datum-podani-do")[0].InnerText = DatumDo.ToString("yyyy-MM-dd");
            if (oXml.GetElementsByTagName("Denik-cj") == null || oXml.GetElementsByTagName("Denik-cj").Count == 0)
            {
                oIdSpisuElem = oXml.CreateElement("Denik-cj", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-cj/request/v_1.0.0.0");
                oIdSpisuElem.InnerText = DenikCj;
                oXml.GetElementsByTagName("Prehled-cj")[0].AppendChild(oIdSpisuElem);
            }
            else
                oXml.GetElementsByTagName("Denik-cj")[0].InnerText = DenikCj;

            if (oXml.GetElementsByTagName("Rok-cj") == null || oXml.GetElementsByTagName("Rok-cj").Count == 0)
            {
                oIdSpisuElem = oXml.CreateElement("Rok-cj", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-cj/request/v_1.0.0.0");
                oIdSpisuElem.InnerText = RokCj;
                oXml.GetElementsByTagName("Prehled-cj")[0].AppendChild(oIdSpisuElem);
            }
            else
                oXml.GetElementsByTagName("Rok-cj")[0].InnerText = RokCj;

            if (oXml.GetElementsByTagName("Poradove-cislo-cj") == null || oXml.GetElementsByTagName("Poradove-cislo-cj").Count == 0)
            {
                oIdSpisuElem = oXml.CreateElement("Poradove-cislo-cj", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-cj/request/v_1.0.0.0");
                oIdSpisuElem.InnerText = PoradoveCisloCj.ToString();
                oXml.GetElementsByTagName("Prehled-cj")[0].AppendChild(oIdSpisuElem);
            }
            else
                oXml.GetElementsByTagName("Poradove-cislo-cj")[0].InnerText = PoradoveCisloCj.ToString();

            oResult = ToXmlNode(m_oSslRef.Prehledcj(ToXElement(oXml)));
            
            if (oResult == null)
                return null;

            XmlNodeList oXmlNodeList = oResult.SelectNodes("//ns:Prehled-cj", ns);
            if (oXmlNodeList == null || oXmlNodeList.Count == 0)
                return null;

            foreach (XmlNode oNode in oXmlNodeList)
            {
                if (oNode.Name == null || oNode.Name.ToString() != "Prehled-cj")
                    continue;

                var sDocId = oNode.SelectSingleNode("ns:Id-init-dokumentu", ns).InnerText;
                oGinisDocument = DetailDokumentu(sDocId);
                oDocs.Add(oGinisDocument);
            }

            return oDocs;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("GetDocuments:" + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' vrátí spis podle spisové znacky
    ///     ''' </summary>
    ///     ''' <param name="DatumOd">datum od</param>
    ///     ''' <param name="DatumDo">datum do</param>
    ///     ''' <param name="Znacka">Spisová značka (ČŠIG-S-502/14 G42/1)</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public List<GinisDocument> NajdiSpisPodleZnacky(DateTime DatumOd, DateTime DatumDo, string Znacka)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        GinisDocument oGinisDocument;
        List<GinisDocument> oDocs = new List<GinisDocument>();
        XmlElement oIdSpisuElem;
        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-dokumentu/response/v_1.0.0.0");

        var sZnacka = Znacka.Replace("-", "*");

        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Prehled-dokumentu.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Datum-podani-od")[0].InnerText = DatumOd.ToString("yyyy-MM-dd");
            oXml.GetElementsByTagName("Datum-podani-do")[0].InnerText = DatumDo.ToString("yyyy-MM-dd");
            oXml.GetElementsByTagName("Priznak-spisu")[0].InnerText = "spis";
            if (oXml.GetElementsByTagName("Znacka") == null || oXml.GetElementsByTagName("Id-spisu").Count == 0)
            {
                oIdSpisuElem = oXml.CreateElement("Znacka", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-dokumentu/request/v_1.0.0.0");
                oIdSpisuElem.InnerText = sZnacka;
                oXml.GetElementsByTagName("Prehled-dokumentu")[0].AppendChild(oIdSpisuElem);
            }
            else
                oXml.GetElementsByTagName("Znacka")[0].InnerText = sZnacka;

            oResult = ToXmlNode(m_oSslRef.Prehleddokumentu(ToXElement(oXml)));

            if (oResult == null)
                return null;

            XmlNodeList oXmlNodeList = oResult.SelectNodes("//ns:Prehled-dokumentu", ns);
            if (oXmlNodeList == null || oXmlNodeList.Count == 0)
                return null;

            foreach (XmlNode oNode in oXmlNodeList)
            {
                if (oNode.Name == null || oNode.Name.ToString() != "Prehled-dokumentu")
                    continue;

                oGinisDocument = new GinisDocument();
                oGinisDocument.IdDokumentu = oNode.SelectSingleNode("ns:Id-dokumentu", ns).InnerText;
                oGinisDocument.IdSpisu = oNode.SelectSingleNode("ns:Id-spisu", ns).InnerText != null ? oNode.SelectSingleNode("ns:Id-spisu", ns).InnerText : null;
                oGinisDocument.PriznakSpisu = oNode.SelectSingleNode("ns:Priznak-spisu", ns).InnerText == "spis" ? true : false;
                oGinisDocument.PriznakCj = oNode.SelectSingleNode("ns:Priznak-cj", ns).InnerText;
                oGinisDocument.PriznakFyzExistence = oNode.SelectSingleNode("ns:Priznak-fyz-existence", ns).InnerText;
                oGinisDocument.PriznakElObrazu = oNode.SelectSingleNode("ns:Priznak-el-obrazu", ns).InnerText;
                oGinisDocument.IdTypuDokumentu = oNode.SelectSingleNode("ns:Id-typu-dokumentu", ns).InnerText != null ? oNode.SelectSingleNode("ns:Id-typu-dokumentu", ns).InnerText : null;
                oDocs.Add(oGinisDocument);
            }

            return oDocs;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("GetDocuments:" + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' vrátí spis podle PID
    ///     ''' </summary>
    ///     ''' <param name="DatumOd">datum od</param>
    ///     ''' <param name="DatumDo">datum do</param>
    ///     ''' <param name="PID">PID spisu</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public List<GinisDocument> NajdiSpis(DateTime DatumOd, DateTime DatumDo, string PID)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        GinisDocument oGinisDocument;
        List<GinisDocument> oDocs = new List<GinisDocument>();
        XmlElement oIdSpisuElem;
        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-dokumentu/response/v_1.0.0.0");

        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Prehled-dokumentu.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Datum-podani-od")[0].InnerText = DatumOd.ToString("yyyy-MM-dd");
            oXml.GetElementsByTagName("Datum-podani-do")[0].InnerText = DatumDo.ToString("yyyy-MM-dd");
            oXml.GetElementsByTagName("Priznak-spisu")[0].InnerText = "spis";
            if (oXml.GetElementsByTagName("Id-spisu") == null || oXml.GetElementsByTagName("Id-spisu").Count == 0)
            {
                oIdSpisuElem = oXml.CreateElement("Id-spisu", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-dokumentu/request/v_1.0.0.0");
                oIdSpisuElem.InnerText = PID;
                oXml.GetElementsByTagName("Prehled-dokumentu")[0].AppendChild(oIdSpisuElem);
            }
            else
                oXml.GetElementsByTagName("Id-spisu")[0].InnerText = PID;

            oResult = ToXmlNode(m_oSslRef.Prehleddokumentu(ToXElement(oXml)));

            if (oResult == null)
                return null;

            XmlNodeList oXmlNodeList = oResult.SelectNodes("//ns:Prehled-dokumentu", ns);
            if (oXmlNodeList == null || oXmlNodeList.Count == 0)
                return null;

            foreach (XmlNode oNode in oXmlNodeList)
            {
                if (oNode.Name == null || oNode.Name.ToString() != "Prehled-dokumentu")
                    continue;

                oGinisDocument = new GinisDocument();
                oGinisDocument.IdDokumentu = oNode.SelectSingleNode("ns:Id-dokumentu", ns).InnerText;
                oGinisDocument.IdSpisu = oNode.SelectSingleNode("ns:Id-spisu", ns).InnerText != null ? oNode.SelectSingleNode("ns:Id-spisu", ns).InnerText : null;
                oGinisDocument.PriznakSpisu = oNode.SelectSingleNode("ns:Priznak-spisu", ns).InnerText == "spis" ? true : false;
                oGinisDocument.PriznakCj = oNode.SelectSingleNode("ns:Priznak-cj", ns).InnerText;
                oGinisDocument.PriznakFyzExistence = oNode.SelectSingleNode("ns:Priznak-fyz-existence", ns).InnerText;
                oGinisDocument.PriznakElObrazu = oNode.SelectSingleNode("ns:Priznak-el-obrazu", ns).InnerText;
                oGinisDocument.IdTypuDokumentu = oNode.SelectSingleNode("ns:Id-typu-dokumentu", ns).InnerText != null ? oNode.SelectSingleNode("ns:Id-typu-dokumentu", ns).InnerText : null;
                oDocs.Add(oGinisDocument);
            }

            return oDocs;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("GetDocuments:" + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' vrátí seznam dokumentů GINIS
    ///     ''' </summary>
    ///     ''' <param name="DatumOd">datum od</param>
    ///     ''' <param name="DatumDo">datum do</param>
    ///     ''' <param name="Spis">flag, zda vracet spis (true) nebo jinou písemost (false - dokument)</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public List<GinisDocument> SeznamDokumentu(DateTime DatumOd, DateTime DatumDo, bool Spis)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        GinisDocument oGinisDocument;
        List<GinisDocument> oDocs = new List<GinisDocument>();

        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/prehled-dokumentu/response/v_1.0.0.0");

        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Prehled-dokumentu.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Datum-podani-od")[0].InnerText = DatumOd.ToString("yyyy-MM-dd");
            oXml.GetElementsByTagName("Datum-podani-do")[0].InnerText = DatumDo.ToString("yyyy-MM-dd");
            oXml.GetElementsByTagName("Priznak-spisu")[0].InnerText = Spis ? "spis" : "pisemnost";

            oResult = ToXmlNode(m_oSslRef.Prehleddokumentu(ToXElement(oXml)));

            if (oResult == null)
                return null;

            XmlNodeList oXmlNodeList = oResult.SelectNodes("//ns:Prehled-dokumentu", ns);
            if (oXmlNodeList == null || oXmlNodeList.Count == 0)
                return null;

            foreach (XmlNode oNode in oXmlNodeList)
            {
                if (oNode.Name == null || oNode.Name.ToString() != "Prehled-dokumentu")
                    continue;

                oGinisDocument = new GinisDocument();
                oGinisDocument.IdDokumentu = oNode.SelectSingleNode("ns:Id-dokumentu", ns).InnerText;
                oGinisDocument.IdSpisu = oNode.SelectSingleNode("ns:Id-spisu", ns).InnerText != null ? oNode.SelectSingleNode("ns:Id-spisu", ns).InnerText : null;
                oGinisDocument.PriznakSpisu = oNode.SelectSingleNode("ns:Priznak-spisu", ns).InnerText == "spis" ? true : false;

                oGinisDocument.PriznakSpisu = oNode.SelectSingleNode("ns:Priznak-spisu", ns).InnerText == "spis" ? true : false;

                oGinisDocument.PriznakCj = oNode.SelectSingleNode("ns:Priznak-cj", ns).InnerText;
                oGinisDocument.PriznakFyzExistence = oNode.SelectSingleNode("ns:Priznak-fyz-existence", ns).InnerText;
                oGinisDocument.PriznakElObrazu = oNode.SelectSingleNode("ns:Priznak-el-obrazu", ns).InnerText;
                oGinisDocument.IdTypuDokumentu = oNode.SelectSingleNode("ns:Id-typu-dokumentu", ns).InnerText != null ? oNode.SelectSingleNode("ns:Id-typu-dokumentu", ns).InnerText : null;
                
                oDocs.Add(oGinisDocument);
            }

            return oDocs;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("GetDocuments:" + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' Procedura přidá soubor do existujícího dokumentu
    ///     ''' </summary>
    ///     ''' <param name="PID_Dokumentu">PID dokumentu</param>
    ///     ''' <param name="FileName">Plná cesta k souboru (c:\Temp\Upload\Text.pdf)</param>
    ///     ''' <param name="TypVazby">elektronicky-obraz/elektronicka-priloha</param>
    ///     ''' <param name="PopisSouboru">Popis souboru</param>
    ///     ''' <remarks></remarks>
    public string NahratSouborDoGinis(string PID_Dokumentu, string FileName, string TypVazby, string PopisSouboru)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        List<GinisDocument> oDocs = new List<GinisDocument>();

        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/pridat-soubor/response/v_1.0.0.0");

        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Pridat-soubor.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Id-dokumentu")[0].InnerText = PID_Dokumentu;
            oXml.GetElementsByTagName("Jmeno-souboru")[0].InnerText = Path.GetFileName(FileName);
            oXml.GetElementsByTagName("Typ-vazby")[0].InnerText = TypVazby;
            oXml.GetElementsByTagName("Popis-souboru")[0].InnerText = PopisSouboru;
            oXml.GetElementsByTagName("Data")[0].InnerText = ReturnFile(FileName);
            
            oResult = ToXmlNode(m_oSslRef.Pridatsoubor(ToXElement(oXml)));

            

            if (oResult == null)
            {
                
                throw new Exception("Soubor " + FileName + " nebyl přidán do GINISu. ");
            }

            

            return oResult.SelectSingleNode("//ns:Id-souboru", ns).InnerText;
        }
        catch (Exception ex)
        {
            

            OnError?.Invoke(ex.Message);
            throw new Exception("UploadFile" + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' vrátí seznam souborů v dokumentu GINIS
    ///     ''' </summary>
    ///     ''' <param name="PID_dokumentu">PID dokumentu</param>
    ///     ''' <returns>list souborů pro daný GINIS dokument</returns>
    ///     ''' <remarks></remarks>
    public List<GinisFile> SeznamSouboru(string PID_dokumentu)
    {
        GinisDocument oDokument;
        List<GinisFile> oSeznamSouboru = new List<GinisFile>();
        GinisFile oSoubor;
        try
        {
            // načti detail doc.
            oDokument = DetailDokumentu(PID_dokumentu);

            // přidej el. obraz, pokud existuje
            if (oDokument.PriznakElObrazu == "existuje" | oDokument.PriznakElObrazu == "existuje-neaut-konv")
            {
                oSoubor = new GinisFile();
                oSoubor.IdDokumentu = oDokument.IdDokumentu;
                oSoubor.IdSouboru = oDokument.IdSouboru;
                oSoubor.JmenoOrigSouboru = oDokument.JmenoSouboru;
                oSoubor.PopisSouboru = oDokument.PopisSouboru;
                oSoubor.TypVazby = "elektronicky-obraz";
                oSeznamSouboru.Add(oSoubor);
            }

            // přidej el. přílohy, pokud existují
            if (oDokument.SeznamPriloh.Count > 0)
                oSeznamSouboru.AddRange(oDokument.SeznamPriloh);

            return oSeznamSouboru;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("SeznamSouboru" + ex.Message.ToString());
        }
    }



    /// <summary>
    ///     ''' 
    ///     ''' </summary>
    ///     ''' <param name="PID_dokumentu">PID dokumentu</param>
    ///     ''' <param name="PID_souboru">PID souboru</param>
    ///     ''' <param name="TypVazby">typ vazby elektronicky-obraz/elektronicka-priloha</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public GinisFile StahnoutSouborZGinis(string PID_dokumentu, string PID_souboru, string TypVazby)
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        GinisFile oGinisFile;
        List<GinisDocument> oDocs = new List<GinisDocument>();

        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/ssl/wfl-dokument/nacist-soubor/response/v_1.0.0.0");

        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Nacist-soubor.xml"));
            // AppendIxsExtAttribute(oXml)
            oXml.GetElementsByTagName("Id-dokumentu")[0].InnerText = PID_dokumentu;
            oXml.GetElementsByTagName("Id-souboru")[0].InnerText = PID_souboru;
            oXml.GetElementsByTagName("Typ-vazby")[0].InnerText = TypVazby;

            oResult = ToXmlNode(m_oSslRef.Nacistsoubor(ToXElement(oXml)));
            if (oResult == null)
                throw new Exception("Soubor " + PID_souboru + " nebyl načten z GINISu. ");

            oGinisFile = new GinisFile();
            oGinisFile.JmenoOrigSouboru = oResult.SelectSingleNode("//ns:Jmeno-souboru", ns).InnerText;
            oGinisFile.JmenoTempSouboru = SaveToTempFile(oResult.SelectSingleNode("//ns:Data", ns).InnerText, Path.GetExtension(oGinisFile.JmenoOrigSouboru));

            return oGinisFile;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("DownloadFile: " + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' Vrátí seznam typů dokumentu (písemnosti)
    ///     ''' </summary>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public List<GinisDocumentType> SeznamTypuDokumentu()
    {
        XmlDocument oXml = new XmlDocument();
        XmlNode oResult;
        List<GinisDocumentType> oGinisDocTypes = new List<GinisDocumentType>();
        GinisDocumentType oGinisDocType;

        XmlNamespaceManager ns = new XmlNamespaceManager(oXml.NameTable);
        ns.AddNamespace("ns", "http://www.gordic.cz/xrg/gin/ciselnik/prehled-typu-dokumentu/response/v_1.0.0.0");

        try
        {
            oXml.Load(Path.Combine(m_sXmlTemplatesPath, "Prehled-typu-dokumentu.xml"));
            // AppendIxsExtAttribute(oXml)
            oResult = ToXmlNode(m_oGinRef.Prehledtypudokumentu(ToXElement(oXml)));

            if (oResult == null)
                return null;

            XmlNodeList oXmlNodeList = oResult.SelectNodes("//ns:Prehled-typu-dokumentu", ns);
            if (oXmlNodeList == null || oXmlNodeList.Count == 0)
                return null;

            foreach (XmlNode oNode in oXmlNodeList)
            {
                oGinisDocType = new GinisDocumentType();
                oGinisDocType.IdTypuDokumentu = oNode.SelectSingleNode("ns:Id-typu-dokumentu", ns).InnerText;
                oGinisDocType.IdNazevTypuDokumentu = oNode.SelectSingleNode("ns:Nazev-typu-dokumentu", ns).InnerText;

                oGinisDocTypes.Add(oGinisDocType);
            }

            return oGinisDocTypes;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("GetDocumentsTypes:" + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' Uloží data z webservice do souboru v Temp adresáři
    ///     ''' </summary>
    ///     ''' <param name="Data">Data v MIME 64</param>
    ///     ''' <param name="Extension">přípona souboru</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    private string SaveToTempFile(object Data, string Extension)
    {
        try
        {
            Guid g = Guid.NewGuid();
            var sFilePath = Path.Combine(System.IO.Path.GetTempPath(), g.ToString() + Extension);
            FileStream fs = File.Create(sFilePath);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(Convert.FromBase64String(Data as string));
            bw.Close();
            fs.Close();

            return sFilePath;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("SaveToTempFile: " + ex.Message.ToString());
        }
    }

    /// <summary>
    ///     ''' Vrátí soubor jako BASE64 string
    ///     ''' </summary>
    ///     ''' <param name="FileName"></param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    private string ReturnFile(string FileName)
    {
        try
        {
            System.IO.FileStream dd = System.IO.File.OpenRead(FileName);
            byte[] Bytes = new byte[dd.Length - 1 + 1];
            dd.Read(Bytes, 0, Bytes.Length);
            dd.Close();
            return Convert.ToBase64String(Bytes);
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            throw new Exception("ReturnFile: " + ex.Message.ToString());
        }
    }
}

