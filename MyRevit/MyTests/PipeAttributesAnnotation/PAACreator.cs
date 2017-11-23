﻿using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace MyRevit.MyTests.PAA
{
    public class PAACreator
    {
        public void Generate(Document doc, PAAModelForSingle model, Element element)
        {
            Generate(doc, model, element, null);
        }
        public void Generate(Document doc, PAAModelForSingle model, Element element, XYZ offset)
        {
            //主体
            model.TargetId = element.Id;
            //线生成
            List<Line> lines = new List<Line>();
            model.CalculateLocations(element, offset);//计算内容定位
            lines.Add(Line.CreateBound(model.BodyStartPoint, model.BodyEndPoint));//竖干线
            lines.Add(Line.CreateBound(model.BodyEndPoint, model.LeafEndPoint));//斜支线
            model.LineIds = new List<ElementId>();
            foreach (var line in lines)
            {
                var lineElement = doc.Create.NewDetailCurve(doc.ActiveView,line);
                model.LineIds.Add(lineElement.Id);
            }
            var group = doc.Create.NewGroup(model.LineIds);
            model.GroupId = group.Id;
            //文本生成
            var tagSymbol = model.AnnotationType.GetTagFamily(doc);//获取线标注类型 
            IndependentTag subTag = doc.Create.NewTag(doc.ActiveView, doc.GetElement(model.TargetId), false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, model.TextLocation);
            subTag.TagHeadPosition = model.TextLocation;
            subTag.ChangeTypeId(tagSymbol.Id);
        }
        public void Regenerate(Document doc, PAAModelForSingle model, Element target)
        {
            //FamilyInstance line = doc.GetElement(model.LineId) as FamilyInstance;
            //PAAContext.Creator.Regenerate(doc, model, target, (line.Location as LocationPoint).Point - model.LineLocation);
        }
        public void Regenerate(Document doc, PAAModelForSingle model, Element target, XYZ offset)
        {
            //TODO_PAA
            //model.BodyStartPoint =;
            //model.BodyEndPoint =;
            //model.LeafEndPoint=;
            //Clear(doc, model);
            //Generate(doc, model, target, offset);
        }
        public void Clear(Document doc, PAAModelForSingle model)
        {
            ////删除线
            //if (model.LineId != null && doc.GetElement(model.LineId) != null)
            //    doc.Delete(model.LineId);
            ////删除标注
            //foreach (var item in model.TextNoteIds)
            //    if (doc.GetElement(item) != null)
            //        doc.Delete(item);
        }
    }
}
