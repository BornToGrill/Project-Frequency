using System;
using UnityEngine;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

public static class GlobalSettings {

    private static SettingsContainer _instance;
    private static bool _latestVersion;

    public static SettingsContainer Instance {
        get {
            try {
                SettingsContainer container = Load();
                _instance = container;
                return _instance;
            }
            catch {
                return _instance;
            }
        }
    }

    public static bool LatestVersion {
        get {
            if (!_latestVersion) {
                try {
                    _instance = Download();
                    _latestVersion = true;
                }
                catch {
                    return _latestVersion;
                }
            }
            return _latestVersion; 
        }
    }

    static GlobalSettings() {
        try {
            _instance = Download();
            Debug.Log("Settings file loaded from remote website");
        }
        catch {
            try {
                _instance = Load();
                Debug.LogWarning("Settings file loaded from local backup");
            }
            catch {
                _instance = new SettingsContainer();
                Save();
                Debug.LogError("New settings file created");
            }
        }
    }

    static void Save() {
        using (FileStream stream = new FileStream("Settings.xml", FileMode.Create)) {
            SettingsContainer container = new SettingsContainer();
            XmlWriterSettings writerSettings = new XmlWriterSettings() {
                OmitXmlDeclaration = true,
                Indent = true
            };
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (XmlWriter writer = XmlWriter.Create(stream, writerSettings)) {
                new XmlSerializer(typeof (SettingsContainer)).Serialize(writer, container, namespaces);
            }
        }
    }

    static SettingsContainer Load() {
        using (FileStream stream = new FileStream("Settings.xml", FileMode.Open)) {
            return (SettingsContainer)new XmlSerializer(typeof (SettingsContainer)).Deserialize(stream);
        }
    }

    static SettingsContainer Download() {
        using (QuickWebClient client = new QuickWebClient()) {
            client.DownloadFile("http://daniel-molenaar.com/FrequencySettings/Settings.xml", "Settings.xml");
            return Load();
        }
    }
}

public class QuickWebClient : WebClient {
    protected override WebRequest GetWebRequest(Uri address) {
        WebRequest request = base.GetWebRequest(address);
        request.Timeout = 7000;
        return request;
    }
}

public class SettingsContainer {

    public string ServerIp = "84.106.110.245";
    public int ServerPort = 9500;
}