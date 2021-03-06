﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using MyRevit.MyTests.BeamAlignToFloor;
using MyRevit.MyTests.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static PmSoft.Optimization.DrawingProduction.AnnotationCreater;
using MyRevit.MyTests.MepCurveAvoid;

namespace PmSoft.Optimization.DrawingProduction.Utils
{
    public class GraphicsDisplayerManager
    {
        #region 全自动绕弯

        internal static void Display(string path, List<AvoidElement> avoidElements, IEnumerable<ValueNode> winners)
        {
            if (avoidElements.Count() == 0)
                return;

            var lines = avoidElements.Select(c => Line.CreateBound((c.MEPCurve.Location as LocationCurve).Curve.GetEndPoint(0), (c.MEPCurve.Location as LocationCurve).Curve.GetEndPoint(1))).ToList();
            var maxX = (int)lines.Max(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Max(b => b.X));
            var minX = (int)lines.Min(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Min(b => b.X));
            var maxY = (int)lines.Max(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Max(b => b.Y));
            var minY = (int)lines.Min(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Min(b => b.Y));
            var graphicsDisplayer = new GraphicsDisplayer(minX, maxX, minY, maxY);
            //显示 线
            graphicsDisplayer.DisplayLines(lines, new Pen(Brushes.Black), false, false);
            //显示 线的ID
            var lineIds = avoidElements.Select(c => ((c.MEPCurve.Location as LocationCurve).Curve.GetEndPoint(0) + (c.MEPCurve.Location as LocationCurve).Curve.GetEndPoint(1)) / 2).ToList();
            var lineIdTexts = avoidElements.Select(c => c.MEPCurve.Id.IntegerValue.ToString()).ToList();
            graphicsDisplayer.DisplayPointText(lineIds, lineIdTexts, Brushes.DarkGreen);
            foreach (var avoidElement in avoidElements)
            {
                var elements = avoidElement.ConflictElements.Where(c => c.CompeteType == CompeteType.Winner);//!c.IsConnector&& 
                //显示 碰撞点结果 分组
                var locations = elements.Select(c => c.ConflictLocation).ToList();
                var texts = elements.Select(c => "W:" + c.AvoidEle.MEPCurve.Id.IntegerValue.ToString() + "-G:" + c.GroupId.ToString().Substring(0, 4)).ToList();
                graphicsDisplayer.DisplayPointText(locations, texts, Brushes.Red);

            }
            graphicsDisplayer.SaveTo(path);
        }
        #endregion

        #region 标注避让
        public static void Display(string path, VLTriangle triangle, List<Line> pipeLines, List<Line> pipeCollisions, List<BoundingBoxXYZ> crossedBoundingBoxes, List<BoundingBoxXYZ> uncrossedBoundingBoxes)
        {
            if (pipeLines.Count() == 0)
                return;

            var uncross = new Pen(Brushes.LightGray);
            var cross = new Pen(Brushes.Red);
            var self = new Pen(Brushes.Black);
            var maxX = (int)pipeLines.Max(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Max(b => b.X));
            var minX = (int)pipeLines.Min(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Min(b => b.X));
            var maxY = (int)pipeLines.Max(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Max(b => b.Y));
            var minY = (int)pipeLines.Min(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Min(b => b.Y));
            var offSetX = -minX;
            var offSetY = -minY;
            var graphicsDisplayer = new GraphicsDisplayer(minX, maxX, minY, maxY);
            int displayLength = 10;
            graphicsDisplayer.DisplayLines(pipeLines.Where(c => c.Length >= displayLength).ToList(), uncross, false, true);
            graphicsDisplayer.DisplayLines(pipeLines.Where(c => c.Length < displayLength).ToList(), uncross, false, false);
            graphicsDisplayer.DisplayLines(pipeCollisions, cross, false, true);
            graphicsDisplayer.DisplayClosedInterval(new List<XYZ>() { triangle.A, triangle.B, triangle.C }, self, false, true);
            foreach (var boundingBox in crossedBoundingBoxes)
                graphicsDisplayer.DisplayClosedInterval(GetPointsFromBoundingBox(boundingBox), cross, false, true);
            foreach (var boundingBox in uncrossedBoundingBoxes)
                graphicsDisplayer.DisplayClosedInterval(GetPointsFromBoundingBox(boundingBox), uncross, false, true);
            graphicsDisplayer.SaveTo(path);
        }

