﻿using Autodesk.Revit.DB;
using PmSoft.Common.CADBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyRevit.Utilities
{
    public class VLUnitTransUtils
    {
        #region 英寸毫米转化
        /// <summary>
        /// 毫米转化为 英寸
        /// </summary>
        /// <param name="mmVal"></param>
        /// <returns></returns>
        public static double MMToFeet(double mmVal)
        {
            return mmVal / kFeetToMM;
        }

        /// <summary>
        /// 坐标点：毫米转化为英寸
        /// </summary>
        /// <param name="temPt"></param>
        /// <returns></returns>
        public static XYZ PointMMToFeet(XYZ temPt)
        {
            return new XYZ(MMToFeet(temPt.X), MMToFeet(temPt.Y), MMToFeet(temPt.Z));
        }

        /// <summary>
        /// 坐标点：毫米转化为英寸
        /// </summary>
        /// <param name="temPt"></param>
        /// <returns></returns>
        public static CPoint3d MMToFeet(CPoint3d temPt)
        {
            return new CPoint3d(MMToFeet(temPt.x), MMToFeet(temPt.y), MMToFeet(temPt.z));
        }

        public static void MMToFeet(ref CPoint3d pt)
        {
            pt.x = MMToFeet(pt.x);
            pt.y = MMToFeet(pt.y);
            pt.z = MMToFeet(pt.z);
        }

        /// <summary>
        /// 矢量：毫米转化为英寸
        /// </summary>
        /// <param name="temPt"></param>
        /// <returns></returns>
        public static void MMToFeet(ref CBorder border)
        {
            border.ScaleBy(1.0 / kFeetToMM);
        }

        /// <summary>
        /// 矢量：毫米转化为英寸
        /// </summary>
        /// <param name="temPt"></param>
        /// <returns></returns>
        public static CArea MMToFeet(CArea area)
        {
            area.ScaleBy(1.0 / kFeetToMM);
            return area;
        }

        /// <summary>
        /// 矢量：毫米转化为英寸
        /// </summary>
        /// <param name="temPt"></param>
        /// <returns></returns>
        public static CVector3d MMToFeet(CVector3d temPt)
        {
            return new CVector3d(MMToFeet(temPt.x), MMToFeet(temPt.y), MMToFeet(temPt.z));
        }

        /// <summary>
        /// 坐标点：英寸转化为毫米
        /// </summary>
        /// <param name="poly"></param>
        public static void MMToFeet(ref CPolyline poly)
        {
            CPoint3d[] pts = null;
            double[] arrBulge = null;
            bool bClose = true;
            poly.GetPoly(out pts, out arrBulge, out bClose);

            //
            for (int i = 0; i < pts.Length; i++)
            {
                MMToFeet(ref pts[i]);
            }

            //重置poly
            poly.SetPoly(pts, arrBulge, bClose);
        }

        /// <summary>
        /// 英寸转化为毫米
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double FeetToMM(double val)
        {
            return val * kFeetToMM;
        }

        /// <summary>
        /// 坐标点：英寸转化为毫米
        /// </summary>
        /// <param name="temPt"></param>
        /// <returns></returns>
        public static XYZ PointFeetToMM(XYZ temPt)
        {
            return new XYZ(FeetToMM(temPt.X), FeetToMM(temPt.Y), FeetToMM(temPt.Z));
        }

        public static double kFeetToMM = 304.8;

        /// <summary>
        /// 坐标点：英寸转化为毫米
        /// </summary>
        /// <param name="temPt"></param>
        /// <returns></returns>
        public static CPoint3d CPoint3dFeetToMM(CPoint3d pt)
        {
            return new CPoint3d(FeetToMM(pt.x), FeetToMM(pt.y), FeetToMM(pt.z));
        }

        /// <summary>
        /// Border：英寸转化为毫米
        /// </summary>
        /// <param name="border"></param>
        /// <returns></returns>
        public static CBorder CBorderFeetToMM(CBorder border)
        {
            CPoint3d spoint = CPoint3dFeetToMM(border.GetPointBegin());
            CPoint3d epoint = CPoint3dFeetToMM(border.GetPointEnd());
            return new CBorder(spoint, epoint, border.GetBulge());
        }

        /// <summary>
        /// 坐标点：英寸转化为毫米
        /// </summary>
        /// <param name="brd"></param>
        public static void FeetToMM(ref CPoint3d pt)
        {
            pt.x = FeetToMM(pt.x);
            pt.y = FeetToMM(pt.y);
            pt.z = FeetToMM(pt.z);
        }

        /// <summary>
        /// 坐标点：英寸转化为毫米
        /// </summary>
        /// <param name="temPt"></param>
        /// <returns></returns>
        public static CBorder FeetToMM(CBorder temPt)
        {
            return new CBorder(CPoint3dFeetToMM(temPt.GetPointBegin()), CPoint3dFeetToMM(temPt.GetPointEnd()), temPt.GetBulge());
        }

        /// <summary>
        /// 坐标点：英寸转化为毫米
        /// </summary>
        /// <param name="brd"></param>
        public static void FeetToMM(ref CBorder brd)
        {
            brd.ScaleBy(kFeetToMM);
        }

        /// <summary>
        /// 坐标点：英寸转化为毫米
        /// </summary>
        /// <param name="area"></param>
        public static void FeetToMM(ref CArea area)
        {
            area.ScaleBy(kFeetToMM);
        }

        /// <summary>
        /// 平方英尺转平方米
        /// </summary>
        /// <param name="squareFoot"></param>
        public static double SquareFootToSquareMeter(double squareFoot)
        {
            return squareFoot * 0.092903;
        }

        /// <summary>
        /// 立方英尺转立方米
        /// </summary>
        /// <param name="cubicFoot"></param>
        /// <returns></returns>
        public static double CubicFootToCubicMeter(double cubicFoot)
        {
            return cubicFoot * 0.0283168;
        }

        /// <summary>
        /// 坐标点：英寸转化为毫米
        /// </summary>
        /// <param name="poly"></param>
        public static void FeetToMM(ref CPolyline poly)
        {
            CPoint3d[] pts = null;
            double[] arrBulge = null;
            bool bClose = true;
            poly.GetPoly(out pts, out arrBulge, out bClose);

            //
            for (int i = 0; i < pts.Length; i++)
            {
                FeetToMM(ref pts[i]);
            }

            //重置poly
            poly.SetPoly(pts, arrBulge, bClose);
        }
        #endregion
    }
}