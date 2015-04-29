using TDEditor.project;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Utils
{
    class ImageInner {
        public Image image;
        public int count;
        public String path;

        public ImageInner(Image image, int count, String path)
        {
            this.image = image;
            this.count = count;
            this.path = path;
        }
    }
    class ImageHelper
    {
        private static Dictionary<String, ImageInner> imageFactory = new Dictionary<String, ImageInner>();

        public static void releaseImage(Image image)
        {
            foreach (String key in imageFactory.Keys)
            {
                ImageInner inner = imageFactory[key];
                if (inner.image == image)
                {
                    if (--inner.count == 0)
                    {
                        inner.image.Dispose();
                        imageFactory.Remove(key);
                        break;
                    }
                }
            }
        }

        public static void releaseImage(String path)
        {
            String relativePath = UIProject.Instance().GetRelativePath(path);
            if (imageFactory.ContainsKey(relativePath))
            {
                ImageInner inner = imageFactory[relativePath];
                if (--inner.count == 0)
                {
                    inner.image.Dispose();
                    imageFactory.Remove(relativePath);
                }
            }
        }
        public static Image FromFileInc(String path)
        {
            String relativePath = UIProject.Instance().GetRelativePath(path);
            Image image = null;
            if (imageFactory.ContainsKey(relativePath))
            {
                ImageInner inner = imageFactory[relativePath];
                inner.count++;
                image = inner.image;
            } else {
                try
                {
                    image = Image.FromFile(UIProject.Instance().GetRealFile(path));
                }
                catch
                {
                    image = null;
                }
                if (image != null)
                {
                    imageFactory.Add(relativePath, new ImageInner(image, 1, relativePath));
                }
            }
            return image;
        }

        public static bool isImageFile(String file)
        {
            if (file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".bmp"))
            {
                return true;
            }
            return false;
        }
    }
}