        private static List<XYZ> GetPointsFromBoundingBox(BoundingBoxXYZ bounding)
        {
            return new List<XYZ>()
            {
                bounding.Min,
                bounding.Min + new XYZ(0, (bounding.Max - bounding.Min).Y, 0),
                bounding.Max,
                bounding.Max - new XYZ(0, (bounding.Max - bounding.Min).Y, 0),
            };
        }

        internal static void Display(string path, List<Face> faces)
        {
            List<Line> lines = new List<Line>();
            List<Edge> edges = new List<Edge>();
            foreach (var face in faces)
            {
                foreach (EdgeArray edgeLoop in face.EdgeLoops)
                {
                    var points = GetPoints(edgeLoop);
                    for (int i = 0; i < points.Count - 1; i++)
                        lines.Add(Line.CreateBound(points[i], points[i + 1]));
                    lines.Add(Line.CreateBound(points[points.Count - 1], points[0]));
                }
            }

            if (lines.Count() == 0)
                return;

            var uncross = new Pen(Brushes.LightGray);
            var cross = new Pen(Brushes.Red);
            var self = new Pen(Brushes.Black);
            var maxX = (int)lines.Max(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Max(b => b.X));
            var minX = (int)lines.Min(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Min(b => b.X));
            var maxY = (int)lines.Max(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Max(b => b.Y));
            var minY = (int)lines.Min(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Min(b => b.Y));
            var offSetX = -minX;
            var offSetY = -minY;
            var graphicsDisplayer = new GraphicsDisplayer(minX, maxX, minY, maxY);
            graphicsDisplayer.DisplayLines(lines, uncross, false, true);
            graphicsDisplayer.SaveTo(path);
        }

        /// <summary>
        /// 获取边的所有点
        /// </summary>
        /// <param name="edgeArray"></param>
        /// <returns></returns>
        /// <summary>
        static List<XYZ> GetPoints(EdgeArray edgeArray)
        {
            List<XYZ> points = new List<XYZ>();
            List<List<XYZ>> pointsCollectionToDeal = new List<List<XYZ>>();
            foreach (Edge edge in edgeArray)
            {
                pointsCollectionToDeal.Add(edge.Tessellate().ToList());
            }
            while (pointsCollectionToDeal.Count > 1)
            {
                var current = pointsCollectionToDeal[0];
                if (points.Count == 0)
                {
                    foreach (var point in current)
                    {
                        points.Add(point);
                    }
                }
                else
                {
                    if (points.Last().VL_XYZEqualTo(current.First()))
                    {
                        for (int i = 1; i < current.Count(); i++)
                        {
                            var point = current[i];
                            points.Add(point);
                        }
                    }
                    else if (points.First().VL_XYZEqualTo(current.First()))
                    {
                        for (int i = 1; i < current.Count(); i++)
                        {
                            var point = current[i];
                            points.Insert(0, point);
                        }
                    }
                    else if (points.Last().VL_XYZEqualTo(current.Last()))
                    {
                        for (int i = current.Count() - 2; i >= 0; i--)
                        {
                            var point = current[i];
                            points.Add(point);
                        }
                    }
                    else if (points.First().VL_XYZEqualTo(current.Last()))
                    {
                        for (int i = current.Count() - 2; i >= 0; i--)
                        {
                            var point = current[i];
                            points.Insert(0, point);
                        }
                    }
                    else
                    {
                        var temp = pointsCollectionToDeal[0];
                        pointsCollectionToDeal.RemoveAt(0);
                        pointsCollectionToDeal.Add(temp);
                        continue;
                    }
                }
                pointsCollectionToDeal.RemoveAt(0);
            }
            return points;
        }
        #endregion

