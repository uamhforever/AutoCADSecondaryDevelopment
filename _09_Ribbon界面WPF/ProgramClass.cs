using System.IO;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;

namespace _09_Ribbon界面WPF
{
    public class ProgramClass
    {
        [CommandMethod("RibbonDemo")]
        public void RibbonDemo()
        {
            RibbonControl ribbonCtrl = ComponentManager.Ribbon; //获取cad的Ribbon界面
            RibbonTab tab = ribbonCtrl.AddTab("我的测试选项卡", "Acad.RibbonId1", true); //给Ribbon界面添加一个选项卡
            CurPaht.curPaht = Path.GetDirectoryName(this.GetType().Assembly.Location) + "\\"; //获取程序集的加载路径
            RibbonPanelSource panelSource = tab.AddPanel("绘图"); //给选项卡添加面板
            panelSource.Items.Add(RibbonButtonInfos.LineBtn); //添加直线命令按钮
            panelSource.Items.Add(RibbonButtonInfos.PolylineBtn); //添加多段线命令按钮

        }
    }
}
