﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI.Selection;
using MyRevit.MyTests.Utilities;
using MyRevit.MyTests.VLBase;
using MyRevit.Utilities;
using System;
using System.Collections.Generic;

namespace MyRevit.MyTests.DAA
{
    /// <summary>
    /// 标注对象
    /// </summary>
    public enum DAATargetType
    {
        /// <summary>
        /// 管道
        /// </summary>
        Pipe,
        /// <summary>
        /// 风管
        /// </summary>
        Duct,
        /// <summary>
        /// 桥架
        /// </summary>
        CableTray,
        /// <summary>
        /// 线管
        /// </summary>
        Conduit,
    }
    /// <summary>
    /// 标记样式
    /// </summary>
    public enum DAAAnnotationType
    {
        /// <summary>
        /// 系统缩写 管道尺寸 离地高度
        /// </summary>
        SPL,
        /// <summary>
        /// 系统缩写 离地高度
        /// </summary>
        SL,
        /// <summary>
        /// 管道尺寸 离地高度
        /// </summary>
        PL,
        /// <summary>
        /// 系统缩写 截面尺寸
        /// </summary>
        SP,
    }
    static class DAAAnnotationTypeEx
    {
        /// <summary>
        /// 获取 标注对应的族
        /// </summary>
        public static FamilySymbol GetAnnotationFamilySymbol(this DAAAnnotationType type, Document doc)
        {
            switch (type)
            {
                case DAAAnnotationType.SPL:
                    return DAAContext.GetSPLTagForRectangle(doc);
                case DAAAnnotationType.SL:
                    return DAAContext.GetSLTagForRectangle(doc);
                case DAAAnnotationType.PL:
                    return DAAContext.GetPLTagForRectangle(doc);
                case DAAAnnotationType.SP:
                    return DAAContext.GetSPTagForRectangle(doc);
                default:
                    throw new NotImplementedException("暂不支持该类型");
            }
        }
    }
        /// <summary>
        /// 离地模式
        /// </summary>
        public enum DAALocationType
    {
        /// <summary>
        /// 中心离地
        /// </summary>
        Center,
        /// <summary>
        /// 顶部离地
        /// </summary>
        Top,
        /// <summary>
        /// 底部离地
        /// </summary>
        Bottom,
    }
    /// <summary>
    /// 文字方式
    /// </summary>
    public enum DAATextType
    {
        /// <summary>
        /// 文字在线上
        /// </summary>
        Middle,
        /// <summary>
        /// 文字在线端
        /// </summary>
        Above,
    }

    public class DAAModel : VLModel
    {
        public ElementId TargetId { get; set; }




        public DAATargetType TargetType { set; get; }//标注对象
        public DAAAnnotationType AnnotationType { set; get; }//标记样式
        public DAALocationType LocationType { set; get; }//离地模式
        public DAATextType TextType { set; get; }//文字方式

        public List<ElementId> TargetIds { set; get; }//标记的目标对象


        #region 通用属性
        /// <summary>
        /// 离地模式前缀
        /// </summary>
        public string AnnotationPrefix { set; get; }
        public XYZ VerticalVector { get; private set; }
        public XYZ ParallelVector { get; private set; }
        #endregion

        public DAAModel() : base("")
        {
        }
        public DAAModel(string data) : base(data)
        {
        }

        public void UpdateVectors(Line locationCurve)
        {
            XYZ parallelVector = null;
            XYZ verticalVector = null;
            parallelVector = locationCurve.Direction;
            verticalVector = new XYZ(parallelVector.Y, -parallelVector.X, 0);
            parallelVector = LocationHelper.GetVectorByQuadrant(parallelVector, QuadrantType.OneAndFour);
            verticalVector = LocationHelper.GetVectorByQuadrant(verticalVector, QuadrantType.OneAndTwo);
            double xyzTolarance = 0.01;
            if (Math.Abs(verticalVector.X) > 1 - xyzTolarance)
                verticalVector = new XYZ(-verticalVector.X, -verticalVector.Y, verticalVector.Z);
            VerticalVector = verticalVector;
            ParallelVector = parallelVector;
        }

        internal object GetAnnotationFamily(Document document)
        {
            return AnnotationType.GetAnnotationFamilySymbol(document);
        }

        public override bool LoadData(string data)
        {
            if (string.IsNullOrEmpty(data))
                return false;
            //try
            //{
            //    StringReader sr = new StringReader(data);
            //    TargetId = sr.ReadFormatStringAsElementId();
            //    LineIds = sr.ReadFormatStringAsElementIds();
            //    TextNoteIds = sr.ReadFormatStringAsElementIds();
            //    TextNoteTypeElementId = sr.ReadFormatStringAsElementId();
            //    CSALocationType = sr.ReadFormatStringAsEnum<CSALocationType>();
            //    TextLocations = sr.ReadFormatStringAsXYZs();
            //    Texts = sr.ReadFormatStringAsStrings();
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    //TODO log
            //    return false;
            //}
            return true;
        }

        public override string ToData()
        {
            //public PAAAnnotationType AnnotationType { set; get; }//标记样式
            //public string AnnotationPrefix { set; get; }//离地模式前缀
            //public PAALocationType LocationType { set; get; }//离地模式
            //public PAATextType TextType { set; get; }//文字方式
            //public List<ElementId> LineIds { get; set; }//线对象
            //public ElementId GroupId { get; set; }//线组对象
            //public XYZ BodyEndPoint { get; set; }//干线终点
            //public XYZ BodyStartPoint { get; set; }//干线起点
            //public XYZ LeafEndPoint { set; get; }//支线终点
            //public XYZ TextLocation { set; get; }//文本定位坐标
            //public double CurrentFontSizeScale { set; get; }//当前文本大小比例 以毫米表示
            //public double CurrentFontHeight { set; get; }//当前文本高度 double, foot
            //public double CurrentFontWidthScale { set; get; }//当前文本 Revit中的宽度缩放比例


            //public ([\w<>]+) (\w+) .+
            //=>
            //sb.AppendItem($2);


            return "";
            //StringBuilder sb = new StringBuilder();
            //sb.AppendItem(TargetId);
            //sb.AppendItem(LineIds);
            //sb.AppendItem(TextNoteIds);
            //sb.AppendItem(TextNoteTypeElementId);
            //sb.AppendItem(CSALocationType);
            //sb.AppendItem(TextLocations);
            //sb.AppendItem(Texts);
            //return sb.ToData();
        }

        public ISelectionFilter GetFilter()
        {
            switch (TargetType)
            {
                case DAATargetType.Pipe:
                    return new ClassFilter(typeof(Pipe));
                case DAATargetType.Duct:
                    return new ClassFilter(typeof(Duct));
                case DAATargetType.CableTray:
                    return new ClassFilter(typeof(CableTray));
                case DAATargetType.Conduit:
                    return new ClassFilter(typeof(Conduit));
                default:
                    throw new NotImplementedException("未支持该类型的过滤:" + TargetType.ToString());
            }
        }
    }
}