        #region 梁齐板分析支持
        //public static void Display(string path, SeperatePoints lineSeperatePoints, List<LevelOutLines> outLinesCollection)
        //{
        //    var maxX = (int)outLinesCollection.Max(c => c.OutLines.Max(v => v.Points.Max(b => b.X)));
        //    var minX = (int)outLinesCollection.Min(c => c.OutLines.Min(v => v.Points.Min(b => b.X)));
        //    var maxY = (int)outLinesCollection.Max(c => c.OutLines.Max(v => v.Points.Max(b => b.Y)));
        //    var minY = (int)outLinesCollection.Min(c => c.OutLines.Min(v => v.Points.Min(b => b.Y)));
        //    var offSetX = -minX;
        //    var offSetY = -minY;
        //    GraphicsDisplayer = new GraphicsDisplayer(maxX - minX, maxY - minY, offSetX, offSetY);
        //    foreach (var levelOutLines in outLinesCollection)
        //        foreach (var outLine in levelOutLines.OutLines)
        //            Display(outLine);
        //    GraphicsDisplayer.DisplayClosedInterval(lineSeperatePoints.AdvancedPoints.Select(c => c.Point).ToList(), new Pen(Brushes.Red), true);
        //    var randomValue = new Random().Next(10);
        //    GraphicsDisplayer.DisplayPointsText(lineSeperatePoints.AdvancedPoints.Select(c => c.Point).ToList(), Brushes.Red, randomValue, randomValue);
        //    GraphicsDisplayer.SaveTo(path);
        //}

        //static void Display(OutLine outLine)
        //{
        //    var randomValue = new Random().Next(10);
        //    GraphicsDisplayer.DisplayClosedInterval(outLine.Points, null, false);
        //    if (outLine.Points.Count <= 6)
        //        GraphicsDisplayer.DisplayPointsText(outLine.Points, null, randomValue, randomValue);

        //    foreach (var subOutLine in outLine.SubOutLines)
        //        Display(subOutLine);
        //}
        #endregion

        #region 多管标注分析支持
        //internal static void Display(string path, List<PipeAndNodePoint> pipes)
        //{
        //    var maxX = (int)pipes.Max(c => new XYZ[] { (c.Pipe.Location as LocationCurve).Curve.GetEndPoint(0), (c.Pipe.Location as LocationCurve).Curve.GetEndPoint(1) }.Max(v => v.X));
        //    var minX = (int)pipes.Min(c => new XYZ[] { (c.Pipe.Location as LocationCurve).Curve.GetEndPoint(0), (c.Pipe.Location as LocationCurve).Curve.GetEndPoint(1) }.Min(v => v.X));
        //    var maxY = (int)pipes.Max(c => new XYZ[] { (c.Pipe.Location as LocationCurve).Curve.GetEndPoint(0), (c.Pipe.Location as LocationCurve).Curve.GetEndPoint(1) }.Max(v => v.Y));
        //    var minY = (int)pipes.Min(c => new XYZ[] { (c.Pipe.Location as LocationCurve).Curve.GetEndPoint(0), (c.Pipe.Location as LocationCurve).Curve.GetEndPoint(1) }.Min(v => v.Y));
        //    var offSetX = -minX;
        //    var offSetY = -minY;
        //    var GraphicsDisplayer = new GraphicsDisplayer(maxX - minX, maxY - minY, offSetX, offSetY);
        //    GraphicsDisplayer.DisplayLines(pipes.Select(c => (c.Pipe.Location as LocationCurve).Curve as Line).ToList(), new Pen(Brushes.Black), true, true);
        //    GraphicsDisplayer.DisplayPoints(pipes.Select(c => c.NodePoint).ToList(), new Pen(Brushes.Red), true);
        //    GraphicsDisplayer.SaveTo(path);
        //}
        #endregion

        #region 结构做法标注
        public static void Display(string path, List<Line> lines, List<XYZ> textLocations)
        {
            if (lines.Count() == 0)
                return;

            var uncross = new Pen(Brushes.LightGray);
            var cross = new Pen(Brushes.Red);
            var self = new Pen(Brushes.Black);
            var maxX = (int)lines.Max(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Max(b => b.X));
            var minX = (int)lines.Min(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Min(b => b.X));
            var maxY = (int)lines.Max(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Max(b => b.Y));
            var minY = (int)lines.Min(c => new XYZ[] { c.GetEndPoint(0), c.GetEndPoint(1) }.Min(b => b.Y));
            minX--;
            minY--;
            maxX++;
            maxY++;
            var offSetX = -minX;
            var offSetY = -minY;
            var graphicsDisplayer = new GraphicsDisplayer(minX, maxX, minY, maxY);
            graphicsDisplayer.DisplayLines(lines, uncross, true, true);
            graphicsDisplayer.DisplayPoints(textLocations, Pens.Red, true);
            graphicsDisplayer.SaveTo(path);
        }
        #endregion
    }

