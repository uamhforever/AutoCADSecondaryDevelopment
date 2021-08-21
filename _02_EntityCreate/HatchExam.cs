using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;

namespace _02_EntityCreate
{

    public class HatchExam
    {
        [CommandMethod("HatchDemo")]
        public void HatchDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;  // 创建图形数据库对象

            ObjectId cId = db.AddCircleToModeSpace(new Point3d(100, 100, 0), 100); // 添加一个圆心为（100,100,0） 半径为00的圆到数据库中
            #region
            //ObjectIdCollection obIds = new ObjectIdCollection();
            //obIds.Add(cId);

            //// 启用事务处理
            //using (Transaction trans = db.TransactionManager.StartTransaction())
            //{
            //    // 声明一个图案填充对象
            //    Hatch hatch = new Hatch();
            //    // 设置填充比例
            //    hatch.PatternScale = 5;
            //    // 设置填充类型和图案名称
            //    hatch.SetHatchPattern(HatchPatternType.PreDefined, "ANGLE");
            //    // 加入图形数据库
            //    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
            //    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            //    btr.AppendEntity(hatch);
            //    trans.AddNewlyCreatedDBObject(hatch, true);

            //    // 设置填充角度
            //    hatch.PatternAngle = Math.PI / 4;
            //    // 设置关联
            //    hatch.Associative = true;
            //    // 设置边界图形和填充方式
            //    hatch.AppendLoop(HatchLoopTypes.Outermost, obIds);
            //    // 计算填充并显示
            //    hatch.EvaluateHatch(true);
            //    // 提交事务
            //    trans.Commit();
            //}
            #endregion


            db.HatchEntity(HatchTools.HatchPatternName.Arb88, 4, 45, cId); // scale = 4  degree = 45°

            ObjectId cId1 = db.AddCircleToModeSpace(new Point3d(300, 100, 0), 100);
            db.HatchEntity(HatchTools.HatchPatternName.Arbrstd, 2, 0, Color.FromColorIndex(ColorMethod.ByColor, 2), 1, cId1);

            ObjectId c1 = db.AddCircleToModeSpace(new Point3d(500, 500, 0), 200);
            ObjectId c2 = db.AddCircleToModeSpace(new Point3d(500, 500, 0), 100);

            List<HatchLoopTypes> loopTypes = new List<HatchLoopTypes>();
            loopTypes.Add(HatchLoopTypes.Outermost); // 外部边界
            loopTypes.Add(HatchLoopTypes.Outermost); // 边界
            db.HatchEntity(loopTypes, HatchTools.HatchPatternName.Arbrelm, 0.2, 0, c1, c2); // 两个边界的填充

            c1 = db.AddCircleToModeSpace(new Point3d(1000, 500, 0), 200);
            c2 = db.AddCircleToModeSpace(new Point3d(1000, 500, 0), 100);
            ObjectId c3 = db.AddCircleToModeSpace(new Point3d(1000, 500, 0), 50);
            loopTypes.Add(HatchLoopTypes.Outermost);
            db.HatchEntity(loopTypes, HatchTools.HatchPatternName.Arbconc, 0.2, 0, c1, c2, c3);  // 三个边界填充


            c1 = db.AddCircleToModeSpace(new Point3d(1000, 500, 0), 200);
            c2 = db.AddCircleToModeSpace(new Point3d(1000, 500, 0), 100);
            c3 = db.AddCircleToModeSpace(new Point3d(1000, 500, 0), 50);

            db.HatchEntity(loopTypes, HatchTools.HatchPatternName.Arbrelm, 0.2, 0, c1); // 忽略中间边界进行填充


            // 渐变填充

            // Database db = HostApplicationServices.WorkingDatabase;
            short colorIndex1 = 1;
            short colorIndex2 = 2;
            ObjectIdCollection objIds = new ObjectIdCollection
            {
                db.AddCircleToModeSpace(new Point3d(100, 100, 0), 100)
            };
            string hatchGradientName = HatchTools.HatchGradientName.GrInvcurved;
            ObjectId objectId = db.AddRectToModeSpace(new Point2d(100, 100), new Point2d(500, 300));
            db.HatchGradient(colorIndex1, colorIndex2, hatchGradientName, objectId);

        }


    }
}
