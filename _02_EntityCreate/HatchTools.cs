using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;

namespace _02_EntityCreate
{
    public static class HatchTools
    {
        /// <summary>
        /// 填充图案名称
        /// </summary>
        public struct HatchPatternName
        {
            public static readonly string Solid = "SOLID";
            public static readonly string Angle = "ANGLE";
            public static readonly string Ansi31 = "ANSI31";
            public static readonly string Ansi32 = "ANSI32";
            public static readonly string Ansi33 = "ANSI33";
            public static readonly string Ansi34 = "ANSI34";
            public static readonly string Ansi35 = "ANSI35";
            public static readonly string Ansi36 = "ANSI36";
            public static readonly string Ansi37 = "ANSI37";
            public static readonly string Ansi38 = "ANSI38";
            public static readonly string Arb816 = "AR-B816";
            public static readonly string Arb816C = "AR-B816C";
            public static readonly string Arb88 = "AR-B88";
            public static readonly string Arbrelm = "AR-BRELM";
            public static readonly string Arbrstd = "AR-BRSTD";
            public static readonly string Arbconc = "AR-CONC";
        }

        /// <summary>
        /// 渐变填充名称
        /// </summary>
        public struct HatchGradientName
        {
            public static readonly string GrLinear = "Linear";
            public static readonly string GrCylinear = "Cylinear";
            public static readonly string GrInvcylinear = "Invcylinear";
            public static readonly string GrSpherical = "Spherical";
            public static readonly string GrHemisperical = "Hemisperical";
            public static readonly string GrCurved = "Curved";
            public static readonly string GrInvsperical = "Inveperical";
            public static readonly string GrInvhemisperical = "Invhemisperical";
            public static readonly string GrInvcurved = "Invcurved";

        }

        /// <summary>
        /// 图案填充 无颜色
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="patternName">图案名称</param>
        /// <param name="scale">填充比例</param>
        /// <param name="degree">旋转角度</param>
        /// <param name="entid">边界图形的ObjectId</param>
        /// <returns></returns>
        public static ObjectId HatchEntity(this Database db, string patternName, double scale, double degree, ObjectId entid)
        {
            ObjectId hatchId;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // 声明一个图案填充对象
                Hatch hatch = new Hatch
                {
                    PatternScale = scale // 设置填充比例
                };

                // 设置填充类型和图案名称
                hatch.SetHatchPattern(HatchPatternType.PreDefined, patternName);
                
                // 加入图形数据库
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                hatchId = btr.AppendEntity(hatch);
                trans.AddNewlyCreatedDBObject(hatch, true);

                // 设置填充角度
                hatch.PatternAngle = degree;
                // 设置关联
                hatch.Associative = true;
                // 设置边界图形和填充方式

                ObjectIdCollection obIds = new ObjectIdCollection {entid};
                hatch.AppendLoop(HatchLoopTypes.Outermost, obIds);
                // 计算填充并显示
                hatch.EvaluateHatch(true);
                // 提交事务
                trans.Commit();
            }
            return hatchId;
        }


        /// <summary>
        /// 图案填充 有填充颜色
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="patternName">图案名称</param>
        /// <param name="scale">填充比例</param>
        /// <param name="degree">旋转角度</param>
        /// <param name="bkColor">背景色</param>
        /// <param name="hatchColorIndex">填充图案的颜色</param>
        /// <param name="entId">边界图形的ObjectId</param>
        /// <returns></returns>

