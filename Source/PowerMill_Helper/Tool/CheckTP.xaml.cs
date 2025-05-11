using PowerMILL;
using PowerMill_Helper.Class;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using File = System.IO.File;
using Path = System.IO.Path;
using Point = System.Windows.Point;

namespace PowerMill_Helper.Tool
{
    /// <summary>
    /// CheckTP.xaml 的交互逻辑
    /// </summary>
    public partial class CheckTP : UserControl
    {
        public CheckTP(MainCS mainCS_, PowerMILL.PluginServices PmServices_)
        {
            InitializeComponent();
            PmServices = PmServices_;
            MCS = mainCS_;
            this.DataContext = MCS;

            CheckExperMenu();
        }

        private MainCS MCS;

        #region PmCommand
        public string token;
        public PowerMILL.PluginServices PmServices;
        private void PMCom(string comd)
        {
            PmServices.InsertCommand(token, comd);
        }

        private string PMComEX(string comd)
        {
            object item;
            PmServices.InsertCommand(token, "ECHO OFF DCPDEBUG UNTRACE COMMAND ACCEPT");
            PmServices.DoCommandEx(token, comd, out item);
            return item.ToString().TrimEnd();
        }
        private string GetPMVal(string comd)
        {
            string Result = PmServices.GetParameterValueTerse(token, comd);
            if (Result.IndexOf("#错误:") == 0) return "";
            return PmServices.GetParameterValueTerse(token, comd);
        }
        #endregion

