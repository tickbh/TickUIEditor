using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Utils
{
    class FileHelper
    {
        public static void ensurePathExist(String path)
        {
            if(!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }

        public static void ensureFileExist(String file)
        {
            if (!File.Exists(file))
            {
                file = GetFormatPath(file);
                if (file.LastIndexOf("/") != -1)
                {
                    ensurePathExist(file.Substring(0, file.LastIndexOf("/")));
                }
                StreamWriter w = new StreamWriter(file);
                w.Write("");
                w.Close();
            }
        }

        public static String GetFormatPath(String path)
        {
            path = path.Replace("\\/", "/");
            path = path.Replace("//", "/");
            path = path.Replace("\\", "/");
            return path;
        }

        public static String SubPath(String folder, String path, out bool success)
        {
            String result = path;
            success = false;
            path = GetFormatPath(path);
            folder = GetFormatPath(folder);
            if (path.StartsWith(folder))
            {
                if (folder.EndsWith("/"))
                {
                    result = path.Substring(folder.Length);
                }
                else
                {
                    result = path.Substring(folder.Length + 1);
                }
                success = true;
            }
            return result;
        }

        public static bool IsAbsolutePath(String path)
        {
            if(path.Length < 2) {
                 return false;
            }
            if (((path[0] >= 'a' && path[0] <= 'z')
                || (path[0] >= 'A' && path[0] <= 'Z')) && path[1] == ':')
            {
                return true;
            }
            return false;
        }

        public static String GetFullContent(String path)
        {
            if (!File.Exists(path))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            StreamReader reader = new StreamReader(path);
            String line = null;
            do 
            {
                line = reader.ReadLine();
                if(line != null) {
                    sb.AppendLine(line);
                }
            } while (line != null);
            reader.Close();
            return sb.ToString();
        }

        public static void SaveFileContent(String path, String content)
        {
            FileHelper.ensureFileExist(path);
            StreamWriter writer = new StreamWriter(path);
            writer.Write(content);
            writer.Close();
        }

        public static String GetFileName(String path)
        {
            path = GetFormatPath(path);
            int index = path.LastIndexOf("/");
            if (index == -1 || index == path.Length - 1)
            {
                return path;
            }
            return path.Substring(index + 1);
        }

        public static bool IsFilePath(String path)
        {
            return File.Exists(path);
        }

        public static bool IsFloderPath(String path)
        {
            return Directory.Exists(path);
        }

        
        public static void GetFiles(String filePath, ref List<String> result)
        {
            DirectoryInfo folder = new DirectoryInfo(filePath);
            FileInfo[] chldFiles = folder.GetFiles("*.*");
            foreach (FileInfo chlFile in chldFiles)
            {
                result.Add(chlFile.FullName);
            }
            DirectoryInfo[] chldFolders = folder.GetDirectories();
            foreach (DirectoryInfo chldFolder in chldFolders)
            {
                GetFiles(folder.FullName, ref result);
            }
        }

        public static bool CutPathToAnother(String srcPath, String destPath)
        {
            if (srcPath == null || destPath == null)
            {
                return false;
            }
            if (!IsFloderPath(destPath))
            {
                return false;
            }
            destPath += "/";
            if (IsFilePath(srcPath))
            {
                File.Copy(srcPath, destPath + GetFileName(srcPath), true);
                File.Delete(srcPath);
            }
            else
            {
                List<String> files = new List<String>();
                GetFiles(srcPath, ref files);
                srcPath = GetFormatPath(srcPath + "/");
                foreach (String file in files)
                {
                    String opFile = GetFormatPath(file).Substring(srcPath.Length);
                    File.Copy(file, destPath + opFile, true);
                    File.Delete(file);

                }
            }
            return true;
        }
    }
}
