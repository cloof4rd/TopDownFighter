using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YogiGameCore.Data

{
    public class CSVTool
    {
        private static char _csvSeparator = ',';

        private static bool _trimColumns = false;

        //获取一个单元格的写入格式
        public static string GetCSVFormat(string str)
        {
            string tempStr = str;
            if (str.Contains(",") || str.Contains("\""))
            {
                tempStr = str.Replace("\"", "\"\"");
                tempStr = "\"" + tempStr + "\"";
            }

            return tempStr;
        }

        //获取一行的写入格式
        public static string GetCSVFormatLine(List<string> strList)
        {
            string tempStr = "";
            for (int i = 0; i < strList.Count - 1; i++)
            {
                string str = strList[i];
                tempStr = tempStr + GetCSVFormat(str) + ",";
            }

            tempStr = tempStr + GetCSVFormat(strList[strList.Count - 1]) + "\r\n";
            return tempStr;
        }

        //解析一行
        public static List<string> ParseLine(string line)
        {
            StringBuilder _columnBuilder = new StringBuilder();
            List<string> Fields = new List<string>();
            bool inColum = false; //是否是在一个列元素里
            bool inQuotes = false; //是否需要转义
            bool isNotEnd = false; //读取完毕未结束转义
            _columnBuilder.Remove(0, _columnBuilder.Length);

            //空行也是一个空元素，一个逗号是2个空元素
            if (line == "")
            {
                Fields.Add("");
            }

            // Iterate through every character in the line  遍历行中的每个字符
            for (int i = 0; i < line.Length; i++)
            {
                char character = line[i];

                //If we are not currently inside a column   如果我们现在不在一列中
                if (!inColum)
                {
                    // If the current character is a double quote then the column value is contained within
                    //如果当前字符是双引号，则列值包含在内
                    // double quotes, otherwise append the next character
                    //双引号，否则追加下一个字符
                    inColum = true;
                    if (character == '"')
                    {
                        inQuotes = true;
                        continue;
                    }
                }

                // If we are in between double quotes   如果我们处在双引号之间
                if (inQuotes)
                {
                    if ((i + 1) == line.Length) //这个字符已经结束了整行
                    {
                        if (character == '"') //正常转义结束，且该行已经结束
                        {
                            inQuotes = false;
                            continue;
                        }
                        else //异常结束，转义未收尾
                        {
                            isNotEnd = true;
                        }
                    }
                    else if (character == '"' && line[i + 1] == _csvSeparator) //结束转义，且后面有可能还有数据
                    {
                        inQuotes = false;
                        inColum = false;
                        i++; //跳过下一个字符
                    }
                    else if (character == '"' && line[i + 1] == '"') //双引号转义
                    {
                        i++; //跳过下一个字符
                    }
                    else if (character == '"') //双引号单独出现（这种情况实际上已经是格式错误，为了兼容暂时不处理）
                    {
                        throw new System.Exception("格式错误，错误的双引号转义");
                    }
                    //其他情况直接跳出，后面正常添加
                }
                else if (character == _csvSeparator)
                {
                    inColum = false;
                }

                // If we are no longer in the column clear the builder and add the columns to the list
                ////结束该元素时inColumn置为false，并且不处理当前字符，直接进行Add
                if (!inColum)
                {
                    Fields.Add(_trimColumns ? _columnBuilder.ToString().Trim() : _columnBuilder.ToString());
                    _columnBuilder.Remove(0, _columnBuilder.Length);
                }
                else //追加当前列
                {
                    _columnBuilder.Append(character);
                }
            }

            // If we are still inside a column add a new one （标准格式一行结尾不需要逗号结尾，而上面for是遇到逗号才添加的，为了兼容最后还要添加一次）
            if (inColum)
            {
                if (isNotEnd)
                {
                    _columnBuilder.Append("\r\n");
                }

                Fields.Add(_trimColumns ? _columnBuilder.ToString().Trim() : _columnBuilder.ToString());
            }
            else //如果inColumn为false，说明已经添加，因为最后一个字符为分隔符，所以后面要加上一个空元素
            {
                Fields.Add("");
            }

            return Fields;
        }

        //读取文件
        public static List<List<string>> Read(string filePath)
        {
            List<List<string>> result = new List<List<string>>();
            string content = File.ReadAllText(filePath,Encoding.GetEncoding("gb2312"));
            string[] lines = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                List<string> line = ParseLine(lines[i]);
                result.Add(line);
            }

            return result;
        }

        //写入文件
        public static void Write(string filePath, Encoding encoding, List<List<string>> result)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < result.Count; i++)
            {
                List<string> line = result[i];
                builder.Append(GetCSVFormatLine(line));
            }

            File.WriteAllText(filePath, builder.ToString(), encoding);
        }


    }
}