using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace ArtixPost
{
    public class ParseMap
    {
        public delegate string TextFunction(string text);
        public delegate string RenderContentFunction(ParseMap rules);

        protected Dictionary<string, string> ParseDictionary;
        protected Dictionary<string, TextFunction> TextFuncions;

        public ParseMap()
        {
            ParseDictionary = new Dictionary<string, string>();
            TextFuncions = new Dictionary<string, TextFunction>
            {
                { "trim", (str) => str.Trim() },
                { "tolower", (str) => str.ToLower() },
                { "toupper", (str) => str.ToUpper() },
                { "escapenewline", (str) => str.Replace("\n", "<br />") },
                { "fsread", (str) => File.ReadAllText(str) }
            };
        }

        public void AddTextFunction(string name, TextFunction func)
        {
            TextFuncions.Add(name.ToLower(), func);
        }

        private string callTextFunction(string funcName, string param)
        {
            if (!TextFuncions.ContainsKey(funcName.ToLower()))
                throw new Exception($"No function called \"{funcName}\" was found!");

            return TextFuncions[funcName.ToLower()](param);
        }

        public void AddParseRule(string tag, string output)
        {
            ParseDictionary.Add(tag.ToLower(), output);
        }

        public string GetParseRule(string tag)
        {
            var key = tag.ToLower();
            if (!ParseDictionary.ContainsKey(key))
                throw new Exception($"No rule found for tag \"{key}\"!");

            return ParseDictionary[key];
        }

        public string GetValue(string value, ArtixXmlElement element, RenderContentFunction contentFunc)
        {
            string returnValue = null;

            if (!value.StartsWith("$"))
                throw new Exception("Value must start with $ symbol!");

            else if (value.Length == 1)
                throw new Exception("Invalid value '$'!");

            string expr = value.Substring(1).ToLower();

            string[] exprSplit = expr.Split("@");

            value = exprSplit[0];

            if (value == "content")
            {
                returnValue = contentFunc(this);
            }
            else if (value == "rawcontent")
            {
                returnValue = element.InnerXml;
            }
            else
            {
                string attr = element.GetAttribute(value);

                if (attr == null) returnValue = value;
                else returnValue = attr;
            }


            for (int i = 1; i < exprSplit.Length; i++)
            {
                string fnName = exprSplit[i];
                returnValue = callTextFunction(fnName, returnValue);
            }

            return returnValue;
        }

        public static ParseMap LoadFromXML(XmlDocument document)
        {
            ParseMap map = new ParseMap();

            XmlElement elements = document.DocumentElement;

            foreach(XmlNode node in elements)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                map.AddParseRule(node.Name, node.InnerXml);
            }

            return map;
        }

    }
}
