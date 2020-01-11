using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;

namespace DataProcess
{
    namespace StringCase
    {
        public class StringDealer
        {
            /// <summary>
            /// 以yyyyMMdd的格式来产生自定义的DateTime
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static DateTime ChangeToDateTime(string s)
            {
                return DateTime.ParseExact(s, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            }
            /// <summary>
            /// 特定字符串在某字符串中出现的次数
            /// </summary>
            /// <param name="words">某字符串</param>
            /// <param name="key">特定字符串</param>
            /// <returns></returns>
            public int TotalWordNum(string words, string key)
            {
                int total = 0;
                int tmp = words.IndexOf(key);
                while (tmp != -1)
                {
                    total++;
                    tmp = words.IndexOf(key);
                    words = words.Remove(0, tmp + key.Length);
                }
                return total;

            }
            /// <summary>
            /// 以某字符为分割，取其中的内容
            /// </summary>
            /// <param name="words">被分割的字符串</param>
            /// <param name="key">分割字符</param>
            /// <returns></returns>
            public string[] MidIn(string words, char key)
            {
                List<string> res = new List<string>();
                bool isstart = false;
                List<char> temp = new List<char>();
                foreach (char t in words.ToCharArray())
                {
                    if (t == key)
                    {
                        isstart = !isstart;
                        if (isstart == true)
                        {
                            temp.Clear();
                            continue;
                        }
                    }
                    if (isstart == true)
                    {
                        temp.Add(t);
                    }
                    else
                    {
                        if (temp.Count > 0)
                        {
                            string tmp = new string(temp.ToArray());
                            temp.Clear();
                            res.Add(tmp);
                        }
                    }

                }
                return res.ToArray();
            }
            /// <summary>
            /// 以两个字符为分割，取其中的内容
            /// </summary>
            /// <param name="words">待分割的字符串</param>
            /// <param name="before">起始字符</param>
            /// <param name="after">结尾字符</param>
            /// <returns></returns>
            public string[] MidIn(string words, char before, char after)
            {
                List<string> res = new List<string>();
                bool isstart = false;
                List<char> temp = new List<char>();
                foreach (char t in words.ToCharArray())
                {
                    if (t == before)
                    {
                        isstart = true;
                        temp.Clear();
                        continue;
                    }
                    else if (t == after)
                    {
                        isstart = false;
                    }
                    if (isstart == true)
                    {
                        temp.Add(t);
                    }
                    else
                    {
                        if (temp.Count > 0)
                        {
                            string tmp = new string(temp.ToArray());
                            temp.Clear();
                            res.Add(tmp);
                        }
                    }
                }
                return res.ToArray();
            }
        }
    }
    namespace TestSpeedTool
    {
        internal class DataFunctionTester
        {
            /// <summary>
            /// 测试某种操作的所需的时间
            /// </summary>
            /// <param name="test">测试的接口</param>
            /// <returns></returns>
            public TimeSpan SpeedTest(ISpeedTest test)
            {
                DateTime start = DateTime.Now;
                test.TestWork();
                return DateTime.Now - start;
            }
            /// <summary>
            /// 需要计算时间的操作的接口
            /// </summary>
            public interface ISpeedTest
            {
                void TestWork();
            }
        }
    }
    namespace Xml
    {
        /// <summary>
        /// Xml的指定项
        /// </summary>
        public enum XmlItemType
        {
            /// <summary>
            /// 标签内容
            /// </summary>
            InnerText,
            /// <summary>
            /// 属性
            /// </summary>
            Attributes,
            /// <summary>
            /// 标签名
            /// </summary>
            Name
        }
        /// <summary>
        /// Xml处理
        /// </summary>
        public class XmlCase : XmlDocument
        {
            private string nodepath = "";
            public string Xpath
            {
                set
                {
                    nodepath = value;
                    try
                    {
                        NodeList = SelectSingleNode(value).ChildNodes;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                get => nodepath;
            }
            private XmlNodeList nodeList;
            public XmlNodeList NodeList
            {
                get => nodeList;
                set => nodeList = value;
            }
            /// <summary>
            /// 实例化，设置默认节点路径
            /// </summary>
            /// <param name="path"></param>
            public XmlCase(string path)
            {
                Load(path);
            }
            /// <summary>
            /// 默认的初始化
            /// </summary>
            public XmlCase()
            {

            }

            /// <summary>
            /// 创建一个xml文件
            /// </summary>
            /// <param name="RootElement">根节点名</param>
            /// <param name="SaveLocation">储存位置</param>
            public void MakeXml(string RootElement, string SaveLocation)
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);
                XmlElement XMLroot = doc.CreateElement(RootElement);
                doc.AppendChild(XMLroot);
                doc.Save(SaveLocation);
            }
            /// <summary>
            /// 计数Xml中特定节点下的所有子节点
            /// </summary>
            /// <param name="nodepath">需要计数的母节点路径</param>
            /// <returns></returns>
            public int XmlNodeCount(string nodepath)
            {
                return SelectSingleNode(nodepath).ChildNodes.Count;
            }
            public int XmlNodeCount()
            {
                return NodeList.Count;
            }
            /// <summary>
            /// 返回Innertext
            /// </summary>
            /// <param name="index">需要返回项的序号</param>
            /// <returns></returns>
            public string XmlGetInnerText(int index)
            {
                return NodeList.Item(index).InnerText;
            }
            /// <summary>
            /// 返回InnerText的值
            /// </summary>
            /// <param name="InnerName">需要返回项的标签名</param>
            /// <returns></returns>
            public string XmlGetInnerText(string InnerName)
            {
                int index = XmlItemIndex(InnerName);
                return nodeList.Item(index).InnerText;
            }
            /// <summary>
            /// 返回某个节点下的节点列表
            /// </summary>
            /// <param name="type">需要返回的值的类型</param>
            /// <param name="AttributesName">如果选择了属性类型，需要填写属性名</param>
            /// <returns></returns>
            public List<string> XmlItemList(XmlItemType type = XmlItemType.InnerText, string AttributesName = "")
            {
                List<string> res = new List<string>();
                switch (type)
                {
                    case XmlItemType.InnerText:
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            res.Add(nodeList.Item(i).InnerText);
                        }

                        break;
                    case XmlItemType.Attributes:
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            res.Add(nodeList.Item(i).Attributes[AttributesName].Value);
                        }

