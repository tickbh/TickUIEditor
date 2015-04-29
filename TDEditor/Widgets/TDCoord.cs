using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDEditor.Widgets
{
    public enum COORD_DIRECTION
    {
        DIRECTION_HORIZONTAL,
        DIRECTION_VERTICAL,
    }

    public partial class TDCoord : UserControl
    {
        private COORD_DIRECTION direction;
        private int minPoint = 0;
        private int maxPoint = 1024;
        private int curPoint;
        private Font _font;

        public TDCoord(COORD_DIRECTION direction)
        {
            InitializeComponent();
            this.direction = direction;
            _font = new Font("Arial", 10, FontStyle.Regular);

        }

        public void SetPoint(int minPoint = 0, int maxPoint = 1024)
        {
            this.minPoint = minPoint;
            this.maxPoint = maxPoint;
        }

        public void DrawHorizontal(Graphics painter, float x, float y, float width, float height)
        {
            int maxLine = (int)width;
            int minScale = (int)height / 4;
            int middleScale = (int)height / 2;
            int maxScale = (int)height;
            float pixelValue = width * 1.0f / (maxPoint - minPoint);
            int step = calcStep();
            int smallStep = step / 10;

            int drawMin = minPoint + Math.Abs(minPoint) % step;
            float drawMinLength = (drawMin - minPoint) * pixelValue;

            float smallDistance = pixelValue * step / 10;
            int offsetNum = drawMin - (int)(drawMinLength / smallDistance) * smallStep;
            float startPoint = drawMinLength - (int)(drawMinLength / smallDistance) * smallDistance;

            painter.FillRectangle(Brushes.Gray, x, y, width, height);
            Pen pen = new Pen(Color.Red); //画笔
            painter.DrawLine(pen, x, y, x + width, y);
            for (float pos = startPoint, index = 0; pos < maxLine; pos += smallDistance, ++index)
            {
                int currDrawNum = offsetNum + (int)index * smallStep;
                if (Math.Abs(currDrawNum) % step == 0)
                {
                    painter.DrawLine(pen, x + pos, y, x + pos, y + maxScale);
                    painter.DrawString(currDrawNum.ToString(), _font, new SolidBrush(pen.Color), new PointF(x + pos, y + maxScale));
                }
                else if (Math.Abs(currDrawNum) / smallStep % 10 == 5)
                {
                    painter.DrawLine(pen, x + pos, y, x + pos, y + middleScale);
                    if (maxLine > 500)
                    {
                        painter.DrawString(currDrawNum.ToString(), _font, new SolidBrush(pen.Color), new PointF(x + pos, y + middleScale));
                    }
                }
                else
                {
                    painter.DrawLine(pen, x + pos, y, x + pos, y + minScale);
                }

            }

            if (minPoint <= curPoint && curPoint <= maxPoint)
            {
                float pos = (curPoint - minPoint) * pixelValue;
                pen = new Pen(Color.Blue);
                painter.DrawLine(pen, x + pos, y, x + pos, y + height);
                painter.DrawString(curPoint.ToString(), _font, new SolidBrush(pen.Color), new PointF(x + pos, y + height / 2));
            }
        }
        public void DrawVertical(Graphics painter, float x, float y, float width, float height)
        {
            int maxLine = (int)height;
            int minScale = (int)width / 4;
            int middleScale = (int)width / 2;
            int maxScale = (int)width;
            float pixelValue = height * 1.0f / (maxPoint - minPoint);
            int step = calcStep();
            int smallStep = step / 10;

            int drawMin = minPoint + Math.Abs(minPoint) % step;
            float drawMinLength = (drawMin - minPoint) * pixelValue;

            float smallDistance = pixelValue * step / 10;

            int offsetNum = drawMin - (int)(drawMinLength / smallDistance) * smallStep;
            float startPoint = drawMinLength - (int)(drawMinLength / smallDistance) * smallDistance;

            painter.FillRectangle(Brushes.Gray, x, y, width, height);
            Pen pen = new Pen(Color.Red); //画笔
            painter.DrawLine(pen, x, y, x, y + height);
            for (float pos = startPoint, index = 0; pos < maxLine; pos += smallDistance, ++index)
            {
                int currDrawNum = offsetNum + (int)index * smallStep;
                if (Math.Abs(currDrawNum) % step == 0)
                {
                    painter.DrawLine(pen, x, y + pos, x + maxScale, y + pos);
                    painter.DrawString(currDrawNum.ToString(), _font, new SolidBrush(pen.Color), new PointF(x, y + pos));

                }
                else if (Math.Abs(currDrawNum) / smallStep % 10 == 5)
                {
                    painter.DrawLine(pen, x, y + pos, x + middleScale, y + pos);

                    if (maxLine > 500)
                    {
                        painter.DrawString(currDrawNum.ToString(), _font, new SolidBrush(pen.Color), new PointF(x, y + pos));

                    }
                }
                else
                {
                    painter.DrawLine(pen, x, y + pos, x + minScale, y + pos);
                }

            }

            if (minPoint <= curPoint && curPoint <= maxPoint)
            {
                float pos = (curPoint - minPoint) * pixelValue;
                pen = new Pen(Color.Blue);
                painter.DrawLine(pen, x, y + pos, x + width, y + pos);
                painter.DrawString(curPoint.ToString(), _font, new SolidBrush(pen.Color), new PointF(x, y + pos));
            }
        }
        public void setCurPoint(int point)
        {
            this.curPoint = point;
        }

        private static int[] steps = { 1000, 900, 800, 700, 600, 500, 400, 300, 200, 100, 80, 50, 40, 30, 20, 10, 5, 4, 3, 2, 1 };

        private int calcStep()
        {
            int step = (maxPoint - minPoint) / 5;
            for (int i = 0, len = steps.Length / sizeof(int); i < len; i++)
            {
                if (step > steps[i])
                {
                    step = steps[i];
                    break;
                }
            }
            return step;
        }
    }
}
