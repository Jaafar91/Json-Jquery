using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace jsonjquery
{
    class Program
    {
        static void Main(string[] args)
        {
            readJson();
        }
        public static void readJson()
        {

            JObject o1 = JObject.Parse(File.ReadAllText(@"jaf.json"));
            string htmlLocation = o1["html"].ToString();


            if (File.Exists(htmlLocation))
            {

                startGen();
                genOnLoad(o1["onLoad"].ToList());
                genEvents(o1["events"].ToList());
                genModels(o1["model"].ToList());
                endGen();
                //Console.WriteLine(File.ReadAllText(@"gen.js"));

                //System.Diagnostics.Process.Start(htmlLocation);
                //System.Diagnostics.Process.Start(@"gen.js");
                //Console.ReadLine();
            }
        }
        public static void startGen()
        {
            File.AppendAllText(@"gen.js", "$(document).ready(function(){" + "\n");
        }
        public static void endGen()
        {
            File.AppendAllText(@"gen.js", "});" + "\n");
            Console.ReadLine();
        }
        public static void addFeature(List<string> list, string feature)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string s in list)
            {
                string result = String.Format("\t$('{0}').prop('{1}',true);" + "\n", s, feature);
                File.AppendAllText(@"gen.js", result);
            }
        }
        public static void genEvents(List<JToken> events)
        {
            foreach (JToken myEvent in events)
            {
                foreach (JToken sub in getTokensList(myEvent))
                {
                    string result = "";
                    string selector = "";
                    string value = "";

                    foreach (JToken sub2 in getTokensList(sub))
                    {
                        JProperty p = sub2.Value<JProperty>();
                        //Console.ReadLine();
                        if (p.Value.Type == JTokenType.Array)
                        {
                            foreach (JToken t1 in p.Value.ToArray())
                            {
                                if (t1.Type == JTokenType.String)
                                {
                                    selector = getSelector(t1.ToString());
                                    result += String.Format("\t\t$({0})." + p.Name + "();" + "\n", selector);
                                }
                                else if (t1.Type == JTokenType.Object || t1.Type == JTokenType.Integer)
                                {
                                    JToken tmp = t1.ToList()[0].Value<JProperty>().Value;
                                    selector = getSelector(t1.ToList()[0].Value<JProperty>().Name);
                                    value = getValue(tmp);

                                    if (tmp.Type == JTokenType.String || tmp.Type == JTokenType.Integer)
                                        result += String.Format("\t\t$({0}).{1}({2});" + "\n", selector, p.Name, value);
                                    else if (t1.ToList()[0].Value<JProperty>().Value.Type == JTokenType.Object)
                                        result += String.Format("\t\t$({0}).{1}({2});" + "\n", selector, p.Name, value);
                                }
                            }
                        }
                        else if (p.Value.Type == JTokenType.Object)
                        {
                            foreach (JToken t2 in getTokensList(sub2))
                            {
                                selector = getSelector(t2.Value<JProperty>().Name);
                                foreach (JToken t3 in t2.ToArray())
                                {
                                    foreach (JToken t4 in t3.ToArray())
                                    {
                                        value = getValue(t4.Value<JProperty>().Value);
                                        result += String.Format("\t\t$({0}).{1}('{2}',{3});" + "\n", selector, p.Name, t4.Value<JProperty>().Name, value);
                                    }
                                }
                            }
                        }
                        else if (p.Value.Type == JTokenType.String)
                        {
                            result += "\t\t" + p.Value + ";\n";
                        }
                    }
                    File.AppendAllText(@"gen.js", String.Format("\n\t$('{0}').on('{1}',function(e){{\n{2}\t}});" + "\n", myEvent.Value<JProperty>().Name, sub.Value<JProperty>().Name, result));

                }
            }
        }
        public static void genOnLoad(List<JToken> events)
        {

            string result = "";
            string selector = "";
            string value = "";

            foreach (JToken sub2 in events)
            {
                JProperty p = sub2.Value<JProperty>();
                //Console.ReadLine();
                if (p.Value.Type == JTokenType.Array)
                {
                    foreach (JToken t1 in p.Value.ToArray())
                    {
                        if (t1.Type == JTokenType.String)
                        {
                            selector = getSelector(t1.ToString());
                            result += String.Format("\t$({0})." + p.Name + "();" + "\n", selector);
                        }
                        else if (t1.Type == JTokenType.Object || t1.Type == JTokenType.Integer)
                        {
                            JToken tmp = t1.ToList()[0].Value<JProperty>().Value;
                            selector = getSelector(t1.ToList()[0].Value<JProperty>().Name);
                            value = getValue(tmp);

                            if (tmp.Type == JTokenType.String || tmp.Type == JTokenType.Integer)
                                result += String.Format("\t\t$({0}).{1}({2});" + "\n", selector, p.Name, value);
                            else if (t1.ToList()[0].Value<JProperty>().Value.Type == JTokenType.Object)
                                result += String.Format("\t$({0}).{1}({2});" + "\n", selector, p.Name, value);
                        }
                    }
                }
                else if (p.Value.Type == JTokenType.Object)
                {
                    foreach (JToken t2 in getTokensList(sub2))
                    {
                        selector = getSelector(t2.Value<JProperty>().Name);
                        foreach (JToken t3 in t2.ToArray())
                        {
                            foreach (JToken t4 in t3.ToArray())
                            {
                                value = getValue(t4.Value<JProperty>().Value);
                                result += String.Format("\t$({0}).{1}('{2}',{3});" + "\n", selector, p.Name, t4.Value<JProperty>().Name, value);
                            }
                        }
                    }
                }
                else if (p.Value.Type == JTokenType.String)
                {
                    result += "\t" + p.Value + ";\n";
                }
            }
            File.AppendAllText(@"gen.js", String.Format("\n" + result));
        }

        public static void genModels(List<JToken> models)
        {
            foreach (JToken model in models)
            {
                string result = "";

                foreach (JToken model1 in model.ToList())
                {
                    result = String.Format("\n\t\t$.{0}('{1}',function(response){{}});", model1["method"], model1["url"]);
                }
                File.AppendAllText(@"gen.js", result);


            }
        }
        public List<string> getProperties(IEnumerable<JProperty> list)
        {
            List<string> strlist = new List<string>();
            foreach (JProperty prop in list)
            {
                strlist.Add(prop.Name);
            }
            return strlist;
        }

        public static bool isJqueryFunction(string label)
        {
            List<string> list = new List<string>(){
                "show","hide","focus","empty","remove"
            };
            return list.Contains(label);
        }
        public static bool isJqueryArray(string label)
        {
            List<string> list = new List<string>(){
                "addClass","removeClass"
            };
            return list.Contains(label);
        }
        public static bool isJqueryMultiple(string label)
        {
            List<string> list = new List<string>(){
                "css","prob","attr"
            };
            return list.Contains(label);
        }

        public static List<JToken> getTokensList(JToken token)
        {
            return token.Value<JProperty>().Value.ToList();
        }

        public static string getSelector(string selector)
        {
            if (selector != "this")
            {
                selector = "\"" + selector + "\"";
            }
            return selector;
        }
        public static string getValue(JToken token)
        {
            string str = token.ToString();
            if (token.Type == JTokenType.String)
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

    }
}
