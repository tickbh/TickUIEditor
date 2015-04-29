using TDEditor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDEditor.project
{
    class UIProject
    {
        private static String configFile = Application.StartupPath + "/" + "config.ini";
        
        private static UIProject project;
        public static UIProject Instance()
        {
            if (project == null)
            {
                project = new UIProject();
            }
            return project;
        }

        public String projectPath;
        public String resourcePath;

        public String projectName
        {
            get
            {
                int index = projectPath.LastIndexOf("[\\/]", 1);
                if (index != -1)
                {
                    return projectPath.Substring(index);
                }
                return "project";
            }
        }

        private UIProject()
        {
            FileHelper.ensureFileExist(configFile);
        }

        public String getProjectPath()
        {
            if (projectPath == null)
            {
                projectPath = INIHelper.Read("Project", "Path", configFile);
                if (projectPath == null || projectPath.Length == 0)
                {
                    projectPath = Application.StartupPath + "/project/";
                }
                if (!FileHelper.IsAbsolutePath(projectPath))
                {
                    projectPath = Application.StartupPath + "/" + projectPath;
                }
                FileHelper.ensurePathExist(projectPath);
            }
            return projectPath;
        }

        public void openDefaultProject()
        {
            openProjectPath(getProjectPath(), true);
        } 

        public void openProjectPath(String path, bool openDefault = false)
        {
            String projectConfig = path + "/.project";
            if (!File.Exists(projectConfig))
            {
                FileHelper.ensureFileExist(projectConfig);
            }
            projectPath = path;
            resourcePath = path + "/res/";
            FileHelper.ensurePathExist(resourcePath);
            if (openDefault)
            {
                String oriPath = INIHelper.Read("Project", "Path", configFile);
                if (oriPath == null || oriPath.Length == 0)
                {
                    INIHelper.Write("Project", "Path", path, configFile);
                }
            }
            else
            {
                INIHelper.Write("Project", "Path", path, configFile);
            }
            
            EventManager.RaiserEvent(Constant.ProjectChange, this, null);
        }

        public String GetRealFile(String file)
        {
            if (FileHelper.IsAbsolutePath(file))
            {
                return file;
            }
            if (File.Exists(resourcePath + "/" + file))
            {
                return resourcePath + "/" + file;
            }
            else if (File.Exists(projectPath + "/" + file))
            {
                return projectPath + "/" + file;
            }
            else if (File.Exists(Application.StartupPath + "/" + file))
            {
                return Application.StartupPath + "/" + file;
            }
            
            return file;
        }

        public String GetRelativePath(String path)
        {
            bool success = false;
            String relativePath = FileHelper.SubPath(resourcePath, path, out success);
            if (success)
            {
                return relativePath;
            }
            relativePath = FileHelper.SubPath(projectPath, path, out success);
            if (success)
            {
                return relativePath;
            }
            relativePath = FileHelper.SubPath(Application.StartupPath, path, out success);
            if (success)
            {
                return relativePath;
            }
            return path;
        }

        public bool FileInProject(String path)
        {
            String relative = GetRelativePath(path);
            if (relative == path)
            {
                return false;
            }
            return true;
        }
    }
}
