using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ArtixPost
{
    public abstract class Element
    {
        public Dictionary<string, string> Attributes { get; protected set; }
        public virtual string TagName { get; protected set; }
        public virtual string InnerText { get; protected set; }
        public virtual string OwnerText { get; protected set; }
        public virtual string InnerXml { get; protected set; }
        public virtual string OuterXML { get; protected set; }
        public virtual Element[] Children { get; protected set; }

        protected Element()
        {
            Attributes = new Dictionary<string, string>();
            Children = new Element[0];
        }

        public string GetAttribute(string attr)
        {
            if (!Attributes.ContainsKey(attr)) return null;
            return Attributes[attr];
        }

        protected static string FindOwnerText(System.Xml.XmlElement element)
        {
            string innerXml = element.InnerXml;

            if (element.ChildNodes.Count == 0)
                return innerXml;

            StringBuilder ownerXml = new StringBuilder();


            foreach(XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Text)
                    ownerXml.Append(node.OuterXml);
            }

            return ownerXml.ToString().Trim();

        }

    }
}
