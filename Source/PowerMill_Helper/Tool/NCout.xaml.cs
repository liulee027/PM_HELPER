using Newtonsoft.Json;
using PowerMill_Helper.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PowerMill_Helper.Tool
{
    /// <summary>
    /// NCout.xaml 的交互逻辑
    /// </summary>
    public partial class NCout : UserControl
    {
        public NCout(MainCS mainCS_, PowerMILL.PluginServices PmServices_)
        {
            InitializeComponent();
            PmServices = PmServices_;
            MCS = mainCS_;
            this.DataContext = MCS;
            Ncout_ReadSetting();
            Ncout_ResReadOptFile();
        }

        private MainCS MCS;


        private void Ncout_ReadSetting()
        {
            if (File.Exists(MCS.ConfigInitPath))
            {
                MCS.Ncout_sheetTemplatePath_ = ConfigINI.ReadSetting(MCS.ConfigInitPath, "Ncout", "sheetTemplatePath", Path.Combine(MCS.PluginFolder, "File", "Ncout", "NCSheet.html"));
                MCS.Onchange(nameof(MCS.Ncout_sheetTemplatePath));
                MCS.ncoutOutputFolderPath_ = ConfigINI.ReadSetting(MCS.ConfigInitPath, "Ncout", "OutputFolderPath", "");
                MCS.Onchange(nameof(MCS.Ncout_OutputFolderPath));
                MCS.Ncout_AllTpOutput_ = ConfigINI.ReadSetting(MCS.ConfigInitPath, "Ncout", "AllTpOutput", "True") == "True" ? true : false;
                MCS.Onchange(nameof(MCS.Ncout_AllTpOutput));
                MCS.Ncout_SingleTpOutput_ = ConfigINI.ReadSetting(MCS.ConfigInitPath, "Ncout", "SingleTpOutput", "True") == "True" ? true : false;
                MCS.Onchange(nameof(MCS.Ncout_SingleTpOutput));

            }

        }

        public void Show()
        {
            this.Visibility=Visibility.Visible;
            Ncout_ResToolpathCollection();
        }
        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }

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

        #region 读取OPT文件配置
        private void Ncout_ResReadOptFile()
        {
            try
            {
                // 从配置文件中读取 JSON 字符串
                string json_ = ConfigINI.ReadSetting(MCS.ConfigInitPath, "NCout", "NCOPTFILELIST", "");
                if (json_.Trim() == "") return;

                MCS.NcOpts = JsonConvert.DeserializeObject<ObservableCollection<NcoutOpt>>(json_);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ncout_ResReadOptFile\r配置文件不可读\r" + ex.ToString());
                //MCS.LogVoid(ex.ToString());
            }
        }
        #endregion

        #region 重置数据
        private void ThisAppRes(object sender, RoutedEventArgs e)
        {
            Ncout_ResToolpathCollection();
        }
        #endregion

        #region 选择输出基准
        public event RoutedEventHandler Ncout_UserselectNcoutWorkplant;
        private void Ncout_UserselectNcoutWorkplantButton(object sender, RoutedEventArgs e)
        {
            Ncout_UserselectNcoutWorkplant?.Invoke(sender, e);
        }
        #endregion

        #region 读取刀具路径列表
        private void Ncout_ResToolpathCollection()
        {
            try
            {
                List<string> FolderList_ = new List<string>();
                MCS.NcOutToolpathCollection.Clear();
                string GetPmEntityCollectionPath = GetPMVal($"join(apply(extract(folder('Toolpath'),'name'),\"pathname('Toolpath',this)\"),',')");
                if (GetPmEntityCollectionPath.Trim() != "")
                {
                    foreach (string item in GetPmEntityCollectionPath.Split(','))
                    {
                        string rootpath_ = item.Substring(0, item.LastIndexOf('\\'));
                        string[] rootpathSplit_ = item.Split('\\');

                        if (!FolderList_.Contains(rootpath_))
                        {
                            if (rootpath_ != "Toolpath")
                            {
                                PMEntity pMEntity = new PMEntity()
                                {
                                    Name = rootpathSplit_[rootpathSplit_.Length - 2],
                                    RootPath = item,
                                    isRootTree = true,
                                    uiwidth = 10 * (rootpathSplit_.Length - 1)
                                };
                                MCS.NcOutToolpathCollection.Add(pMEntity);
                            }

                            FolderList_.Add(rootpath_);
                        }
                        string name_ = rootpathSplit_[rootpathSplit_.Length - 1];
                        try
                        {
                            PMEntity pMEntity_ = MCS.PMEmtitys["Toolpath"].FirstOrDefault(e => e.Name == name_);
                            pMEntity_.isRootTree = false;
                            pMEntity_.RootPath = item;
                            pMEntity_.uiwidth = 10 * (rootpathSplit_.Length);
                            MCS.NcOutToolpathCollection.Add(pMEntity_);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(name_);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ncout_ResToolpathCollection\r" + ex.ToString());
            }
        }
        #endregion

        #region 清空待输出列表
        private void ClearRight(object sender, RoutedEventArgs e)
        {
            try
            {
                MCS.Ncout_ReadlyList.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_ClearRight\r" + ex.ToString());
            }
        }

        #endregion

        #region 刀具路径添加入待输出列表
        private void ChooseTpToRight(object sender, RoutedEventArgs e)
        {

            try
            {
                foreach (PMEntity item in LeftList.SelectedItems)
                {
                    if (!MCS.NcOpts.Contains(MCS.NcoutOptSelectOpt))
                    {
                        MessageBox.Show("还没有选后处理");
                        return;
                    }
                    if (item.isRootTree) continue;
                    if (item.Type == "method") continue;
                    if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk) { item.NcoutMachineWorkplane = MCS.NcoutOptSelectOpt.OptSelectMachineWkNameLIst[0]; }
                    ;
                    try
                    {
                        item.NcoutToolNumber = int.Parse(GetPMVal($"entity('toolpath','{item.Name}').tool.number"));
                    }
                    catch (Exception)
                    {
                        item.NcoutToolNumber = 1;
                    }
                    PMEntity pMEntity = item.DeepCopy();
                    MCS.Ncout_ReadlyList.Add(pMEntity);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__OnSelectTpToRight_Event\r" + ex.ToString());
            }

        }
        #endregion

        #region 更改加工坐标系
        private void ShowChangeTpMachineWorkplaneCombobx(object sender, RoutedEventArgs e)
        {
            if (RightTpListbox.SelectedItems.Count > 0)
            {
                Button button = (Button)sender;
                button.Visibility = Visibility.Hidden;
                Grid grid = (Grid)button.Parent;
                ComboBox comboBox = (ComboBox)grid.Children[0];
                comboBox.IsDropDownOpen = true;

            }
        }
        private void ShowChangeTpMachineWorkplaneSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBox comboBox = (ComboBox)sender;
                if (RightTpListbox.SelectedItems.Count > 0)
                {
                    foreach (PMEntity item in RightTpListbox.SelectedItems)
                    {
                        item.NcoutMachineWorkplane = comboBox.SelectedItem.ToString();
                    }
                }
                Grid grid = (Grid)comboBox.Parent;
                Button button = (Button)grid.Children[1];
                button.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__ChangeTlNumber_Event\r" + ex.ToString());
            }
        }

        #endregion

        #region 更改加工刀具刀号
        private void ShowChangeTlNumberinput(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RightTpListbox.SelectedItems.Count > 0)
                {
                    Button button = (Button)sender;
                    button.Visibility = Visibility.Hidden;
                    Grid grid = (Grid)button.Parent;
                    TextBox textBox = (TextBox)grid.Children[0];
                    textBox.Text = "";
                    textBox.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_ShowChangeTlNumberinput\r" + ex.ToString());
            }
        }
        private void ChangeTlNumberinputEnter(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    ChangeTlNumberinputEnterVoid(sender, null);

                }
                if (e.Key == Key.Escape)
                {
                    TextBox textBox = (TextBox)sender;
                    Grid grid = (Grid)textBox.Parent;
                    Button button = (Button)grid.Children[1];
                    button.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ChangeTlNumberinputEnter\r" + ex.ToString());
            }

        }
        private void ChangeTlNumberinputEnterVoid(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                if (RightTpListbox.SelectedItems.Count > 0)
                {

                    if (int.TryParse(textBox.Text, out int number))
                    {
                        NCout__ChangeTlNumber_Event(number, RightTpListbox.SelectedItems);
                        Grid grid = (Grid)textBox.Parent;
                        Button button = (Button)grid.Children[1];
                        button.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (textBox.Text.Trim() == "")
                        {
                            Grid grid = (Grid)textBox.Parent;
                            Button button = (Button)grid.Children[1];
                            button.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            MessageBox.Show("输入数据不为整数");
                        }


                    }
                }
                else
                {
                    Grid grid = (Grid)textBox.Parent;
                    Button button = (Button)grid.Children[1];
                    button.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ChangeTlNumberinputEnterVoid\r" + ex.ToString());
            }

        }
        private void NCout__ChangeTlNumber_Event(int number, IList selectedItems)
        {
            try
            {

                foreach (PMEntity item in selectedItems)
                {
                    item.NcoutToolNumber = number;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__ChangeTlNumber_Event\r" + ex.ToString());
            }
        }
        #endregion

        #region 创建Nc程序

        private void CreateToNcToolpath(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MCS.NcOpts.Contains(MCS.NcoutOptSelectOpt))
                {
                    MessageBox.Show("还没有选后处理");
                    return;
                }

                PMCom("DELETE NCPROGRAM ALL");
                string ForTpname_ = $"NC{MCS.Ncout_ReadlyList[0].Name}-{MCS.Ncout_ReadlyList[MCS.Ncout_ReadlyList.Count - 1].Name}";
                if (MCS.Ncout_AllTpOutput == true)
                {
                    PMCom($"CREATE NCPROGRAM '{ForTpname_}'");
                    if (MCS.NcoutOptSelectOpt.OptCanSelectWkOutput)
                    {
                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' SET WORKPLANE '{MCS.Ncout_Selected_OutputWorkplane.Name}'");//输出基准
                    }
                    else
                    {
                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' SET WORKPLANE ' '");//输出基准
                    }
                    PMCom($"EDIT NCPROGRAM '{ForTpname_}' TOOLCHANGE ALWAYS");//设置Nc程序输出总是换刀
                    PMCom($"EDIT NCPROGRAM '{ForTpname_}' CHANGE BEFORE");//连接前换刀
                    if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                    {
                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' FIXTUREOFFSET RESETALL");
                        foreach (string item in MCS.NcoutOptSelectOpt.OptSelectMachineWkNameLIst)
                        {
                            PMCom($"EDIT NCPROGRAM '{ForTpname_}' FIXTUREOFFSET ADD '{item}'");
                        }

                    }
                }



                for (int i = 0; i < MCS.Ncout_ReadlyList.Count; i++)
                {
                    string TpName_ = MCS.Ncout_ReadlyList[i].Name;
                    int Tpnumber = MCS.Ncout_ReadlyList[i].NcoutToolNumber;
                    string TpMachineworkplane = MCS.Ncout_ReadlyList[i].NcoutMachineWorkplane;
                    string TpOutWkName = "";
                    if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                    {
                        TpOutWkName = MCS.Ncout_Selected_OutputWorkplane.Name;
                    }
                    else
                    {
                        TpOutWkName = GetPMVal($"entity('toolpath','{TpName_}').workplane");
                    }

                    if (MCS.Ncout_SingleTpOutput == true)
                    {
                        PMCom($"CREATE NCPROGRAM '{TpName_}' EDIT NCPROGRAM '{TpName_}' APPEND TOOLPATH '{TpName_}' DEACTIVATE NCPROGRAM");
                        PMCom($"EDIT NCPROGRAM '{TpName_}' ITEM 0 COMPONENT 0 TOOLNUMBER '{Tpnumber}'");
                        //输出坐标系
                        if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                        {
                            PMCom($"EDIT NCPROGRAM '{TpName_}' FIXTUREOFFSET NAME '{TpMachineworkplane}'");
                            PMCom($"EDIT NCPROGRAM '{TpName_}' FIXTUREOFFSET RESETALL");
                            PMCom($"EDIT NCPROGRAM '{TpName_}' FIXTUREOFFSET ADD '{TpMachineworkplane}'");
                            PMCom($"EDIT NCPROGRAM '{TpName_}' ITEM 0 COMPONENT 0 FIXTUREOFFSET '{TpMachineworkplane}'");
                        }
                        //输出基准
                        if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                        {
                            PMCom($"EDIT NCPROGRAM '{TpName_}' SET WORKPLANE '{MCS.Ncout_Selected_OutputWorkplane.Name}'");
                        }
                        else
                        {
                            PMCom($"EDIT NCPROGRAM '{TpName_}' SET WORKPLANE ' '");
                        }
                    }
                    if (MCS.Ncout_AllTpOutput == true)
                    {
                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' INSERT Toolpath '{TpName_}' LAST");
                        //输出坐标系
                        if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                        {
                            PMCom($"EDIT NCPROGRAM '{ForTpname_}' ITEM {i} COMPONENT {i}000 FIXTUREOFFSET '{TpMachineworkplane}'");
                        }

                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' ITEM {i} COMPONENT {i}000 TOOLNUMBER '{Tpnumber}'");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_CreateToNcToolpath\r" + ex.ToString());
            }

        }
        #endregion

        #region 输出程序单

        private void CreateSheet(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MCS.PMNCprogramList.Count == 0)
                {
                    MessageBox.Show("没有NC程序");
                    return;
                }
                if (MCS.Ncout_sheetTemplatePath == "")
                {
                    MessageBox.Show("还没有设置程序单模板"); return;
                }
                DateTime dateTime = DateTime.Now;
                string noewtime_ = dateTime.ToString("yyyyMMddHHmmss");
                string sheetPath = MCS.ProjectPath + "\\程序单\\" + "程序单_" + noewtime_ + ".html";
                string Imageph = MCS.ProjectPath + "\\程序单\\Image_" + noewtime_ + ".PNG";
                Directory.CreateDirectory(MCS.ProjectPath + "\\程序单");
                File.Copy(MCS.Ncout_sheetTemplatePath, sheetPath, true);

                StreamReader streamReader = new StreamReader(sheetPath, Encoding.Default);
                string Htmltxt = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
                double CutAllTime = 0;
                string Html_AddtXT = "";

                for (int i = 0; i < MCS.Ncout_ReadlyList.Count; i++)
                {
                    string TpName_ = MCS.Ncout_ReadlyList[i].Name;
                    int Tpnumber = MCS.Ncout_ReadlyList[i].NcoutToolNumber;
                    string TpTlName = GetPMVal($"entity('toolpath','{TpName_}').tool.name");
                    string TpMachineworkplane = MCS.Ncout_ReadlyList[i].NcoutMachineWorkplane;
                    string TpOutWkName = "";
                    if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                    {
                        TpOutWkName = MCS.Ncout_Selected_OutputWorkplane.Name;
                    }
                    else
                    {
                        TpOutWkName = GetPMVal($"entity('toolpath','{TpName_}').workplane.name");
                    }
                    string tlDim = GetPMVal($"entity('tool','{TpTlName}').diameter");
                    string tlRadius = GetPMVal($"entity('tool','{TpTlName}').TipRadius");
                    string tlLength = GetPMVal($"entity('tool','{TpTlName}').length");
                    string tloverhang = GetPMVal($"entity('tool','{TpTlName}').overhang");
                    string tlholedim = GetPMVal($"entity('tool','{TpTlName}').holdersetvalues[0].lowerdiameter");
                    string tpthince = GetPMVal($"entity('toolpath','{TpName_}').thickness");
                    string tpuseaxiathickness = GetPMVal($"entity('toolpath','{TpName_}').useaxialthickness");
                    string tpaxisthinc = "";
                    if (tpuseaxiathickness == "1")
                    {
                        tpaxisthinc = "/" + GetPMVal($"entity('toolpath','{TpName_}').axialthickness");
                    }
                    string tpcheck1 = GetPMVal($"entity('toolpath','{TpName_}').verification.collisionchecked");
                    string tpcheck2 = GetPMVal($"entity('toolpath','{TpName_}').verification.gougechecked");
                    string Tpchecked = "无";
                    if (tpcheck1 == "1" && tpcheck2 == "1")
                    {
                        Tpchecked = "是";
                    }
                    string tptotaltime = GetPMVal($"entity('ncprogram','{TpName_}').statistics.totaltime");
                    CutAllTime += double.Parse(tptotaltime);
                    string tptotaltimeM = GetPMVal($"time_to_string({tptotaltime},'m')");
                    string tpZmin = GetPMVal($"round(toolpath_cut_limits('{TpName_}')[4],3)");
                    string tpstatgy = GetPMVal($"translate(entity('toolpath','{TpName_}').strategy)");
                    if (tpstatgy.IndexOf("偏移区域清除") >= 0)
                    {
                        if (GetPMVal($"entity('toolpath','{TpName_}').Areaclearance.rest.active") == "1")
                        {
                            tpstatgy = "残留加工";
                        }
                        else
                        {
                            tpstatgy = "区域清除";
                        }
                    }
                    if (tpstatgy.IndexOf("钻孔") >= 0)
                    {
                        tpstatgy = GetPMVal($"translate(entity('toolpath','{TpName_}').drill.type)");
                    }
                    string theToolPhAxis = GetPMVal($"entity('toolpath','{TpName_}').ToolAxis.Type");
                    if (theToolPhAxis != "vertical")
                    {
                        theToolPhAxis = "_/";
                    }
                    else
                    {
                        theToolPhAxis = "_|";
                    }
                    //tool cut value
                    double tapdepth = 0;
                    double rpmvalue_ = 0;
                    double cutspeed_ = 0;
                    try
                    {
                        if (GetPMVal($"entity('tool','{TpTlName}').type") == "tap")
                        {
                            tapdepth = double.Parse(GetPMVal($"round(entity('tool','{TpTlName}').FeedPerTooth.finishing.drill*entity('tool','{TpTlName}').diameter,4)"));
                            rpmvalue_ = double.Parse(GetPMVal($"round((entity('tool','{TpTlName}').cuttingspeed.finishing.drill*1000)/(pi*entity('tool','{TpTlName}').diameter),1)"));
                        }
                        else
                        {
                            tapdepth = double.Parse(GetPMVal($"round(entity('tool','{TpTlName}').FeedPerTooth.finishing.general*entity('tool','{TpTlName}').diameter,4)"));
                            rpmvalue_ = double.Parse(GetPMVal($"round((entity('tool','{TpTlName}').cuttingspeed.finishing.general*1000)/(pi*entity('tool','{TpTlName}').diameter),1)"));
                        }
                        cutspeed_ = tapdepth * rpmvalue_;
                    }
                    catch (Exception)
                    {
                    }
                    Html_AddtXT += $@"<tr>
                            <td nowrap='nowrap'> <strong>{TpName_}</strong></td>
                            <td  nowrap='nowrap'>{TpOutWkName}</td>
                            <td class='wpl' nowrap='nowrap'>{TpMachineworkplane}</td>
                            <td nowrap='nowrap'>{TpTlName}-【{Tpnumber}】</td>
                            <td nowrap='nowrap'>{tlDim}</td>
                            <td nowrap='nowrap'>{tlRadius}</td><td nowrap='nowrap'>{tpthince}{tpaxisthinc}</td>
                            <td nowrap='nowrap'>{tloverhang}</td>
                            <td nowrap='nowrap'>{tlLength}</td>
                            <td nowrap='nowrap'>￠{tlholedim}-检查_{Tpchecked}</td>
                            <td nowrap='nowrap'>{tptotaltimeM}</td>
                            <td nowrap='nowrap'>Z{tpZmin}</td>
                            <td nowrap='nowrap'>{rpmvalue_}/{cutspeed_}</td>
                            <td nowrap='nowrap'>{tpstatgy}{theToolPhAxis}</td></tr>"
                            + "\r";
                }


                string theCutAllTimesstr = GetPMVal($"time_to_string({CutAllTime.ToString()},'m')");
                Htmltxt = Htmltxt.Replace("/*GetForthere*/", Html_AddtXT);
                string ProgramerName_ = MCS.ProjectName;
                if (ProgramerName_.Substring(ProgramerName_.LastIndexOf("_") + 1, ProgramerName_.Length - ProgramerName_.LastIndexOf("_") - 1).Trim().Length == 12)
                {
                    ProgramerName_ = ProgramerName_.Replace("_" + ProgramerName_.Substring(ProgramerName_.LastIndexOf("_") + 1, ProgramerName_.Length - ProgramerName_.LastIndexOf("_") - 1).Trim(), "");
                }
                Htmltxt = Htmltxt.Replace("{project}", ProgramerName_);
                Htmltxt = Htmltxt.Replace("{project.programmer}", GetPMVal("project.Programmer"));
                DirectoryInfo directoryInfo = new DirectoryInfo(MCS.ProjectPath);
                Htmltxt = Htmltxt.Replace("{project.date}", directoryInfo.CreationTime.ToString("yyyy/MM/dd_hh:mm:ss"));
                Htmltxt = Htmltxt.Replace("{PD_TotalTime}", theCutAllTimesstr);
                Htmltxt = Htmltxt.Replace("/*ThereisOptName*/", MCS.NcoutOptSelectOpt.Name);
                Htmltxt = Htmltxt.Replace("/*thereisXLEN*/", GetPMVal("round(entity('toolpath',TpName_).block.limits.xmax-entity('toolpath',TpName_).block.limits.xmin,1)"));
                Htmltxt = Htmltxt.Replace("/*thereisYLEN*/", GetPMVal("round(entity('toolpath',TpName_).block.limits.ymax-entity('toolpath',TpName_).block.limits.ymin,1)"));
                Htmltxt = Htmltxt.Replace("/*thereisZLEN*/", GetPMVal("round(entity('toolpath',TpName_).block.limits.zmax-entity('toolpath',TpName_).block.limits.zmin,1)"));
                Htmltxt = Htmltxt.Replace("/*thereisoutputtime*/", $"{dateTime.Year}年{dateTime.Month}月{dateTime.Day}日_{dateTime.Hour}时{dateTime.Minute}分{dateTime.Second}秒");
                Htmltxt = Htmltxt.Replace("{project.path}", MCS.ProjectPath);
                Htmltxt = Htmltxt.Replace("/*thereisModelpath*/", GetPMVal("folder('model')[0].path"));
                Htmltxt = Htmltxt.Replace("{ncprogram}", MCS.Ncout_ReadlyList[0].Name);
                if (MCS.NcoutOptSelectOpt.OptCanSelectWkOutput)
                {
                    Htmltxt = Htmltxt.Replace("/*OptChoDis*/", "");
                    Htmltxt = Htmltxt.Replace("/*thereisoutwkname*/", MCS.Ncout_Selected_OutputWorkplane.Name);
                }
                else
                {
                    Htmltxt = Htmltxt.Replace("/*OptChoDis*/", "NONE");
                    Htmltxt = Htmltxt.Replace("/*thereisoutwkname*/", "");
                }
                if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                {
                    Htmltxt = Htmltxt.Replace("/*thereisWPSHow*/", "");
                }
                else
                {
                    Htmltxt = Htmltxt.Replace("/*thereisWPSHow*/", "NONE");
                }

                PMCom("UNDRAW ALL");
                PMCom("DRAW BLOCK");
                if (GetPMVal("entity('toolpath',folder('ncprogram')[0].name).isturning()") == "0")
                {
                    PMCom("VIEW MODE MILLING");
                    PMCom("ROTATE TRANSFORM ISO1");
                }
                else
                {
                    PMCom("VIEW MODE TURNING");
                    PMCom("ROTATE TRANSFORM FRONT");
                }
                PMCom("DELETE SCALE");
                PMCom("UNDRAW BLOCK");
                PMCom("VIEW MODEL ; SHADE NORMAL");
                if (MCS.NcoutOptSelectOpt.OptCanSelectWkOutput)
                {
                    PMCom($"DRAW Workplane '{MCS.Ncout_Selected_OutputWorkplane.Name}'");
                }
                else
                {
                    PMCom($"DRAW Workplane '{GetPMVal($"entity('toolpath', '{MCS.Ncout_ReadlyList[0].Name}').workplane.name")}'");
                }

                PMCom($"KEEP BITMAP '{Imageph}'");
                Htmltxt = Htmltxt.Replace("/*thereisimage*/", Imageph);
                FileStream fs = new FileStream(sheetPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.WriteLine(Htmltxt);
                sw.Close();
                fs.Close();
                PMCom("VIEW BROWSER_WIDTH '800'");
                PMCom("BROWSER GO '" + sheetPath + "'");


            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_CreateSheet\r" + ex.ToString());
            }

        }
        #endregion

        #region 写出程序
        private void OutPutNc(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MCS.NcOpts.Contains(MCS.NcoutOptSelectOpt))
                {
                    MessageBox.Show("还没有选后处理");
                    return;
                }
                PMCom($@"EDIT NCPROGRAM ALL TAPEOPTIONS '{MCS.NcoutOptSelectOpt.FullPath}'");
                if (MCS.NcoutOptSelectOpt.OptCanSelectWkOutput)
                {
                    PMCom($"EDIT NCPROGRAM ALL WORKPLANE {MCS.NcoutOptSelectOpt.OptSelectMachineWkName}");
                }
                PMCom("KEEP NCPROGRAM ALL");
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_CreateSheet\r" + ex.ToString());
            }

        }

        #endregion

        #region 打开输出文件夹
        private void openNCFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", MCS.Ncout_OutputFolderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_CreateSheet\r" + ex.ToString());
            }

        }

        #endregion


    }
}