    public class GraphicsDisplayer
    {
        Graphics CurrentGraphics;
        Image CurrentImage;
        int OffsetX;
        int OffsetY;
        int PaddingX = 100;
        int PaddingY = 100;
        int Height;
        int Width;

        public GraphicsDisplayer(int xMin, int xMax, int yMin, int yMax)
        {
            Init(xMin, xMax, yMin, yMax);
        }

        private void Init(int xMin, int xMax, int yMin, int yMax)
        {
            Width = xMax - xMin;
            Height = yMax - yMin;
            OffsetX = -xMin;
            OffsetY = -yMin;
            Scale = Math.Min(4000 / Width, 4000 / Height);
            Width = Scale * Width;
            Height = Scale * Height;
            CurrentImage = new Bitmap(Width + 2 * PaddingX, Height + 2 * PaddingY);
            CurrentGraphics = Graphics.FromImage(CurrentImage);
            CurrentGraphics.Clear(System.Drawing.Color.White);
        }

        int Scale = 5;
        Brush DefaultBrush = Brushes.DarkGray;
        Pen DefaultPen = new Pen(Brushes.Black);
        Font DefaultFont = new Font("宋体", 12, FontStyle.Regular);

        public void DisplayLines(List<Line> lines, Pen pen, bool needPoint = false, bool needText = false)
        {
            if (lines.Count == 0)
                return;
            foreach (var line in lines)
            {
                var p0 = GetPoint(line.GetEndPoint(0));
                var p1 = GetPoint(line.GetEndPoint(1));
                CurrentGraphics.DrawLines(pen ?? DefaultPen, new System.Drawing.Point[] { p0, p1 });
                if (needPoint)
                {
                    CurrentGraphics.DrawEllipse(pen ?? DefaultPen, p0.X, p0.Y, 5, 5);
                    CurrentGraphics.DrawEllipse(pen ?? DefaultPen, p1.X, p1.Y, 5, 5);
                }
                if (needText)
                {
                    var brush = pen.Brush;
                    var point = line.GetEndPoint(0);
                    CurrentGraphics.DrawString($"{ point.X.ToString("f2") },{ point.Y.ToString("f2") }", DefaultFont, brush ?? DefaultBrush, GetPoint(point));
                    point = line.GetEndPoint(1);
                    CurrentGraphics.DrawString($"{ point.X.ToString("f2") },{ point.Y.ToString("f2") }", DefaultFont, brush ?? DefaultBrush, GetPoint(point));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="pen">null for default</param>
        /// <param name="needText"></param>
        public void DisplayPoints(List<XYZ> points, Pen pen, bool needText = false)
        {
            if (points.Count == 0)
                return;
            foreach (var pXYZ in points)
            {
                var point = GetPoint(pXYZ);
                CurrentGraphics.DrawEllipse(pen ?? DefaultPen, point.X, point.Y, 5, 5);
                if (needText)
                {
                    var brush = (pen ?? DefaultPen).Brush;
                    CurrentGraphics.DrawString($"{(int)pXYZ.X },{(int)pXYZ.Y }", DefaultFont, brush, point);
                }
            }
        }
        /// <summary>
        /// 点的文本
        /// </summary>
        /// <param name="points"></param>
        /// <param name="brush">null for default</param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void DisplayPoints(List<XYZ> points, Brush brush, int offsetX = 0, int offsetY = 0)
        {
            if (points.Count == 0)
                return;
            foreach (var point in points)
                CurrentGraphics.DrawString($"{(int)Math.Round(point.X, 0) },{(int)Math.Round(point.Y, 0) }", DefaultFont, brush ?? DefaultBrush, GetPoint(point, offsetX, offsetY));
        }

        /// <summary>
        /// 点的文本
        /// </summary>
        /// <param name="points"></param>
        /// <param name="brush">null for default</param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void DisplayPointText(List<XYZ> points,List<string> texts, Brush brush, int offsetX = 0, int offsetY = 0)
        {
            if (points.Count == 0)
                return;
            for (int i = 0; i < points.Count(); i++)
            {
                var point = points[i];
                CurrentGraphics.DrawString($"{(int)Math.Round(point.X, 0) },{(int)Math.Round(point.Y, 0) }:{texts[i]}", DefaultFont, brush ?? DefaultBrush, GetPoint(point, offsetX, offsetY));
            }
        }
        /// <summary>
        /// 闭合区间
        /// </summary>
        /// <param name="points"></param>
        /// <param name="pen"></param>
        /// <param name="needPoint"></param>
        public void DisplayClosedInterval(List<XYZ> points, Pen pen, bool needPoint = false, bool needText = false)
        {
            if (points.Count == 0)
                return;
            var scaledPoints = points.Select(c => GetPoint(c)).ToList();
            scaledPoints.Add(GetPoint(points.First()));
            CurrentGraphics.DrawLines(pen ?? DefaultPen, scaledPoints.ToArray());
            if (needPoint)
            {
                foreach (var point in scaledPoints)
                    CurrentGraphics.DrawEllipse(pen ?? DefaultPen, point.X, point.Y, 5, 5);
            }
            if (needText)
            {
                var brush = pen.Brush;
                foreach (var point in points)
                    CurrentGraphics.DrawString($"{(int)Math.Round(point.X, 0) },{(int)Math.Round(point.Y, 0) }", DefaultFont, brush ?? DefaultBrush, GetPoint(point));
            }
        }


        /// <summary>
        /// XYZ=>Point的通用转换
        /// </summary>
        /// <param name="c"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns></returns>
        private System.Drawing.Point GetPoint(XYZ c, int offsetX = 0, int offsetY = 0)
        {
            return new System.Drawing.Point((int)Math.Round(c.X * Scale, 0) + OffsetX * Scale + PaddingX + offsetX, Height - (int)Math.Round(c.Y * Scale, 0) - OffsetY * Scale + PaddingY + offsetY);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="path"></param>
        public void SaveTo(string path)
        {
            CurrentImage.Save(path);
        }
    }

    public class GraphicsDisplayerV2
    {
        static Graphics CurrentGraphics;
        static Image CurrentImage;
        static int OffsetX;
        static int OffsetY;
        static int PaddingX;
        static int PaddingY;
        static int Height;
        static int Width;
        static int Scale;

        public static Brush DefaultBrush = Brushes.DarkGray;
        public static Pen DefaultPen = new Pen(Brushes.Black);
        public static Font DefaultFont = new Font("宋体", 12, FontStyle.Regular);

        private static bool Init(double xMin, double xMax, double yMin, double yMax, int size = 1000)
        {
            var width = xMax - xMin;
            var height = yMax - yMin;
            if (width == 0 || height == 0)
                return false;
            Scale = (int)Math.Min(size / width, size / height);
            OffsetX = -(int)xMin;
            OffsetY = -(int)yMin;
            Width = (int)(Scale * width);
            Height = (int)(Scale * height);
            PaddingY = PaddingX = Math.Min(Width, Height) / 5;
            CurrentImage = new Bitmap(Width + 2 * PaddingX, Height + 2 * PaddingY);
            CurrentGraphics = Graphics.FromImage(CurrentImage);
            CurrentGraphics.Clear(System.Drawing.Color.White);
            return true;
        }

        public static void Display(double xMin, double xMax, double yMin, double yMax, string path, Action display)
        {
            if (!Init(xMin, xMax, yMin, yMax))
                return;
            display();
            SaveTo(path);
        }

        public static void DisplayLines(List<List<XYZ>> lineLoops, Pen pen, bool needPoint = false, bool needText = false)
        {
            foreach (var linePoints in lineLoops)
            {
                int pointDensity = linePoints.Count() < 30 ? 1 : linePoints.Count() / 10;
                for (int i = 0; i < linePoints.Count() - 1; i++)
                {
                    var p0 = linePoints[i];
                    var p1 = linePoints[i + 1];
                    var point0 = GetPoint(p0);
                    var point1 = GetPoint(p1);
                    CurrentGraphics.DrawLines(pen ?? DefaultPen, new System.Drawing.Point[] { point0, point1 });
                    if (needPoint)
                    {
                        CurrentGraphics.DrawEllipse(pen ?? DefaultPen, point0.X, point0.Y, 5, 5);
                        CurrentGraphics.DrawEllipse(pen ?? DefaultPen, point1.X, point1.Y, 5, 5);
                    }
                    if (needText && i % pointDensity == 0)
                    {
                        var brush = pen.Brush;
                        CurrentGraphics.DrawString($"{ p0.X.ToString("f2") },{ p0.Y.ToString("f2") }", DefaultFont, brush ?? DefaultBrush, point0);
                        CurrentGraphics.DrawString($"{ p1.X.ToString("f2") },{ p1.Y.ToString("f2") }", DefaultFont, brush ?? DefaultBrush, point1);
                    }
                }
            }

        }

        public static void DisplayLines(List<Line> lines, Pen pen, bool needPoint = false, bool needText = false)
        {
            if (lines.Count == 0)
                return;
            foreach (var line in lines)
            {
                var p0 = line.GetEndPoint(0);
                var p1 = line.GetEndPoint(1);
                var point0 = GetPoint(p0);
                var point1 = GetPoint(p1);
                CurrentGraphics.DrawLines(pen ?? DefaultPen, new System.Drawing.Point[] { point0, point1 });
                if (needPoint)
                {
                    CurrentGraphics.DrawEllipse(pen ?? DefaultPen, point0.X, point0.Y, 5, 5);
                    CurrentGraphics.DrawEllipse(pen ?? DefaultPen, point1.X, point1.Y, 5, 5);
                }
                if (needText)
                {
                    var brush = pen.Brush;
                    var point = line.GetEndPoint(0);
                    CurrentGraphics.DrawString(GetPointString(p0), DefaultFont, brush ?? DefaultBrush, GetPoint(point));
                    point = line.GetEndPoint(1);
                    CurrentGraphics.DrawString(GetPointString(p0), DefaultFont, brush ?? DefaultBrush, GetPoint(point));
                }
            }
        }

        public static string GetPointString(XYZ p0)
        {
            return $"{ p0.X.ToString("f2") },{ p0.Y.ToString("f2") }";
        }

        public static void DisplayPoints(List<XYZ> points, Pen pen, bool needText = false)
        {
            if (points.Count == 0)
                return;
            foreach (var pXYZ in points)
            {
                var point = GetPoint(pXYZ);
                CurrentGraphics.DrawEllipse(pen ?? DefaultPen, point.X, point.Y, 5, 5);
                if (needText)
                {
                    var brush = (pen ?? DefaultPen).Brush;
                    CurrentGraphics.DrawString(GetPointString(pXYZ), DefaultFont, brush, point);
                }
            }
        }

        public static void DisplayClosedInterval(List<XYZ> points, Pen pen, bool needPoint = false, bool needText = false)
        {
            if (points.Count == 0)
                return;
            var scaledPoints = points.Select(c => GetPoint(c)).ToList();
            scaledPoints.Add(GetPoint(points.First()));
            CurrentGraphics.DrawLines(pen ?? DefaultPen, scaledPoints.ToArray());
            if (needPoint)
            {
                foreach (var point in scaledPoints)
                    CurrentGraphics.DrawEllipse(pen ?? DefaultPen, point.X, point.Y, 5, 5);
            }
            if (needText)
            {
                var brush = pen.Brush;
                foreach (var point in points)
                    CurrentGraphics.DrawString(GetPointString(point), DefaultFont, brush ?? DefaultBrush, GetPoint(point));
            }
        }

        public static void DisplayPointsText(List<XYZ> points, Brush brush, int offsetX = 0, int offsetY = 0)
        {
            if (points.Count == 0)
                return;
            foreach (var point in points)
                CurrentGraphics.DrawString(GetPointString(point), DefaultFont, brush ?? DefaultBrush, GetPoint(point, offsetX, offsetY));
        }

        static System.Drawing.Point GetPoint(XYZ c, int offsetX = 0, int offsetY = 0)
        {
            return new System.Drawing.Point((int)Math.Round(c.X * Scale, 0) + OffsetX * Scale + PaddingX + offsetX, Height - (int)Math.Round(c.Y * Scale, 0) - OffsetY * Scale + PaddingY + offsetY);
        }

        public static void SaveTo(string path)
        {
            CurrentImage.Save(path);
        }
    }
}