        public static ObjectId HatchEntity(this Database db, string patternName, double scale, double degree, Color bkColor, int hatchColorIndex, ObjectId entId)
        {
            ObjectId hatchId;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // 声明一个图案填充对象
                Hatch hatch = new Hatch
                {
                    PatternScale = scale,  // 设置填充比例
                    BackgroundColor = bkColor,  // 设置背景色
                    ColorIndex = hatchColorIndex // 设置填充图案颜色
                };
                // 设置填充类型和图案名称
                hatch.SetHatchPattern(HatchPatternType.PreDefined, patternName);
                // 加入图形数据库
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                hatchId = btr.AppendEntity(hatch);
                trans.AddNewlyCreatedDBObject(hatch, true);

                // 设置填充角度
                hatch.PatternAngle = degree;
                // 设置关联
                hatch.Associative = true;
                // 设置边界图形和填充方式
                ObjectIdCollection obIds = new ObjectIdCollection {entId};
                hatch.AppendLoop(HatchLoopTypes.Outermost, obIds);
                // 计算填充并显示
                hatch.EvaluateHatch(true);
                // 提交事务
                trans.Commit();
            }
            return hatchId;
        }


        /// <summary>
        /// 图案填充
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="loopTypes"></param>
        /// <param name="patternName">图案名称</param>
        /// <param name="scale">填充比例</param>
        /// <param name="degree">旋转角度</param>
        /// <param name="entid">边界图形的ObjectId</param>
        /// <returns></returns>
        public static ObjectId HatchEntity(this Database db, List<HatchLoopTypes> loopTypes, string patternName, double scale, double degree, params ObjectId[] entid) // 一个方法只能传递一个可变参数 且需要放在最后
        {
            ObjectId hatchId;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // 声明一个图案填充对象
                Hatch hatch = new Hatch
                {
                    PatternScale = scale // 设置填充比例
                };

                // 设置填充类型和图案名称
                hatch.SetHatchPattern(HatchPatternType.PreDefined, patternName);
                // 加入图形数据库
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                hatchId = btr.AppendEntity(hatch);
                trans.AddNewlyCreatedDBObject(hatch, true);

                // 设置填充角度
                hatch.PatternAngle = degree;
                // 设置关联
                hatch.Associative = true;
                // 设置边界图形和填充方式


                ObjectIdCollection obIds = new ObjectIdCollection();
                // 依次添加图形填充样式
                for (int i = 0; i < entid.Length; i++)
                {
                    obIds.Clear();
                    obIds.Add(entid[i]);
                    hatch.AppendLoop(loopTypes[i], obIds);
                }


                // 计算填充并显示
                hatch.EvaluateHatch(true);
                // 提交事务
                trans.Commit();
            }
            return hatchId;
        }


        /// <summary>
        /// 渐变填充
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="colorIndex1">颜色索引1</param>
        /// <param name="colorIndex2">颜色索引2</param>
        /// <param name="hatchGradientName">渐变图案</param>
        /// <param name="entId">边界图形的ObjectId</param>
        /// <returns>ObjectId</returns>
        public static ObjectId HatchGradient(this Database db, short colorIndex1, short colorIndex2, string hatchGradientName, ObjectId entId)
        {
            // 声明ObjectId, 用于返回
            ObjectId hatchId;
            ObjectIdCollection objIds = new ObjectIdCollection {entId};
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // 声明填充对象
                Hatch hatch = new Hatch
                {
                    HatchObjectType = HatchObjectType.GradientObject // 设置填充类型为渐变类型填充
                };
                
                // 设置渐变填充的类型和渐变填充的图案名称
                hatch.SetGradient(GradientPatternType.PreDefinedGradient, hatchGradientName);
                // 设置填充颜色
                Color color1 = Color.FromColorIndex(ColorMethod.ByColor, colorIndex1);
                Color color2 = Color.FromColorIndex(ColorMethod.ByColor, colorIndex2);
                GradientColor gColor1 = new GradientColor(color1, 0);
                GradientColor gColor2 = new GradientColor(color2, 1);
                hatch.SetGradientColors(new[] { gColor1, gColor2 });
                
                // 将填充对象加入图形数据库
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                hatchId = btr.AppendEntity(hatch);
                trans.AddNewlyCreatedDBObject(hatch, true);
                // 添加关联
                hatch.Associative = true;
                hatch.AppendLoop(HatchLoopTypes.Outermost, objIds);
                // 计算并显示填充
                hatch.EvaluateHatch(true);
                // 提交事务处理
                trans.Commit();
            }
            return hatchId;
        }
    }
}
