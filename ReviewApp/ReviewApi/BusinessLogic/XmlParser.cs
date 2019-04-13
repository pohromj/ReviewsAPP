using ReviewApi.Models.Artifact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ReviewApi.BusinessLogic
{
    public class XmlParser
    {
        public static List<string> NodesInXml(string xml)
        {
            List<string> artifacts = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlNode node in doc.DocumentElement)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    artifacts.Add(child.Name);
                }
                break;
            }
            return artifacts;
        }

        public static List<ArtifactIbm> CreateObjects(string xml)
        {
            List<ArtifactIbm> artifacts = new List<ArtifactIbm>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlNode node in doc.DocumentElement)
            {
                ArtifactIbm a = new ArtifactIbm();
                foreach (XmlNode child in node.ChildNodes)
                {
                    a.AddProperty(child.Name, child.InnerText);
                }
                artifacts.Add(a);
            }
            return artifacts;
        }
        public static List<JazzArtifact>CreateJazzObjects(string xml)
        {
            List<JazzArtifact> artifacts = new List<JazzArtifact>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlNode node in doc.DocumentElement)
            {
                JazzArtifact a = new JazzArtifact();
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "REFERENCE_ID")
                        a.IbmId = Convert.ToInt32(child.InnerText);
                    else if (child.Name == "URL1_title")
                        a.Name = child.InnerText;
                    else if (child.Name == "URL1")
                        a.Url = child.InnerText;
                }
                artifacts.Add(a);
            }
            return artifacts;
        }
    }
}