                        break;
                    case XmlItemType.Name:
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            res.Add(nodeList.Item(i).Name);
                        }

                        break;
                }
                return res;
            }
            /// <summary>
            /// 返回xml中节点的索引位
            /// </summary>
            /// <param name="value">需要返回项的标签内容</param>
            /// <param name="type">需要查找的类型</param>
            /// <returns></returns>
            public int XmlItemIndex(string value, XmlItemType type = XmlItemType.Name)
            {
                int index = -1;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    switch (type)
                    {
                        case XmlItemType.Name:
                            if (nodeList.Item(i).Name.Equals(value))
                            {
                                index = i;
                                break;
                            }
                            break;
                        case XmlItemType.InnerText:
                            if (nodeList.Item(i).InnerText.Equals(value))
                            {
                                index = i;
                                break;
                            }
                            break;
                        case XmlItemType.Attributes:
                            for (int j = 0; j < nodeList.Item(i).Attributes.Count; j++)
                            {
                                if (nodeList.Item(i).Attributes[j].Value.Equals(value))
                                {
                                    index = i;
                                    break;
                                }
                            }

                            break;
                    }
                }
                return index;
            }
            /// <summary>
            /// 获取某一个的属性值
            /// </summary>
            /// <param name="Innername">需要返回项的标签内容</param>
            /// <param name="AttributeName">需要返回的属性名</param>
            /// <returns></returns>
            public string XmlGetItemValue(string Innername, string AttributeName)
            {
                int index = -1;
                string result = "";
                index = XmlItemIndex(Innername);
                if (index != -1)
                {
                    result = nodeList.Item(index).Attributes[AttributeName].Value;
                }
                return result;
            }
            /// <summary>
            /// 获取某一个的属性值
            /// </summary>
            /// <param name="index">需要返回项的的序号</param>
            /// <param name="AttributeName">需要返回的属性名</param>
            /// <returns></returns>
            public string XmlGetItemValue(int index, string AttributeName)
            {
                return nodeList.Item(index).Attributes[AttributeName].Value;
            }
            /// <summary>
            /// 设置某一项的标签内容
            /// </summary>
            /// <param name="index">需要修改项的序号</param>
            /// <param name="value">修改的值</param>
            /// <returns></returns>
            public bool XmlChangeItemInnerText(int index, string value)
            {
                try
                {
                    nodeList.Item(index).InnerText = value;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            /// <summary>
            /// 修改某一项属性
            /// </summary>
            /// <param name="index">需要修改项的序号</param>
            /// <param name="AttributeName">需要修改的属性名</param>
            /// <param name="value">需要修改的属性值</param>
            public bool XmlChangeItemAttribute(int index, string AttributeName, string value)
            {
                try
                {
                    nodeList.Item(index).Attributes[AttributeName].Value = value;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// 删除某一项
            /// </summary>
            /// <param name="InnerName">需要删除项的标签内容</param>
            /// <returns></returns>
            public void XmlDeleteItem(string InnerName)
            {
                XmlNode xmlNode = SelectSingleNode(nodepath);
                XmlNodeList nodeList = xmlNode.ChildNodes;
                int index = 0;
                index = XmlItemIndex(InnerName);
                if (index != -1)
                {
                    xmlNode.RemoveChild(nodeList.Item(index));
                }
            }
            /// <summary>
            /// 删除某一项
            /// </summary>
            /// <param name="index">需要删除项的标签序号</param>
            /// <returns></returns>
            public void XmlDeleteItem(int index)
            {
                XmlNode xmlNode = SelectSingleNode(nodepath);
                XmlNodeList nodeList = xmlNode.ChildNodes;
                xmlNode.RemoveChild(nodeList.Item(index));
            }
            /// <summary>
            /// 增加一项
            /// </summary>
            /// <param name="Innername">增加项的标签名</param>
            /// <param name="InnerText">增加项的标签内容</param>
            /// <param name="AttributesName">增加项目的属性（可多个，需要与值相对应）</param>
            /// <param name="AttributeValues">增加项的属性值（可多个，需要与属性排序相对应）</param>
            /// <returns></returns>
            public bool XmlAddItem(string Innername, string InnerText, string[] AttributesName, string[] AttributeValues)
            {
                try
                {
                    XmlNode xmlNode = SelectSingleNode(nodepath);
                    XmlElement xmlElement = CreateElement(Innername);
                    for (int i = 0; i < AttributesName.Length; i++)
                    {
                        xmlElement.SetAttribute(AttributesName[i], AttributeValues[i]);
                    }
                    xmlElement.InnerText = InnerText;
                    xmlNode.AppendChild(xmlElement);
                    return true;
                }
                catch
                { return false; };
            }
            /// <summary>
            /// 增加一项
            /// </summary>
            /// <param name="Innername">增加项的标签名</param>
            /// <param name="InnerText">增加项的标签内容</param>
            /// <returns></returns>
            public bool XmlAddItem(string Innername, string InnerText)
            {
                try
                {
                    XmlNode xmlNode = SelectSingleNode(nodepath);
                    XmlElement xmlElement = CreateElement(Innername);
                    xmlElement.InnerText = InnerText;
                    xmlNode.AppendChild(xmlElement);
                    return true;
                }
                catch
                { return false; };
            }
        }
    }
    namespace Encryption
    {
        public class MD5Coder
        {
            /// <summary>
            /// 生成MD5码
            /// </summary>
            /// <param name="text">需要生成MD5码的字符串</param>
            /// <returns></returns>
            public static string MD5Encrypt(string text)
            {
                MD5 d5 = MD5Cng.Create();
                byte[] a = Encoding.UTF8.GetBytes(text);
                byte[] b = d5.ComputeHash(a);
                DataCase.NumberCase numberCase = new DataCase.NumberCase();
                string res = "";
                foreach (byte c in b)
                {
                    string x = numberCase.TenToSixTeen(c.ToString()).TrimStart(new char[] { '#' });
                    res += x;
                }
                return res;
            }
        }
        public class BaseEncryption
        {
            /// <summary>
            /// 加密
            /// </summary>
            /// <param name="content">需要加密的文本</param>
            /// <param name="key">加密密钥</param>
            /// <returns></returns>
            public static string Encrypt(string content, string key)
            {
                List<byte> Tkey = new List<byte>();
                List<byte> Tcontent = new List<byte>();
                Tkey = new List<byte>(Encoding.Default.GetBytes(key));
                Tcontent = new List<byte>(Encoding.Default.GetBytes(content));
                byte[] keynum = Tkey.ToArray();
                byte[] contentnum = Tcontent.ToArray();
                byte[] result = new byte[contentnum.Length];
                int z = 0;
                string houzhui = "";
                for (int i = 0; i < contentnum.Length; i++)
                {
                    if (z >= keynum.Length)
                    {
                        z = 0;
                    }
                    result[i] = byte.Parse((contentnum[i] % keynum[z]).ToString());
                    houzhui += contentnum[i] / keynum[z];
                    z++;
                }
                return Convert.ToBase64String(result) + " " + houzhui;
            }
            /// <summary>
            /// 解密
            /// </summary>
            /// <param name="Encode">需要解密的字符串</param>
            /// <param name="key">密钥</param>
            /// <returns></returns>
            public static string Decrypt(string Encode, string key)
            {
                List<byte> Tchushu = new List<byte>();
                string[] temp = Encode.Split(new char[] { ' ' });
                Tchushu = new List<byte>(Encoding.Default.GetBytes(key));
                byte[] chushu = Tchushu.ToArray();
                char[] shang = temp[1].ToCharArray();
                int[] S = new int[shang.Length];
                for (int i = 0; i < shang.Length; i++)
                {
                    S[i] = int.Parse(shang[i].ToString());
                }
                byte[] Ynum = Convert.FromBase64String(temp[0]);
                byte[] beichushu = new byte[shang.Length];
                int z = 0;
                for (int i = 0; i < shang.Length; i++)
                {
                    if (z >= chushu.Length)
                    {
                        z = 0;
                    }
                    beichushu[i] = byte.Parse((S[i] * chushu[z] + Ynum[i]).ToString());
                    z++;
                }
                string res = "";
                res = Encoding.Default.GetString(beichushu);
                return res;
            }
            /// <summary>
            /// 解密
            /// </summary>
            /// <param name="Encode">需要解密的字符串</param>
            /// <param name="key">密钥</param>
            /// <param name="encode">编码类型</param>
            /// <returns></returns>
            public virtual string Decrypt(string Encode, string key, Encoding encode)
            {
                List<byte> Tchushu = new List<byte>();
                string[] temp = Encode.Split(new char[] { ' ' });
                Tchushu = new List<byte>(encode.GetBytes(key));
                byte[] chushu = Tchushu.ToArray();
                char[] shang = temp[1].ToCharArray();
                int[] S = new int[shang.Length];
                for (int i = 0; i < shang.Length; i++)
                {
                    S[i] = int.Parse(shang[i].ToString());
                }
                byte[] Ynum = Convert.FromBase64String(temp[0]);
                byte[] beichushu = new byte[shang.Length];
                int z = 0;
                for (int i = 0; i < shang.Length; i++)
                {
                    if (z >= chushu.Length)
                    {
                        z = 0;
                    }
                    beichushu[i] = byte.Parse((S[i] * chushu[z] + Ynum[i]).ToString());
                    z++;
                }
                string res = "";
                res = encode.GetString(beichushu);
                return res;
            }
            /// <summary>
            /// 解密
            /// </summary>
            /// <param name="content">需要解密的流</param>
            /// <param name="key"></param>
            /// <param name="encode"></param>
            /// <returns></returns>
            public virtual string Decrypt(Stream content, string key, Encoding encode)
            {
                List<byte> Tchushu = new List<byte>();
                StreamReader reader = new StreamReader(content);
                string Encode = reader.ReadToEnd();
                reader.Dispose();
                string[] temp = Encode.Split(new char[] { ' ' });
                Tchushu = new List<byte>(encode.GetBytes(key));
                byte[] chushu = Tchushu.ToArray();
                char[] shang = temp[1].ToCharArray();
                int[] S = new int[shang.Length];
                for (int i = 0; i < shang.Length; i++)
                {
                    S[i] = int.Parse(shang[i].ToString());
                }
                byte[] Ynum = Convert.FromBase64String(temp[0]);
                byte[] beichushu = new byte[shang.Length];
                int z = 0;
                for (int i = 0; i < shang.Length; i++)
                {
                    if (z >= chushu.Length)
                    {
                        z = 0;
                    }
                    beichushu[i] = byte.Parse((S[i] * chushu[z] + Ynum[i]).ToString());
                    z++;
                }
                string res = "";
                res = encode.GetString(beichushu);
                return res;
            }
            /// <summary>
            /// 加密
            /// </summary>
            /// <param name="content">需要加密的字符串</param>
            /// <param name="key">密钥</param>
            /// <param name="encode">编码类型</param>
            /// <returns></returns>
            public virtual string Encrypt(string content, string key, Encoding encode)
            {

                List<byte> Tkey = new List<byte>();
                List<byte> Tcontent = new List<byte>();
                Tkey = new List<byte>(encode.GetBytes(key));
                Tcontent = new List<byte>(encode.GetBytes(content));
                byte[] keynum = Tkey.ToArray();
                byte[] contentnum = Tcontent.ToArray();
                byte[] result = new byte[contentnum.Length];
                int z = 0;
                string houzhui = "";
                for (int i = 0; i < contentnum.Length; i++)
                {
                    if (z >= keynum.Length)
                    {
                        z = 0;
                    }
                    result[i] = byte.Parse((contentnum[i] % keynum[z]).ToString());
                    houzhui += contentnum[i] / keynum[z];
                    z++;
                }
                return Convert.ToBase64String(result) + " " + houzhui;
            }
            /// <summary>
            /// 加密
            /// </summary>
            /// <param name="contents">需要加密的流</param>
            /// <param name="key">密钥</param>
            /// <param name="encode">编码类型</param>
            /// <returns></returns>
            public virtual string Encrypt(Stream contents, string key, Encoding encode)
            {

                List<byte> Tkey = new List<byte>();
                List<byte> Tcontent = new List<byte>();
                StreamReader reader = new StreamReader(contents);
                string content = reader.ReadToEnd();
                Tkey = new List<byte>(encode.GetBytes(key));
                Tcontent = new List<byte>(encode.GetBytes(content));
                byte[] keynum = Tkey.ToArray();
                byte[] contentnum = Tcontent.ToArray();
                byte[] result = new byte[contentnum.Length];
                int z = 0;
                string houzhui = "";
                for (int i = 0; i < contentnum.Length; i++)
                {
                    if (z >= keynum.Length)
                    {
                        z = 0;
                    }
                    result[i] = byte.Parse((contentnum[i] % keynum[z]).ToString());
                    houzhui += contentnum[i] / keynum[z];
                    z++;
                }
                return Convert.ToBase64String(result) + " " + houzhui;
            }
            internal string Decode(string x)
            {
                string r = "";
                char[] h = x.ToCharArray();
                for (int i = 0; i < h.Length - 1; i++)
                {
                    if (isint(h[i]) == true)
                    {
                        if (isint(h[i + 1]) == true)
                        {
                            r += h[i];
                        }
                        else
                        {
                            for (int j = 0; j < (h[i + 1] - 65); j++)
                            {
                                r += h[i];
                            }
                        }
                    }
                }
                return r;
            }
            internal bool isint(char h)
            {
                return h <= 57 && h >= 48;
            }
            internal string Recode(string x)
            {

                char[] h = x.ToCharArray();
                List<int> num = new List<int>();
                List<int> resnum = new List<int>();
                int nums = 0;
                int j = 0;
                for (int i = 0; j < h.Length;)
                {
                    for (j = i; j < h.Length; j++)
                    {
                        if (h[i] == h[j])
                        {
                            nums++;
                        }
                        if (j >= h.Length)
                        {
                            break;
                        }

                        if (h[i] != h[j])
                        {
                            num.Add(nums);
                            resnum.Add(h[i]);
                            nums = 0;
                            i = j;
                            break;
                        }
                    }
                }
                char t;
                string final = "";
                for (int i = 0; i < num.Count; i++)
                {
                    if (num[i] > 1)
                    {
                        t = (char)(65 + num[i]);
                        final += (char)resnum[i] + t.ToString();
                    }
                    else
                    {
                        final += (char)resnum[i];
                    }
                }
                t = (char)(65 + nums);
                final += h[h.Length - 1] + t.ToString(); ;
                return final;
            }
        }
        /// <summary>
        /// 自制加密算法
        /// </summary>
        public class Encryption : BaseEncryption
        {


            /// <summary>
            /// 进阶版加密（缩短了加密后的字符串长度）
            /// </summary>
            /// <param name="content">需要加密的内容</param>
            /// <param name="key">解锁密钥</param>
            /// <returns></returns>
            public string AdvancedEncrypt(string content, string key)
            {
                int num = 2;
                byte[] keynum = Encoding.UTF8.GetBytes(key);
                byte[] contentnum = Encoding.UTF8.GetBytes(content);
                List<byte[]> ContentN = new List<byte[]>();
                int partC = contentnum.Length / num;
                byte[] result = new byte[contentnum.Length];
                string[] houzhui = new string[2];
                Thread thread = new Thread(delegate ()
                {
                    int z = 0;
                    for (int j = 0; j < partC; j++)
                    {
                        if (z >= keynum.Length)
                        {
                            z = 0;
                        }
                        result[j] = byte.Parse((contentnum[j] % keynum[z]).ToString());
                        houzhui[0] += contentnum[j] / keynum[z];
                        z++;
                    }
                });
                Thread thread1 = new Thread(delegate ()
                {
                    int z = partC % key.Length;
                    for (int j = partC; j < contentnum.Length; j++)
                    {
                        if (z >= keynum.Length)
                        {
                            z = 0;
                        }
                        result[j] = byte.Parse((contentnum[j] % keynum[z]).ToString());
                        houzhui[1] += contentnum[j] / keynum[z];
                        z++;
                    }
                });
                thread.Start();
                thread1.Start();
                while (thread1.ThreadState.Equals(ThreadState.Running) || thread.ThreadState.Equals(ThreadState.Running))
                {
                }
                string res = Convert.ToBase64String(result);
                string hou = Recode(houzhui[0] + houzhui[1]);
                return res + " " + hou;
            }
            /// <summary>
            /// 进阶版解密（缩短了加密后的字符串长度）
            /// </summary>
            /// <param name="Encode">密文</param>
            /// <param name="key">解锁密钥</param>
            /// <returns></returns>
            public string AdvancedDecrypt(string Encode, string key)
            {
                string[] temp = Encode.Split(new char[] { ' ' });
                byte[] chushu = Encoding.UTF8.GetBytes(key);
                char[] shang = Decode(temp[1]).ToCharArray();
                int[] S = new int[shang.Length];
                for (int i = 0; i < shang.Length; i++)
                {
                    S[i] = int.Parse(shang[i].ToString());
                }
                byte[] Ynum = Convert.FromBase64String(temp[0]);
                byte[] beichushu = new byte[shang.Length];
                int partC = shang.Length / 2;
                Thread thread = new Thread(delegate ()
                {
                    int z = 0;
                    for (int i = 0; i < partC; i++)
                    {
                        if (z >= chushu.Length)
                        {
                            z = 0;
                        }
                        beichushu[i] = byte.Parse((S[i] * chushu[z] + Ynum[i]).ToString());
                        z++;
                    }
                });
                Thread thread1 = new Thread(delegate ()
                {
                    int z = partC % chushu.Length;
                    for (int j = partC; j < shang.Length; j++)
                    {
                        if (z >= chushu.Length)
                        {
                            z = 0;
                        }
                        beichushu[j] = byte.Parse((S[j] * chushu[z] + Ynum[j]).ToString());
                        z++;
                    }
                });
                thread.Start();
                thread1.Start();
                while (thread1.ThreadState.Equals(ThreadState.Running) || thread.ThreadState.Equals(ThreadState.Running))
                {
                }
                return Encoding.UTF8.GetString(beichushu);
            }
        }
    }
    namespace DataCase
    {
        /// <summary>
        /// 数据处理
        /// </summary>
        public class NumberCase
        {
            /// <summary>
            /// 检查字符串是否是数字
            /// </summary>
            /// <param name="z">需要检查的字符串</param>
            /// <returns></returns>
            public static bool IsNumbers(string z)
            {
                bool result = false;
                char[] array = z.ToArray();
                int index = 0;
                if (array[0] == '-')
                {
                    index = 1;
                }
                else if (array[0] == '.')
                {
                    return false;
                }
                else if (array[array.Length - 1] == '.')
                {
                    return false;
                }

                for (; index < array.Length; index++)
                {
                    if ((array[index] >= '0' && array[index] <= '9') || array[index] == '.')
                    {
                        result = true;
                        continue;
                    }
                    result = false;
                    break;
                }
                return result;
            }
            /// <summary>
            /// 二进制转换为十进制
            /// </summary>
            /// <param name="z">一个二进制数，以字符串形式</param>
            /// <returns></returns>
            public string TwoToTen(string z)
            {
                char[] y = z.ToCharArray();
                char[] x = new char[y.Length];
                int c = 0;
                for (int i = y.Length - 1; i >= 0; i--)
                {
                    x[c] = y[i];
                    c++;
                }
                double res = 0;
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] == '1')
                    {
                        res += Math.Pow(2, i);
                    }
                }
                return res.ToString();
            }
            /// <summary>
            /// 十进制转换为二进制
            /// </summary>
            /// <param name="z">一个10进制的数，以字符串形式</param>
            /// <returns></returns>
            public string TenToTwo(string z)
            {

                ulong x = ulong.Parse(z);
                List<ulong> a = new List<ulong>();
                while (x != 0)
                {
                    a.Add(x % 2);
                    x /= 2;
                }
                string yy = "";
                for (int i = a.Count - 1; i >= 0; i--)
                {
                    yy += a[i];
                }
                return yy;
            }
            private string ChangeToSiteen(string num)
            {
                int a = int.Parse(num);
                if (a >= 0 && a <= 15)
                {
                    string x = a.ToString();
                    switch (a)
                    {
                        case 10:
                            x = "A";
                            break;
                        case 11:
                            x = "B";
                            break;
                        case 12:
                            x = "C";
                            break;
                        case 13:
                            x = "D";
                            break;
                        case 14:
                            x = "E";
                            break;
                        case 15:
                            x = "F";
                            break;
                    }
                    return x;
                }
                else
                {
                    return null;
                }
            }
            private int ChangeToTen(char num)
            {
                int x = -1;
                switch (num)
                {
                    case 'A':
                        x = 10;
                        break;
                    case 'B':
                        x = 11;
                        break;
                    case 'C':
                        x = 12;
                        break;
                    case 'D':
                        x = 13;
                        break;
                    case 'E':
                        x = 14;
                        break;
                    case 'F':
                        x = 15;
                        break;
                }
                return x;
            }
            /// <summary>
            /// 检测开头是否有#以确认是否为十六进制数
            /// </summary>
            /// <param name="nums">需要检查的字符串</param>
            /// <returns></returns>
            public bool IsSixteen(string nums)
            {
                char[] test = nums.ToCharArray();
                return test[0] == '#';
            }
            /// <summary>
            /// 十六进制转换为十进制
            /// </summary>
            /// <param name="z">需要转换的字符串</param>
            /// <returns></returns>
            public string SixteenToTen(string z)
            {
                z = z.ToUpper();
                if (IsSixteen(z).Equals(true))
                {
                    char[] y = z.ToCharArray();
                    int[] x = new int[y.Length - 1];
                    int c = 0;
                    for (int i = y.Length - 1; i > 0; i--)
                    {
                        if (y[i] >= 65 && y[i] <= 70)
                        {
                            x[c] = ChangeToTen(y[i]);
                        }
                        else
                        {
                            x[c] = int.Parse(y[i].ToString());
                        }
                        c++;

                    }
                    double res = 0;
                    for (int i = 0; i < x.Length; i++)
                    {
                        res += x[i] * (Math.Pow(16, i));
                    }

                    return res.ToString();
                }
                else
                {
                    return "非十六进制";
                }
            }
            /// <summary>
            /// 十进制转十六进制
            /// </summary>
            /// <param name="z">需要转换的字符串</param>
            /// <returns></returns>
            public string TenToSixTeen(string z)
            {

                ulong x = ulong.Parse(z);
                string yy = "#";
                List<ulong> a = new List<ulong>();
                if (x != 0)
                {
                    while (x != 0)
                    {
                        a.Add(x % 16);
                        x /= 16;
                    }

                    for (int i = a.Count - 1; i >= 0; i--)
                    {
                        if (a[i] > 9 && a[i] < 16)
                        {
                            yy += ChangeToSiteen(a[i].ToString());
                        }
                        else
                        {
                            yy += a[i];
                        }
                    }
                }
                else
                {
                    yy = "#0";
                }

                if (yy.Length == 2)
                {
                    yy = yy.Insert(1, "0");
                }
                return yy;
            }
        }
        public class DataTool
        {
            /// <summary>
            /// 快速排序条件判定接口
            /// </summary>
            public interface ISortCondition
            {
                /// <summary>
                /// before大于等于key时返回true
                /// </summary>
                /// <param name="before">作为排序依据的对象</param>
                /// <param name="key">另一个作为排序依据的对象（排序的坐标值）</param>
                /// <returns></returns>
                bool SortConditionBefore(object before, object key);
                /// <summary>
                /// after小于等于key时返回true
                /// </summary>
                /// <param name="after">作为排序依据的对象</param>
                /// <param name="key">另一个作为排序依据的对象（排序的坐标值）</param>
                /// <returns></returns>
                bool SortConditionAfter(object after, object key);
            }
            private int sortUnit<T>(T[] array, ISortCondition sort, int low, int high)
            {
                T key = array[low];
                while (low < high)
                {
                    /*从后向前搜索比key小的值*/
                    while (sort.SortConditionBefore(array[high], key) == true && high > low)
                    {
                        --high;
                    }
                    /*比key小的放左边*/
                    array[low] = array[high];
                    /*从前向后搜索比key大的值，比key大的放右边*/
                    while (sort.SortConditionAfter(array[low], key) == true && high > low)
                    {
                        ++low;
                    }
                    /*比key大的放右边*/
                    array[high] = array[low];
                }
                /*左边都比key小，右边都比key大。//将key放在游标当前位置。//此时low等于high */
                array[low] = key;
                return high;
            }
            /// <summary>
            /// 自定义类型数组的排序
            /// </summary>
            /// <typeparam name="T">需要排序的对象类型</typeparam>
            /// <param name="array">需要排序的数组</param>
            /// <param name="sort">排序的判定函数</param>
            /// <param name="low">数组的起始位置，一般为0</param>
            /// <param name="high">数组的最后位置，一般为数组长度-1</param>
            public void CustomQuickSort<T>(T[] array, ISortCondition sort, int low, int high)
            {
                if (low >= high)
                {
                    return;
                }
                /*完成一次单元排序*/
                int index = sortUnit(array, sort, low, high);
                /*对左边单元进行排序*/
                CustomQuickSort(array, sort, low, index - 1);
                /*对右边单元进行排序*/
                CustomQuickSort(array, sort, index + 1, high);
            }
            private int sortUnit(int[] array, int low, int high)
            {
                int key = array[low];
                while (low < high)
                {
                    /*从后向前搜索比key小的值*/
                    while (array[high] >= key && high > low)
                    {
                        high--;
                    }
                    /*比key小的放左边*/
                    array[low] = array[high];
                    /*从前向后搜索比key大的值，比key大的放右边*/
                    while (array[low] <= key && high > low)
                    {
                        low++;
                    }
                    /*比key大的放右边*/
                    array[high] = array[low];
                }
                /*左边都比key小，右边都比key大。//将key放在游标当前位置。//此时low等于high */
                array[low] = key;
                return high;
            }
            private int sortUnit(double[] array, int low, int high)
            {
                double key = array[low];
                while (low < high)
                {
                    /*从后向前搜索比key小的值*/
                    while (array[high] >= key && high > low)
                    {
                        high--;
                    }
                    /*比key小的放左边*/
                    array[low] = array[high];
                    /*从前向后搜索比key大的值，比key大的放右边*/
                    while (array[low] <= key && high > low)
                    {
                        low++;
                    }
                    /*比key大的放右边*/
                    array[high] = array[low];
                }
                /*左边都比key小，右边都比key大。//将key放在游标当前位置。//此时low等于high */
                array[low] = key;
                return high;
            }
            private void QuickSort(int[] array, int low, int high)
            {
                if (low >= high)
                {
                    return;
                }
                /*完成一次单元排序*/
                int index = sortUnit(array, low, high);
                /*对左边单元进行排序*/
                QuickSort(array, low, index - 1);
                /*对右边单元进行排序*/
                QuickSort(array, index + 1, high);
            }
            private void QuickSort(double[] array, int low, int high)
            {
                if (low >= high)
                {
                    return;
                }
                /*完成一次单元排序*/
                int index = sortUnit(array, low, high);
                /*对左边单元进行排序*/
                QuickSort(array, low, index - 1);
                /*对右边单元进行排序*/
                QuickSort(array, index + 1, high);
            }
            /// <summary>
            /// 快速排序（适合数据量庞大的）
            /// </summary>
            /// <param name="array">需要排序的数组</param>
            /// <param name="low">数组的起始位置，一般为0</param>
            /// <param name="high">数组的最后位置，一般为数组长度-1</param>
            public void QuickCollate(int[] array, int low, int high)
            {
                QuickSort(array, low, high);
            }
            /// <summary>
            /// 快速排序（适合数据量庞大的）
            /// </summary>
            /// <param name="array">需要排序的数组</param>
            /// <param name="low">数组的起始位置，一般为0</param>
            /// <param name="high">数组的最后位置，一般为数组长度-1</param>
            public void QuickCollate(double[] array, int low, int high)
            {
                QuickSort(array, low, high);
            }
            /// <summary>
            /// 排序
            /// </summary>
            /// <param name="num">需要排序的数组</param>
            /// <param name="isBigToSmall">true:从大到小排序，false：以小到大排序</param>
            /// <returns></returns>
            public int[] SelectCollate(int[] num, bool isBigToSmall = true)
            {
                int[] res = new int[num.Length];
                int i, j, k;
                for (i = 0; i < num.Length - 1; i++)
                {
                    k = i;
                    for (j = i + 1; j < num.Length; j++)
                    {
                        if (num[k] > num[j])
                        {
                            k = j;
                        }
                    }
                    if (k != i)
                    {
                        int temp = num[k];
                        num[k] = num[i];
                        num[i] = temp;
                    }
                }
                switch (isBigToSmall)
                {
                    case true:
                        for (i = 0; i < num.Length; i++)
                        {
                            res[i] = num[num.Length - 1 - i];
                        }
                        break;
                    case false:
                        for (i = 0; i < num.Length; i++)
                        {
                            res[i] = num[i];
                        }
                        break;
                }
                return res;
            }

            /// <summary>
            /// 排序
            /// </summary>
            /// <param name="num">需要排序的数组</param>
            /// <param name="isBigToSmall">true:从大到小排序，false：以小到大排序</param>
            /// <returns></returns>
            public double[] SelectCollate(double[] num, bool isBigToSmall = true)
            {
                double[] res = new double[num.Length];
                int i, j, k;
                for (i = 0; i < num.Length - 1; i++)
                {
                    k = i;
                    for (j = i + 1; j < num.Length; j++)
                    {
                        if (num[i] > num[j])
                        {
                            k = j;
                        }
                    }
                    if (k != i)
                    {
                        double temp = num[k];
                        num[k] = num[i];
                        num[i] = temp;
                    }
                }
                switch (isBigToSmall)
                {
                    case true:
                        for (i = 0; i < num.Length; i++)
                        {
                            res[i] = num[num.Length - 1 - i];
                        }
                        break;
                    case false:
                        for (i = 0; i < num.Length; i++)
                        {
                            res[i] = num[i];
                        }
                        break;
                }
                return res;
            }
            /// <summary>
            /// 排序
            /// </summary>
            /// <param name="num">需要排序的数组</param>
            /// <param name="isBigToSmall">true:从大到小排序，false：以小到大排序</param>
            /// <returns></returns>
            public int[] BobCollate(int[] num, bool isBigToSmall = true)
            {
                int[] res = new int[num.Length];
                int i, j;
                for (i = 0; i < num.Length - 1; i++)
                {
                    for (j = 0; j < num.Length - 1 - i; j++)
                    {
                        if (num[j] > num[j + 1])
                        {
                            int temp = num[j];
                            num[j] = num[j + 1];
                            num[j + 1] = temp;
                        }
                    }
                }
                switch (isBigToSmall)
                {
                    case true:
                        for (i = 0; i < num.Length; i++)
                        {
                            res[i] = num[num.Length - 1 - i];
                        }
                        for (i = 0; i < res.Length; i++)
                        {
                            num[i] = res[i];
                        }
                        break;
                    case false:
                        for (i = 0; i < num.Length; i++)
                        {
                            res[i] = num[i];
                        }
                        break;
                }
                return res;
            }
            /// <summary>
            /// 排序
            /// </summary>
            /// <param name="num">需要排序的数组</param>
            /// <param name="isBigToSmall">true:从大到小排序，false：以小到大排序</param>
            /// <returns></returns>
            public double[] BobCollate(double[] num, bool isBigToSmall = true)
            {
                double[] res = new double[num.Length];
                int i, j;
                for (i = 0; i < num.Length - 1; i++)
                {
                    for (j = 0; j < num.Length - 1 - i; j++)
                    {
                        if (num[j] > num[j + 1])
                        {
                            double temp = num[j];
                            num[j] = num[j + 1];
                            num[j + 1] = temp;
                        }
                    }
                }
                switch (isBigToSmall)
                {
                    case true:
                        for (i = 0; i < num.Length; i++)
                        {
                            res[i] = num[num.Length - 1 - i];
                        }
                        for (i = 0; i < res.Length; i++)
                        {
                            num[i] = res[i];
                        }
                        break;
                    case false:
                        for (i = 0; i < num.Length; i++)
                        {
                            res[i] = num[i];
                        }
                        break;
                }
                return res;
            }
            /// <summary>
            /// 去除重复
            /// </summary>
            /// <param name="a">需要去重的数组</param>
            /// <returns></returns>
            public int[] RemoveSame(int[] a)
            {
                List<int> d = new List<int>(a);
                d.Sort();
                int time = d.Count - 1;
                while (time >= 1)
                {
                    for (; d[time - 1].Equals(d[time]);)
                    {
                        d.RemoveAt(time - 1);
                        time--;
                        if (time == 0)
                        {
                            break;
                        }
                    }
                    time--;
                }
                return d.ToArray();
            }
            /// <summary>
            /// 去除重复
            /// </summary>
            /// <param name="a">需要去重的数组</param>
            /// <returns></returns>
            public string[] RemoveSame(string[] a)
            {
                List<string> d = new List<string>(a);
                d.Sort();
                int time = d.Count - 1;
                while (time >= 1)
                {
                    for (; d[time - 1].Equals(d[time]);)
                    {
                        d.RemoveAt(time - 1);
                        time--;
                        if (time == 0)
                        {
                            break;
                        }
                    }
                    time--;
                }
                return d.ToArray();
            }
            /// <summary>
            /// 去除重复
            /// </summary>
            /// <param name="a">需要去重的数组</param>
            /// <returns></returns>
            public double[] RemoveSame(double[] a)
            {
                List<double> d = new List<double>(a);
                d.Sort();
                int time = d.Count - 1;
                while (time >= 1)
                {
                    for (; d[time - 1].Equals(d[time]);)
                    {
                        d.RemoveAt(time - 1);
                        time--;
                        if (time == 0)
                        {
                            break;
                        }
                    }
                    time--;
                }
                return d.ToArray();
            }
        }

    }

}

