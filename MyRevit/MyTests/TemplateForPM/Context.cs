﻿using Autodesk.Revit.DB;
using PmSoft.Common.RevitClass.VLUtils;

namespace PmSoft.MepProject.MepWork.FullFunctions.MEPCurveAutomaticTurn
{
    class MATContext
    {
        #region Creator
        private static MATCreator _Creator = null;
        public static MATCreator Creator
        {
            get
            {
                return _Creator ?? (_Creator = new MATCreator());
            }
        }
        #endregion

        #region Storage
        public static bool IsEditing;
        private static MATModelCollection Collection;
        private static MATStorageEntity CStorageEntity = new MATStorageEntity();
        /// <summary>
        /// 取数据Collection
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static MATModelCollection GetCollection(Document doc)
        {
            Collection = VLDelegateHelper.DelegateTryCatch(
                () =>
                {
                    string data = ExtensibleStorageHelper.GetData(doc, CStorageEntity, CStorageEntity.FieldOfData);
                    return new MATModelCollection(data);
                },
                () =>
                {
                    return new MATModelCollection("");
                }
            );
            return Collection;
        }

        /// <summary>
        /// 保存Collection
        /// </summary>
        /// <param name="doc"></param>
        public static bool SaveCollection(Document doc)
        {
            if (Collection == null)
                return false;
            var data = Collection.ToData();
            return VLDelegateHelper.DelegateTryCatch(
                () =>
                {
                    ExtensibleStorageHelper.SetData(doc, CStorageEntity, CStorageEntity.FieldOfData, data);
                    return true;
                },
                () =>
                {
                    ExtensibleStorageHelper.RemoveStorage(doc, CStorageEntity);
                    ExtensibleStorageHelper.SetData(doc, CStorageEntity, CStorageEntity.FieldOfData, data);
                    return false;
                }
            );
        }
        #endregion
    }
}
