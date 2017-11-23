﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MyRevit.MyTests.CompoundStructureAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRevit.MyTests.PAA
{
    /// <summary>
    /// 编辑Updater
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class PAAUpdaterCommand : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            var uiApp = application;
            var app = uiApp.ControlledApplication;
            var editUpdater = new PAAUpdater_Edit(new AddInId(new Guid("B593F2C4-F38C-41D7-AE2C-369BB0149D90")));
            var updaterInfo = UpdaterRegistry.GetRegisteredUpdaterInfos().FirstOrDefault(c => c.UpdaterName == editUpdater.GetUpdaterName());
            if (updaterInfo == null)
                UpdaterRegistry.UnregisterUpdater(editUpdater.GetUpdaterId());
            UpdaterRegistry.RegisterUpdater(editUpdater, true);
            UpdaterRegistry.AddTrigger(editUpdater.GetUpdaterId(), new LogicalOrFilter(new List<ElementFilter>() {
                    new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves),//管道
                    new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves),//风管
                    new ElementCategoryFilter(BuiltInCategory.OST_CableTray),//桥架
                    //TODO_PAA 确定线组的检测
                    new ElementCategoryFilter(BuiltInCategory.OST_IOSGroups),//线组
                    //new ElementCategoryFilter(BuiltInCategory.OST_IOSModelGroups),//线组
                    new ElementCategoryFilter(BuiltInCategory.OST_PipeTags),//标注
                    new ElementCategoryFilter(BuiltInCategory.OST_DuctTags),//标注
                    new ElementCategoryFilter(BuiltInCategory.OST_CableTrayTags),//标注
                })
            , Element.GetChangeTypeAny());
            //var deleteUpdater = new CompoundStructureAnnotationUpdater_Delete(new AddInId(new Guid("B593F2C4-F38C-41D7-AE2C-369BB0149D90")));
            //updaterInfo = UpdaterRegistry.GetRegisteredUpdaterInfos().FirstOrDefault(c => c.UpdaterName == deleteUpdater.GetUpdaterName());
            //if (updaterInfo == null)
            //    UpdaterRegistry.UnregisterUpdater(deleteUpdater.GetUpdaterId());
            //UpdaterRegistry.RegisterUpdater(deleteUpdater, true);
            //UpdaterRegistry.AddTrigger(deleteUpdater.GetUpdaterId(), new LogicalOrFilter(new List<ElementFilter>() {
            //        new ElementCategoryFilter(BuiltInCategory.OST_Walls),//墙
            //        new ElementCategoryFilter(BuiltInCategory.OST_Floors),//板
            //        new ElementCategoryFilter(BuiltInCategory.OST_Roofs),//屋顶
            //        new ElementCategoryFilter(BuiltInCategory.OST_DetailComponents),//线族
            //        new ElementCategoryFilter(BuiltInCategory.OST_TextNotes),//文本
            //    })
            //, Element.GetChangeTypeElementDeletion());
            return Result.Succeeded;
        }
    }
}
