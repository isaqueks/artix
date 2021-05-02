using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ArtixPost
{
    public class ArtixXmlElement : Element
    {
        protected XmlElement _baseElement;

        private string renderValues(string outputRule, ParseMap.RenderContentFunction renderContent, ParseMap parseRules)
        {
            StringBuilder output = new StringBuilder(outputRule);
            int varIndex = outputRule.IndexOf("$");

            while (varIndex >= 0)
            {
                string variable = "$";
                for (int i = varIndex + 1; i < outputRule.Length; i++)
                {
                    char ch = outputRule[i];
                    if (ch != '@' && char.ToLower(ch) == char.ToUpper(ch)) break;
                    variable += ch;
                }

                string value = string.Empty;

                value = parseRules.GetValue(variable, this, renderContent);

                output.Replace(variable, value);

                varIndex = outputRule.IndexOf("$", varIndex + variable.Length);
            }

            return output.ToString();

        }

        private string renderContent(ParseMap parseRules)
        {
            StringBuilder renderedContent = new StringBuilder();

            foreach (XmlNode node in _baseElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Text)
                {
                    renderedContent.Append(node.OuterXml);
                }
                else if (node.NodeType == XmlNodeType.Element)
                {
                    XmlElement asElement = node as XmlElement;

                    bool found = false;
                    foreach (var child in this.Children)
                    {
                        var artixChild = ((ArtixXmlElement)child);
                        if (artixChild._baseElement == asElement)
                        {
                            renderedContent.Append(artixChild.Render(parseRules));
                            found = true;

                            break;
                        }
                    }

                    if (!found)
                        throw new Exception("Could not render an element.");
                }
            }

            return renderedContent.ToString();

        }

        public string Render(ParseMap parseRules)
        {
            
            string outputRule = parseRules.GetParseRule(TagName);
            string output = renderValues(outputRule, renderContent, parseRules);

            return output;
        }

        public ArtixXmlElement(XmlElement element) : base()
        {
            _baseElement = element;

            this.TagName = element.Name;
            this.InnerText = element.InnerText;
            this.InnerXml = element.InnerXml;
            this.OwnerText = FindOwnerText(element);

            foreach(XmlAttribute attr in element.Attributes)
            {
                this.Attributes.Add(attr.Name, attr.Value);
            }

            List<Element> children = new List<Element>();
            foreach(XmlNode childNode in element.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement asElement = childNode as XmlElement;
                    children.Add(new ArtixXmlElement(asElement));
                }
            }

            this.Children = children.ToArray();
        }
    }
}