        #region ControlTitleEvent
        Point Grid_Move_Pos = new Point();
        private void Move_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label label = (Label)sender;
                Grid_Move_Pos = e.GetPosition(null);
                label.CaptureMouse();
                label.Cursor = Cursors.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_MouseDown\r" + ex.ToString());
            }


        }
        private void Move_Border_Mousemove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double dx = e.GetPosition(null).X - Grid_Move_Pos.X + this.Margin.Left;
                    double dy = e.GetPosition(null).Y - Grid_Move_Pos.Y + this.Margin.Top;
                    this.Margin = new Thickness(dx, dy, 0, 0);
                    Grid_Move_Pos = e.GetPosition(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_Mousemove\r" + ex.ToString());
            }

        }
        private void Move_Border_MoseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label label = (Label)sender;
                label.ReleaseMouseCapture();
                label.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_MoseUp\r" + ex.ToString());
            }
        }

        private void BorderCloserButton(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show("BorderCloserButton\r" + ex.ToString());
            }

        }


        #endregion

        #region 检查右键菜单
        private void CheckExperMenu()
        {
            string explorerFilePath = Path.Combine(MCS.popupsFolderPath, "explorer.ppm");
            string explorerFilePath_Copy = Path.Combine(MCS.popupsFolderPath, "explorer_copy.ppm");
            if (!File.Exists(explorerFilePath_Copy)) File.Copy(explorerFilePath, explorerFilePath_Copy, true);

            string xmlText = File.ReadAllText(explorerFilePath);

            // 要插入的新 button 标签
            string insertText = @"<button command='plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}VerifyToolpathS'
                                        label='检查程序碰撞过切'
                                        behaviour='shift_key_modify'
                                        multiple_selection='allowed'
                                    />";

            // 检查是否已存在该宏按钮（只要有这段路径就算存在）
            if (xmlText.Contains(@"command='plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}VerifyToolpathS'"))
            {
                //MessageBox.Show("按钮已存在，无需添加。");
                return;
            }
            // 提取 Toolpath 菜单区域
            string toolpathPattern = @"<!-- Toolpath Child Node Menu Start -->(.*?)<!-- Toolpath Child Node Menu End -->";
            Match toolpathMatch = Regex.Match(xmlText, toolpathPattern, RegexOptions.Singleline);
            if (!toolpathMatch.Success)
            {
                Console.WriteLine("未找到 Toolpath 区域。");
                return;
            }

            string menuBlock = toolpathMatch.Groups[1].Value;

            // 找到 <menupage label="Edit"...> 的位置
            string editTagPattern = @"<menupage\s+label\s*=\s*""Edit""[^>]*>";
            Match editMatch = Regex.Match(menuBlock, editTagPattern, RegexOptions.IgnoreCase);
            if (!editMatch.Success)
            {
                Console.WriteLine("未找到 Edit 菜单。");
                return;
            }

            int editStartIndex = editMatch.Index;
            int currentIndex = editStartIndex + editMatch.Length;

            // 用栈解析 Edit 的 menupage 结构，找到对应的结束标签位置
            int depth = 1;
            while (depth > 0 && currentIndex < menuBlock.Length)
            {
                // 从 currentIndex 开始往后找
                string remaining = menuBlock.Substring(currentIndex);

                Match nextOpen = Regex.Match(remaining, @"<menupage\b", RegexOptions.IgnoreCase);
                Match nextClose = Regex.Match(remaining, @"</menupage>", RegexOptions.IgnoreCase);

                if (!nextClose.Success)
                {
                    Console.WriteLine("未找到匹配的 </menupage>");
                    return;
                }

                // 根据相对位置判断嵌套结构
                if (nextOpen.Success && nextOpen.Index < nextClose.Index)
                {
                    depth++;
                    currentIndex += nextOpen.Index + nextOpen.Length;
                }
                else
                {
                    depth--;
                    currentIndex += nextClose.Index + nextClose.Length;
                }
            }

            // 插入新标签
            if (depth == 0)
            {
                int insertPosInBlock = currentIndex;
                string newMenuBlock = menuBlock.Insert(insertPosInBlock, insertText + "\n");

                // 替换整个 Toolpath 区域
                string newXml = xmlText.Replace(menuBlock, newMenuBlock);
                File.WriteAllText(explorerFilePath, newXml);
                Console.WriteLine("菜单插入完成！");
            }
            else
            {
                Console.WriteLine("解析标签失败，嵌套不完整。");
            }



            // MessageBox.Show("按钮插入完成。");
        }
        #endregion

        #region 获取已选刀路
        public void GetexplorerSelectTP()
        {
            try
            {
                string GetPmEntityCollectionPath = GetPMVal($"join(extract(explorer_selected_entities(),'name'),',')");
                MCS.CheckTPToolpathCollection.Clear();
                foreach (string item in GetPmEntityCollectionPath.Split(','))
                {
                    PMEntity pMEntity = new PMEntity()
                    {
                        Name = item,
                    };
                    MCS.CheckTPToolpathCollection.Add(pMEntity);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetexplorerSelectTP\r" + ex.ToString());
            }
        }
        #endregion


        #region 选择碰撞过切类型
        private void SelectChecktypeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton radioButton = (RadioButton)sender;
                MCS.CheckTP_Checktype = int.Parse(radioButton.Tag.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("CheckTP SelectChecktypeClick\r" + ex.ToString());
            }

        }
        #endregion

        #region 选择变换模型方式
        private void SelecttransformTypeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton radioButton = (RadioButton)sender;
                MCS.CheckTP_transform_Type = int.Parse(radioButton.Tag.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("CheckTP SelectChecktypeClick\r" + ex.ToString());
            }
        }
        #endregion

        #region 选择参考残留模型
        private void Selectentity_Click(object sender, RoutedEventArgs e)
        {
            entitySelect_Event?.Invoke(sender, e);
        }

        #endregion

        #region 实例选择路由
        public event RoutedEventHandler entitySelect_Event;

        #endregion

        #region 检查

        #endregion

        private void Check(object sender, RoutedEventArgs e)
        {
            // foreach (Model item in Models_) MessageBox.Show(item.Name);
            try
            {
                if (MCS.CheckTPToolpathCollection.Count == 0) return;
                if (MCS.CheckTP_refer_Selectindex == 1)
                {
                    if (MCS.CheckTP_refer_StockmodelName.Name == "")
                    {
                        MessageBox.Show("选择了参考【参考模型】检查但是还没有指定【参考模型】，检查结束"); return;
                    }
                }
                if (MCS.CheckTP_transform_Type == 2)
                {
                    if (MCS.CheckTP_transform_Workplane.Name == "")
                    {
                        MessageBox.Show("选择了多轴变换模型 但是还没有指定变换【基准】，检查结束"); return;
                    }
                }
                var models = PmServices.Project.Models;

                string MacroTempMacriFile = Path.Combine(MCS.CTempPath, "CheckTpTempMacro.mac");
                string MacroTempMSGFile = Path.Combine(MCS.CTempPath, "CheckTpTempMSG.mac");
                if (File.Exists(MacroTempMacriFile)) File.Delete(MacroTempMacriFile);
                if (File.Exists(MacroTempMSGFile)) File.Delete(MacroTempMSGFile);
                using (FileStream fs = new FileStream(MacroTempMacriFile, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        //sw.WriteLine("Print");

                        sw.WriteLine("PROJECT SAVE");
                        sw.WriteLine("ECHO OFF DCPDEBUG UNTRACE COMMAND ACCEPT");
                        sw.WriteLine("RESET LOCALVARS");
                        sw.WriteLine("DIALOGS MESSAGE OFF");
                        sw.WriteLine("DIALOGS ERROR OFF");
                        //sw.WriteLine("VIEW ANIMATION TIMELIMIT '0.2'");
                        sw.WriteLine("EDIT MODEL ALL DESELECT ALL");
                        sw.WriteLine($"string MSGFIlePath='{MacroTempMSGFile}'");
                        sw.WriteLine("string list Messageing = {}");
                        sw.WriteLine("string MessageTemp = ''");
                        sw.WriteLine("string CMDText = ''");

                        //在宏中指定变换类型
                        sw.WriteLine("EDIT PAR create string 'Plugin_checkTP_transform_TYPE'");
                        sw.WriteLine($"EDIT PAR 'project.Plugin_checkTP_transform_TYPE' '{MCS.CheckTP_transform_Type}'");
                        sw.WriteLine("EDIT PAR create string 'Plugin_HAStransform'");

                        //如果使用多轴对件检查先变换模型
                        sw.WriteLine("if ($project.Plugin_checkTP_transform_TYPE==2) {");
                        sw.WriteLine($"ACTIVATE WORKPLANE '{MCS.CheckTP_transform_Workplane.Name}'");
                        foreach (Model item in models)
                        {
                            sw.WriteLine($"TRANSFORM TYPE SCALEX TRANSFORM SCALEVALUE -1 TRANSFORM MODEL '{item.Name}'");
                        }
                        sw.WriteLine($"EDIT PAR 'project.Plugin_HAStransform' '1'");
                        sw.WriteLine("}");
                        foreach (PMEntity item in MCS.CheckTPToolpathCollection)
                        {
                            sw.WriteLine($"ACTIVATE Toolpath '{item.Name}'");
                            //如果使用三轴对件检查先变换模型
                            sw.WriteLine("if ($project.Plugin_checkTP_transform_TYPE==1) {");
                            sw.WriteLine($"ACTIVATE WORKPLANE '{MCS.CheckTP_transform_Workplane.Name}'");
                            foreach (Model model_ in models)
                            {
                                sw.WriteLine($"TRANSFORM TYPE SCALEX TRANSFORM SCALEVALUE -1 TRANSFORM MODEL '{model_.Name}'");
                            }
                            sw.WriteLine($"EDIT PAR 'project.Plugin_HAStransform' '1'");
                            sw.WriteLine("}");
                            //参数设置

                            sw.WriteLine($"print='{MCS.CheckTP_Checktype}'");
                            //碰撞检查设置
                            if (MCS.CheckTP_Checktype <= 1)
                            {
                                sw.WriteLine($"EDIT COLLISION TYPE COLLISION");//切换到碰撞检查模式


                                sw.WriteLine($"EDIT COLLISION HOLDER_CLEARANCE '{MCS.CheckTP_CkeckToolHolderGAP}'");//刀具夹持间隙
                                sw.WriteLine($"EDIT COLLISION SHANK_CLEARANCE '{MCS.CheckTP_CkeckToolGAP}'");//刀具夹持间隙
                                //计算碰撞深度
                                if (MCS.CheckTP_CheckSafeDepth)
                                {
                                    sw.WriteLine("EDIT COLLISION DEPTH Y");
                                }
                                else
                                {
                                    sw.WriteLine("EDIT COLLISION DEPTH N");
                                }
                                sw.WriteLine("EDIT COLLISION ADJUST_TOOL N");//调整刀具
                                CheckDataSet(sw);
                                //准备录制检查信息
                                sw.WriteLine("IF (file_exists($MSGFIlePath)) {");
                                sw.WriteLine("DELETE FILE $MSGFIlePath");
                                sw.WriteLine("}");
                                sw.WriteLine("TRACEFILE OPEN $MSGFIlePath");
                                //开始检查
                                sw.WriteLine($"EDIT COLLISION APPLY");
                                //录制检查信息结束
                                sw.WriteLine("TRACEFILE CLOSE");
                                SendCOLLISIONMSG(sw, "CheckTPMsg_COLLISION", item.Name);
                            }
                            //过切检查设置
                            if (MCS.CheckTP_Checktype >= 1)
                            {
                                sw.WriteLine($"EDIT COLLISION TYPE GOUGE");//切换到过切检查模式
                                CheckDataSet(sw);
                                //准备录制检查信息
                                sw.WriteLine("IF (file_exists($MSGFIlePath)) {");
                                sw.WriteLine("DELETE FILE $MSGFIlePath");
                                sw.WriteLine("}");
                                sw.WriteLine("TRACEFILE OPEN $MSGFIlePath");
                                //开始检查
                                sw.WriteLine($"EDIT COLLISION APPLY");
                                //录制检查信息结束
                                sw.WriteLine("TRACEFILE CLOSE");
                                SendCOLLISIONMSG(sw, "CheckTPMsg_GOUGE", item.Name);

                            }

                            //如果使用三轴对件检查结束变换回模型
                            sw.WriteLine("if ($project.Plugin_checkTP_transform_TYPE==1) {");
                            foreach (Model model_ in models)
                            {
                                sw.WriteLine($"TRANSFORM TYPE SCALEX TRANSFORM SCALEVALUE -1 TRANSFORM MODEL '{model_.Name}'");
                            }
                            sw.WriteLine($"EDIT PAR 'project.Plugin_HAStransform' '0'");
                            sw.WriteLine("}");

                        }
                        //如果使用多轴对件检查先变换模型
                        sw.WriteLine("if ($project.Plugin_checkTP_transform_TYPE==2) {");
                        sw.WriteLine($"ACTIVATE WORKPLANE '{MCS.CheckTP_transform_Workplane.Name}'");
                        foreach (Model item in models)
                        {
                            sw.WriteLine($"TRANSFORM TYPE SCALEX TRANSFORM SCALEVALUE -1 TRANSFORM MODEL '{item.Name}'");
                        }
                        sw.WriteLine($"EDIT PAR 'project.Plugin_HAStransform' '0'");
                        sw.WriteLine("}");
                        sw.WriteLine("DIALOGS MESSAGE ON");
                        sw.WriteLine("DIALOGS ERROR ON");
                    }
                }
                PMCom($"Macro '{MacroTempMacriFile}'");

            }
            catch (Exception ex)
            {
                MessageBox.Show("CheckTP 检查出现问题\r" + ex.ToString());
            }

        }

        private void CheckDataSet(StreamWriter sw)
        {
            //参考模型/残留模型
            if (MCS.CheckTP_refer_Selectindex == 0)
            {
                sw.WriteLine($"EDIT COLLISION STOCKMODEL_CHECK N");
            }
            else
            {
                sw.WriteLine($"EDIT COLLISION STOCKMODEL_CHECK N");
                sw.WriteLine($"EDIT COLLISION STOCKMODEL  \"{MCS.CheckTP_refer_StockmodelName.Name}\"");

            }

            sw.WriteLine("EDIT COLLISION SCOPE ALL");//参考所有
                                                     //分割刀路
            if (MCS.CheckTP_SplitTP)
            {
                sw.WriteLine($"EDIT COLLISION SPLIT_TOOLPATH Y");
                //输出安全刀路
                if (MCS.CheckTP_SplitTP_usesafe)
                {
                    sw.WriteLine($"EDIT COLLISION MISS_OUTPUT Y");
                }
                else
                {
                    sw.WriteLine($"EDIT COLLISION MISS_OUTPUT N");
                }
                //输出不安全刀路
                if (MCS.CheckTP_SplitTP_useunsafe)
                {
                    sw.WriteLine($"EDIT COLLISION HIT_OUTPUT Y");
                }
                else
                {
                    sw.WriteLine($"EDIT COLLISION HIT_OUTPUT N");
                }
            }
            else
            {
                sw.WriteLine($"EDIT COLLISION SPLIT_TOOLPATH N");
            }
            //显示不安全移动
            if (MCS.CheckTP_ShowUnSafe)
            {
                sw.WriteLine("DRAW COLLISION");
            }
            else
            {
                sw.WriteLine("UNDRAW COLLISION");
            }
        }

        private void SendCOLLISIONMSG(StreamWriter sw, string CheckTPMSGTYPE, string Tpname)
        {
            string ComHeader = "plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}";
            sw.WriteLine($"FILE OPEN $MSGFIlePath FOR READ AS 'file'");
            sw.WriteLine($"FILE READ $Messageing FROM 'file'");
            sw.WriteLine($"FILE CLOSE 'file'");
            sw.WriteLine($"$MessageTemp=''");
            sw.WriteLine("FOREACH line IN $Messageing {");
            sw.WriteLine("string word =substring($line,0,length($line)-1)");
            sw.WriteLine("$MessageTemp=$MessageTemp+$word");
            sw.WriteLine("}");
            sw.WriteLine($"$CMDText='{ComHeader}{CheckTPMSGTYPE}@split#'+$MessageTemp+'@split#{Tpname}'");
            sw.WriteLine($"DOCOMMAND $CMDText");
        }

        private void checkTPResult(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var listBox = sender as ListBox;
                var selectedItem = listBox?.SelectedItem as PMEntity;

                if (selectedItem == null)
                {

                    return;
                }

                MessageBox.Show($"刀具路径【{selectedItem.Name}】检查结果\r碰撞检查结果：\r{selectedItem.CheckTP1Msg}\r \r过切检查结果:\r{selectedItem.CheckTP2Msg}");

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询检查结果出现问题\rcheckTPResult\r" + ex.ToString());
            }
        }
    }
}
