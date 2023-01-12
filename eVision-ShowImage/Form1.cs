//eVision-Show Image Sample 
//Function: 1. Load Image 
//          2. convert Bitmap to EImageC24
//          3. Show EImageC24 Image to picturebox
//Enviornment: VS 2012, eVision 2.2.2
//Caution: using eVision need to run [Visual Studio] or [compiled exe] as administrator, if not it will not able to get the grant from license dongle.
using Emgu.CV;
using Euresys.Open_eVision_22_08;
//using System.Text.Json;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

// Load Data <a href="https://www.flaticon.com/free-icons/folder" title="folder icons">Folder icons created by Creative Stall Premium - Flaticon</a>
// Camera <a href="https://www.flaticon.com/free-icons/cinema" title="cinema icons">Cinema icons created by Good Ware - Flaticon</a>
// Capture <a href="https://www.flaticon.com/free-icons/camera" title="camera icons">Camera icons created by Freepik - Flaticon</a>
// adjust <a href="https://www.flaticon.com/free-icons/maintenance" title="maintenance icons">Maintenance icons created by nawicon - Flaticon</a>
// measure <a href="https://www.flaticon.com/free-icons/ruler" title="ruler icons">Ruler icons created by Creaticca Creative Agency - Flaticon</a>
// measure <a href="https://www.flaticon.com/free-icons/ruler" title="ruler icons">Ruler icons created by Freepik - Flaticon</a>
// down <a href="https://www.flaticon.com/free-icons/down-arrow" title="down arrow icons">Down arrow icons created by Freepik - Flaticon</a>
// up <a href="https://www.flaticon.com/free-icons/up-arrow" title="up arrow icons">Up arrow icons created by Creatype - Flaticon</a>

//命名慣例
//Form 元件 小駝峰加底線，ex: btn_Camera
//variable 小駝峰，ex: someNumber
//method 大駝峰，ex: CalcSum
//套件 instance 前面要註明套件名稱 EC24Image1

//發現
//放大或縮小影像會需要多次矯正

namespace eVision_ShowImage
{
    public partial class Form1 : Form
    {
        // EImage
        EImageC24 EC24Image1 = new EImageC24(); //eVision的彩色圖像物件
        EImageBW8 EBW8Image1 = new EImageBW8(); //eVision的灰階圖像物件
        EImageBW8 EBW8Image2 = new EImageBW8();
        EImageBW8 EBW8ImageStd = new EImageBW8(); //for Efinder

        Point EBW8Image1Center;
        EImageBW8 mask = new EImageBW8(); //eVision的灰階圖像物件

        // Ratio
        float scalingRatio = 0; //Picturebox與原始影像大小的縮放比例
        float adjustRatio;

        // Detect
        EImageEncoder codedImage1Encoder = new EImageEncoder();// EImageEncoder instance
        ECodedImage2 codedImage1 = new ECodedImage2(); // ECodedImage2 instance
        EObjectSelection codedImage1ObjectSelection = new EObjectSelection(); // EObjectSelection instance


        // Measure
        EWorldShape EWorldShape1 = new EWorldShape(); // EWorldShape instance
        EFrameShape EFrameShape1 = new EFrameShape(); // EFrameShape instance
        EPointGauge EPointGauge1 = new EPointGauge(); // EPointGauge instance
        ELineGauge ELineGauge1 = new ELineGauge(); // ELineGauge instance
        //ELine measuredLine1 = null, measuredLine2 = null; // ELine instance
        ECircleGauge ECircleGauge1 = new ECircleGauge(); // ECircleGauge instance
        ECircle measuredCircle = null; // ECircle instance
        ERectangleGauge ERectangleGauge1 = new ERectangleGauge(); // ERectangleGauge instance
        ERectangle measuredRect = null;

        // ObjectSet
        ArrayList ObjectGSet = new ArrayList(0); //Golden Sample的Set
        ArrayList ObjectUSet = new ArrayList(0); //Unknown Sample的Set
        ArrayList ObjNGSet = new ArrayList(0); // NG Objects

        // Batch
        ObjectInfo clickedObj;
        ArrayList batchIndexes = new ArrayList();

        // Form
        Graphics g;

        // listbox
        bool listIsGolden = true; //目前listbox1顯示的是Golden或者待測物


        // Camera
        VideoCapture capture;
        bool isCapturing = false;
        BitmapData bmpData = null;
        Bitmap bmp = null;
        Mat m = new Mat();

        // EFind
        EPatternFinder EPatternFinder1; // EPatternFinder instance
        EFoundPattern[] EPatternFinder1FoundPatterns; // EFoundPattern instances
        EPatternFinder EPatternFinder2; // EPatternFinder instance
        EFoundPattern[] EPatternFinder2FoundPatterns; // EFoundPattern instances

        float finder1CenterX;
        float finder1CenterY;

        float finder2CenterY;

        // ERoi
        EROIBW8 EBW8Roi1 = new EROIBW8(); //EROIBW8 instance
        EROIBW8 EBW8Roi2 = new EROIBW8(); //EROIBW8 instance
        Point ERoi1Center = new Point(383, 679); //手動調整


        // save
        string savePath;


        // Form Dot Grid
        public float x = 5;
        public float y = 5;

        public Form1()
        {
            InitializeComponent();
        }

        // -----------------------Form-----------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Jpg|*.jpg|PNG|*.png|Json|*.json|All files|*.*";

            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.Filter = "Json|*.json|BMP|*.bmp|All files|*.*";

            g = pbImage.CreateGraphics();

            Learn(sender, e);
        }

        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            // 放掉攝影機
            if (capture != null)
            {
                capture.Dispose();
            }
        }

        // -----------------------PictureBox-----------------------
        private void pbImage_Down(object sender, MouseEventArgs e)
        {
            if (codedImage1ObjectSelection.ElementCount == 0)
            {
                Console.WriteLine("Detect first");
                return;
            }

            ECodedElement element;
            uint itemIndex;
            float
                center_X,
                center_Y,
                elementWidth,
                elementHeight;

            int realClickX = (int)((float)e.X / (float)scalingRatio);
            int realClickY = (int)((float)e.Y / (float)scalingRatio);
            Point click_Point = new Point(realClickX, realClickY);


            // 逐一檢查是否點再 codedImage1ObjectSelection 的 elements 裡面
            for (uint i = 0; i < codedImage1ObjectSelection.ElementCount; i++)
            {
                element = codedImage1ObjectSelection.GetElement(i);

                center_X = element.BoundingBoxCenterX;
                center_Y = element.BoundingBoxCenterY;
                Point center_Point = new Point((int)center_X, (int)center_Y);

                elementWidth = element.BoundingBoxWidth;
                elementHeight = element.BoundingBoxHeight;

                // 如果點在 selection object 裡
                if (IsClickInObject(center_Point, click_Point, elementWidth, elementHeight))
                {
                    // 如果在測量待測物
                    // 確認 clickedObj 是不是 NG
                    if (listIsGolden == false)
                    {
                        // 看 listBox2 的每個 items 的 index，有沒有等於 i
                        for (int j = 0; j < listBox2.Items.Count; j++)
                        {
                            itemIndex = uint.Parse(listBox2.Items[j].ToString().Substring(0, 3));

                            if (itemIndex == i)
                            {
                                clickedObj = (ObjectInfo)ObjectUSet[j];
                                listBox2.SelectedIndex = j;
                                listBox1.SelectedIndex = -1;
                                return;
                            }
                        }

                        clickedObj = (ObjectInfo)ObjectUSet[(int)i];

                        listBox1.SelectedIndex = (int)i;
                        listBox2.SelectedIndex = -1;
                    }
                    else
                    {
                        clickedObj = (ObjectInfo)ObjectGSet[(int)i];

                        listBox1.SelectedIndex = (int)i;
                        listBox2.SelectedIndex = -1;
                    }

                }

                element.Dispose();
            }
        }

        // -----------------------listBox-----------------------
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)// 如果 list item 被選取
            {
                pbImage.Refresh();
                EBW8Image1.Draw(g, scalingRatio); //再繪製上去

                // 被選取資料
                uint selectedIndex = uint.Parse(listBox1.SelectedItem.ToString().Substring(0, 3));//前三碼是物件編號
                ECodedElement element = codedImage1ObjectSelection.GetElement(selectedIndex);

                codedImage1.Draw(g, new ERGBColor(0, 0, 255), element, scalingRatio);

                ObjectInfo objInfo;
                if (listIsGolden) //如果目前listbox1顯示的是Golden的資料
                {
                    objInfo = (ObjectInfo)ObjectGSet[(int)listBox1.SelectedIndex];
                }
                else
                {
                    objInfo = (ObjectInfo)ObjectUSet[(int)listBox1.SelectedIndex];
                }

                ShowMeasureResult(objInfo);

                element.Dispose();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                pbImage.Refresh();
                EBW8Image1.Draw(g, scalingRatio); //再繪製上去

                uint idx = uint.Parse(listBox2.SelectedItem.ToString().Substring(0, 3));
                ECodedElement element = codedImage1ObjectSelection.GetElement(idx);//前三碼是物件編號

                codedImage1.Draw(g, element, scalingRatio);

                ObjectInfo objInfo = (ObjectInfo)ObjectUSet[(int)idx];

                ShowMeasureErrResult(objInfo);

                element.Dispose();
            }
        }

        // -----------------------Menu-----------------------
        private void MenuItemSaveSetting_Click(object sender, EventArgs e)
        {
            if (ObjectGSet.Count > 0) //防呆機制
            {
                try
                {
                    saveFileDialog1.Filter = "Json|*.json";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string jsonString = JsonConvert.SerializeObject(ObjectGSet);
                        StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                        sw.Write(jsonString);
                        sw.Close();
                        MessageBox.Show("設定檔寫入成功", "儲存設定檔", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "儲存設定檔", MessageBoxButtons.OK);
                }
            }
        }

        private void MenuItemOpenSetting_Click(object sender, EventArgs e)
        {
            bool isOverwrite = false;
            openFileDialog1.FilterIndex = 3; //Json
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (ObjectGSet.Count > 0) //檢查是否有設定檔了
                {
                    if (MessageBox.Show("確定要覆蓋目前的工件設定內容嗎?", "開啟設定檔", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        isOverwrite = true;
                    }
                }
                else
                {
                    isOverwrite = true; //沒有資料可以直接覆蓋
                }

                if (isOverwrite) //不管是空的或者可以複寫，都可以載入
                {
                    try
                    {
                        string jsonString = File.ReadAllText(openFileDialog1.FileName);
                        ArrayList tmpGSet = JsonConvert.DeserializeObject<ArrayList>(jsonString);
                        //反序列化進來之後，每個只是ArrayList中的JObject，是原本ObjectInfo的序列化JSON字串，所以需要逐一反序列化
                        ObjectGSet.Clear();
                        ObjectInfo tmpObj;
                        for (int i = 0; i < tmpGSet.Count; i++)
                        {
                            tmpObj = JsonConvert.DeserializeObject<ObjectInfo>(tmpGSet[i].ToString());
                            ObjectGSet.Add(tmpObj);
                        }
                        MessageBox.Show("設定檔載入成功", "載入設定檔", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "載入設定檔", MessageBoxButtons.OK);
                    }
                }

            }
        }

        private void Menu_Item_Open_Old_File_Click(object sender, EventArgs e)
        {
            // 開檔案
            //使用者選取Bitmap檔案
            openFileDialog1.FilterIndex = 2; //PNG
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName.Remove(openFileDialog1.FileName.Length - openFileDialog1.SafeFileName.Length);

                if (File.Exists(path + "ObjectGSet"))
                {
                    //載入圖檔
                    EBW8Image1.Load(openFileDialog1.FileName);

                    //計算Picturebox與顯示影像的比例，以便將影像縮放並且完整呈現到picturebox上。
                    scalingRatio = CalcRatioWithPictureBox(pbImage, EBW8Image1.Width, EBW8Image1.Height);
                    //顯示影像於Picturebox
                    pbImage.Refresh(); //先清除目前圖像
                    EBW8Image1.Draw(g, scalingRatio); //再繪製上去

                    listBox1.Items.Clear();
                    listBox2.Items.Clear();

                    // load ObjectGSet
                    ObjectGSet = ReadFromBinaryFile<ArrayList>(path + "ObjectGSet");

                    // savePath 防呆
                    string tempPath = savePath;
                    savePath = "";

                    // measureItem
                    btnMeasureItem_Click(sender, e);

                    savePath = tempPath;
                }
                else
                {
                    MessageBox.Show("資料夾內需要有 ObjectGSet");
                    return;
                }


            }
            else
            {
                MessageBox.Show("檔案開啟錯誤");
                return;
            }



            // openFileDialog1 清除

            // 載入 ObjectGSet

            // 載入 EBW8Image1

            // Measure and Inspect EBW8Image1

        }

        private void Menu_Dot_Grid_Setting_Click(object sender, EventArgs e)
        {
            // 開相機
            if (isCapturing == false)
            {
                btn_Camera_Click(sender, e);
            }

            // 設定 x, y
            FormDotGrid f2 = new FormDotGrid(x, y);
            f2.ShowDialog(this);

            btn_Camera_Click(sender, e);

            // Calibration
            //EBW8Image1.Load(Environment.CurrentDirectory + "\\Dot_Grid_2.tif");
            EWorldShape1.AutoCalibrateDotGrid(EBW8Image1, x, y, false);

            if (EWorldShape1.CalibrationSucceeded() == false)
            {
                MessageBox.Show("Calibration Failed");
            }
        }


        // -----------------------btn-----------------------
        private void btnGray_Click(object sender, EventArgs e)
        {
            try
            {
                //使用者選取Bitmap檔案
                openFileDialog1.FilterIndex = 2; //PNG
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //載入圖檔
                    EBW8Image1.Load(openFileDialog1.FileName);

                }
                else
                {
                    MessageBox.Show("檔案開啟錯誤");
                    return;
                }

                //計算Picturebox與顯示影像的比例，以便將影像縮放並且完整呈現到picturebox上。
                scalingRatio = CalcRatioWithPictureBox(pbImage, EBW8Image1.Width, EBW8Image1.Height);

                //顯示影像於Picturebox
                pbImage.Refresh(); //先清除目前圖像

                EBW8Image1.Draw(g, scalingRatio); //再繪製上去

                listBox1.Items.Clear();
                listBox2.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }


        private void btnRecord_Click(object sender, EventArgs e)
        {
            try
            {
                DetectObject(); //先偵測孔洞

                // listBox 一筆一筆添加資料
                ListBoxAddObj(listBox1, codedImage1ObjectSelection);

                //針對每個物件進行量測與結果紀錄
                BuildObjectSet(codedImage1ObjectSelection, ObjectGSet);
                // ObjectGSet 需要顯示正確
                ObjectInfo obj;

                for (int i = 0; i < ObjectGSet.Count; i++)
                {
                    obj = (ObjectInfo)ObjectGSet[i];
                    obj.checkResult = 0;
                }

                // 建立檔案夾，建立標準檔案，回傳儲存路徑
                BuildStdFile();

                listIsGolden = true; //為了確認listbox_indexchanged的時候要顯示誰的資料
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }


        private void btnMeasureItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (capture != null && isCapturing == true)
                {
                    btn_Camera_Click(sender, e);
                }

                DetectObject(); //先偵測孔洞



                // listBox 一筆一筆添加資料
                ListBoxAddObj(listBox1, codedImage1ObjectSelection);

                //針對每個物件進行量測與結果紀錄
                BuildObjectSet(codedImage1ObjectSelection, ObjectUSet);

                Inspect(sender, e);

                InspectShowAndSave();

                listIsGolden = false; //為了確認listbox_indexchanged的時候要顯示誰的資料
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        private void DetectObject()
        {
            // 如果 EBW8Image1
            if (EBW8Image1 == null || (EBW8Image1.Width == 0 && EBW8Image1.Height == 0))
            {
                return;
            }

            // codedImage1Encoder 設定
            //codedImage1Encoder.GrayscaleSingleThresholdSegmenter.BlackLayerEncoded = false; //為初始設定
            //codedImage1Encoder.GrayscaleSingleThresholdSegmenter.WhiteLayerEncoded = true; //為初始設定
            //codedImage1Encoder.GrayscaleSingleThresholdSegmenter.Mode = EGrayscaleSingleThreshold.MinResidue; //為初始設定

            // codedImage1 圖層
            codedImage1Encoder.Encode(EBW8Image1, codedImage1);

            CodedImage1ObjectSelection();

        }

        private void CodedImage1ObjectSelection()
        {
            codedImage1ObjectSelection.Clear();

            // codedImage1ObjectSelection 設定
            codedImage1ObjectSelection.FeretAngle = 0.00f;

            // codedImage1ObjectSelection 圖層
            codedImage1ObjectSelection.AddObjects(codedImage1);
            codedImage1ObjectSelection.AttachedImage = EBW8Image1;

            // don't care area 條件
            codedImage1ObjectSelection.RemoveUsingUnsignedIntegerFeature(EFeature.Area, 20, ESingleThresholdMode.Less);
            codedImage1ObjectSelection.RemoveUsingUnsignedIntegerFeature(EFeature.Area, 150000, ESingleThresholdMode.Greater);
        }

        private void BuildObjectSet(EObjectSelection EObjectSelection, ArrayList ObjectSet)
        {
            ObjectSet.Clear();

            uint length = EObjectSelection.ElementCount;
            ObjectInfo tmpObjInfo;
            ECodedElement element;

            //針對每個物件進行量測與結果紀錄
            for (uint i = 0; i < length; i++)
            {
                //listBox1.SelectedIndex = (int)i; //會觸發indexChanged，導致還沒有準備好ObjectGSet的例外
                //obj = codedImage1ObjectSelection.GetElement(uint.Parse(listBox1.SelectedItem.ToString().Substring(0, 3)));
                element = codedImage1ObjectSelection.GetElement(i);

                tmpObjInfo = new ObjectInfo();
                //Console.WriteLine("Mesure before");
                MeasureObj(element, tmpObjInfo); //針對每個物件進行量測
                //Console.WriteLine("Mesure after");

                if (tmpObjInfo.shapeNo == -1)
                {
                    MessageBox.Show(i + " 偵測到錯誤圖形。");
                }

                //Console.WriteLine("Show Mesure before");
                ShowMeasureResult(tmpObjInfo);
                //Console.WriteLine("Show Mesure after");

                tmpObjInfo.index = i; //紀錄CodedImage中的索引，屆時方便繪製回去

                //Console.WriteLine("add obj before");
                ObjectSet.Add(tmpObjInfo); //把資料放進陣列
                //Console.WriteLine("add obj after");

                element.Dispose();
            }
        }

        private void BuildStdFile()
        {
            // Result Folder
            string path = Environment.CurrentDirectory + "\\result";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            // Date Folder
            path += "\\" + DateTime.Now.ToString("yyyyMMdd");
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            // Component Name Folder
            path += "\\" + "1U2N2G-B550_2T-ChASSIS";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            // Time Folder
            path += "\\" + "1U2N2G-B550_2T-ChASSIS_" + DateTime.Now.ToString("HHmmss");
            Directory.CreateDirectory(path);


            // 存圖檔
            EBW8Image1.SavePng(path + "\\Standard.png");

            // 存ObjectGSet
            WriteToBinaryFile<ArrayList>(path + "\\ObjectGSet", ObjectGSet);

            savePath = path;
        }

        private void Inspect(object sender, EventArgs e)
        {
            if (ObjectGSet.Count == 0)
            {
                MessageBox.Show("請測量標準樣本，才可以做比對。");
                return;
            }

            listBox2.Items.Clear();

            EBW8Image1.Draw(g, scalingRatio);

            //先逐一檢查待測物上的孔洞是否與OK樣本的標準值一致
            ObjectInfo
                tmpObjU, // Unknown
                tmpObj;


            for (int i = 0; i < ObjectUSet.Count; i++)
            {
                tmpObjU = (ObjectInfo)ObjectUSet[i];
                int j = 0;

                do
                {
                    tmpObj = (ObjectInfo)ObjectGSet[j];

                    if (tmpObjU.CheckDistance(tmpObj, 10) == true) //如果在10個pixels內，算是同一個物件
                    {
                        int widthTest;
                        int heightTest;
                        int diameterTest;

                        switch (tmpObjU.shapeNo)
                        {
                            case 0:
                                //不管有沒有超過臨界值，都記錄差值，並記錄對應的標準值
                                tmpObjU.stdWidth = tmpObj.stdWidth;
                                tmpObjU.stdHeight = tmpObj.stdHeight;
                                tmpObjU.widthError = tmpObjU.width - tmpObjU.stdWidth; //有正負號，少或者多，使用量測的數值減去設定的標準值
                                tmpObjU.heightError = tmpObjU.height - tmpObjU.stdHeight;


                                // 如果寬高超過 thresshold 回傳 1(NG) or 0(OK)
                                widthTest = IsBigger((float)numThreshold.Value, tmpObjU.widthError);
                                heightTest = IsBigger((float)numThreshold.Value, tmpObjU.heightError);

                                if ((widthTest + heightTest) > 0) //如果至少有一個 NG 回傳 NG
                                {
                                    tmpObjU.checkResult = 1; //NG
                                    ListBoxAddObj(listBox2, tmpObjU);
                                }
                                else
                                {
                                    tmpObjU.checkResult = 0; //OK
                                }
                                break;
                            case 1:
                                //不管有沒有超過臨界值，都記錄差值，並記錄對應的標準值
                                tmpObjU.stdWidth = tmpObj.stdWidth;
                                tmpObjU.stdHeight = tmpObj.stdHeight;
                                tmpObjU.widthError = tmpObjU.width - tmpObjU.stdWidth; //有正負號，少或者多，使用量測的數值減去設定的標準值
                                tmpObjU.heightError = tmpObjU.height - tmpObjU.stdHeight;


                                // 如果寬高超過 thresshold 回傳 1(NG) or 0(OK)
                                widthTest = IsBigger((float)numThreshold.Value, tmpObjU.widthError);
                                heightTest = IsBigger((float)numThreshold.Value, tmpObjU.heightError);

                                if ((widthTest + heightTest) > 0) //如果至少有一個 NG 回傳 NG
                                {
                                    tmpObjU.checkResult = 1; //NG
                                    ListBoxAddObj(listBox2, tmpObjU);
                                }
                                else
                                {
                                    tmpObjU.checkResult = 0; //OK
                                }
                                break;
                            case 2:
                                //不管有沒有超過臨界值，都記錄差值
                                tmpObjU.diameterError = tmpObjU.diameter - tmpObj.stdDiameter; //有正負號，少或者多

                                diameterTest = IsBigger((float)numThreshold.Value, tmpObjU.diameterError);

                                if (diameterTest > 0)
                                {
                                    tmpObjU.checkResult = 1; //NG
                                    ListBoxAddObj(listBox2, tmpObjU);
                                }
                                else
                                {
                                    tmpObjU.checkResult = 0; //OK
                                }
                                break;
                            case 3:
                                //不管有沒有超過臨界值，都記錄差值
                                tmpObjU.widthError = tmpObjU.width - tmpObj.stdWidth; //有正負號，少或者多
                                tmpObjU.heightError = tmpObjU.height - tmpObj.stdHeight;

                                widthTest = IsBigger((float)numThreshold.Value, tmpObjU.widthError);
                                heightTest = IsBigger((float)numThreshold.Value, tmpObjU.heightError);

                                if ((widthTest + heightTest) > 0) //如果至少有一個 NG 回傳 NG
                                {
                                    tmpObjU.checkResult = 1; //NG
                                    ListBoxAddObj(listBox2, tmpObjU);
                                }
                                else
                                {
                                    tmpObjU.checkResult = 0; //OK
                                }
                                break;
                        }
                        break;
                    }

                    // 防呆，如果有個 Golden 沒有找到對應的孔洞？

                    j++;

                    if (j >= ObjectGSet.Count)//沒有找到，就是該obj在待測物中是多出來的，標為NG
                    {
                        tmpObjU.checkResult = 1; //NG
                        ListBoxAddObj(listBox2, tmpObjU);
                        break;
                    }
                } while (true);


            }

        }

        private void InspectShowAndSave()
        {
            if (listBox2.Items.Count == 0 && ObjectGSet.Count == ObjectUSet.Count)
            {
                ControlPaint.DrawBorder(g, pbImage.ClientRectangle,
                    Color.GreenYellow, 5, ButtonBorderStyle.Solid,
                    Color.GreenYellow, 5, ButtonBorderStyle.Solid,
                    Color.GreenYellow, 5, ButtonBorderStyle.Solid,
                    Color.GreenYellow, 5, ButtonBorderStyle.Solid);

                EBW8Image1.SavePng(savePath + "\\OK_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");
            }
            else
            {
                ECodedElement element;

                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    uint selectedIndex = uint.Parse(listBox2.Items[i].ToString().Substring(0, 3));//前三碼是物件編號
                    element = codedImage1ObjectSelection.GetElement(selectedIndex);
                    codedImage1.Draw(g, element, scalingRatio);

                    element.Dispose();
                }

                EBW8Image1.SavePng(savePath + "\\NG_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");
            }

        }

        // -----------------------Measure-----------------------
        private void MeasureObj(ECodedElement obj, ObjectInfo objInfo)
        {
            objInfo.centerX = obj.BoundingBoxCenterX;
            objInfo.centerY = obj.BoundingBoxCenterY;
            //這裡需要先使用Raio判斷長方形或圓形(正方形)，亦或者分別量測，有結果的才算是有那個形狀
            if (obj.BoundingBoxWidth / obj.BoundingBoxHeight >= 0.95 && obj.BoundingBoxWidth / obj.BoundingBoxHeight <= 1.05) //正方形或圓形
            {
                //必須先量測正方形，再來量測圓形，不然會誤判
                //嘗試看看是否為正方形
                measuredRect = MeasureRect(EBW8Image1, obj);

                if (measuredRect is object) //防呆機制，如果有找到正方形
                {
                    objInfo.shapeNo = 1;
                    objInfo.width = measuredRect.SizeX;
                    objInfo.height = measuredRect.SizeY;
                    objInfo.stdWidth = measuredRect.SizeX;
                    objInfo.stdHeight = measuredRect.SizeY;


                    var names = new List<string>();
                    foreach (var name in names)
                    {

                    }
                }
                else
                {
                    measuredCircle = MeasureCircle(EBW8Image1, obj);
                    if (measuredCircle is object) //如果回傳不是null，表示有圓形
                    {
                        objInfo.shapeNo = 2;
                        objInfo.stdDiameter = measuredCircle.Diameter;
                        objInfo.diameter = measuredCircle.Diameter;
                    }
                    else
                    {
                        objInfo.shapeNo = -1;
                    }
                }
            }
            else
            {
                //進行長方形量測
                measuredRect = MeasureRect(EBW8Image1, obj);
                if (measuredRect is object) //防呆機制
                {
                    objInfo.shapeNo = 0;
                    objInfo.stdWidth = measuredRect.SizeX;
                    objInfo.stdHeight = measuredRect.SizeY;
                    objInfo.width = measuredRect.SizeX;
                    objInfo.height = measuredRect.SizeY;
                }
                else
                {
                    EPoint p = MeasureSpecial(EBW8Image1, obj); //進行特殊長方形量測
                    if (p is object)
                    {
                        objInfo.shapeNo = 3;
                        objInfo.stdWidth = p.X;
                        objInfo.stdHeight = p.Y;
                        objInfo.width = p.X;
                        objInfo.height = p.Y;
                    }
                    else
                    {
                        objInfo.shapeNo = -1;
                    }
                }

            }
        }

        private ERectangle MeasureRect(EImageBW8 img, ECodedElement element)
        {
            EWorldShape1.SetSensorSize(EBW8Image1.Width, EBW8Image1.Height);
            ERectangleGauge1.Attach(EWorldShape1);
            ERectangleGauge1.Center = element.BoundingBoxCenter;
            if (element.BoundingBoxWidth > element.BoundingBoxHeight) //容忍值必須以寬高數值小的為基準，不然涵蓋兩個邊
            {
                ERectangleGauge1.Tolerance = element.BoundingBoxHeight / 4;
            }
            else
            {
                ERectangleGauge1.Tolerance = element.BoundingBoxWidth / 4;
            }
            ERectangleGauge1.TransitionType = ETransitionType.Bw; //由外往裡面看
            ERectangleGauge1.SamplingStep = 1;
            ERectangleGauge1.SetSize(element.BoundingBoxWidth, element.BoundingBoxHeight);
            ERectangleGauge1.Measure(img);

            if (ERectangleGauge1.NumValidSamples < ERectangleGauge1.NumSamples * 0.9) //防呆機制，萬一給的不是長方形
            {
                return null;
            }
            else
            {
                return ERectangleGauge1.MeasuredRectangle;
            }
        }

        private ECircle MeasureCircle(EImageBW8 img, ECodedElement element)
        {
            //先用圓形量測
            EWorldShape1.SetSensorSize(EBW8Image1.Width, EBW8Image1.Height);
            ECircleGauge1.Attach(EWorldShape1);
            ECircleGauge1.Center = element.BoundingBoxCenter;
            ECircleGauge1.Diameter = element.BoundingBoxWidth;
            ECircleGauge1.TransitionType = ETransitionType.Wb; //是由圓心往外看，跟Rectangle不同
            ECircleGauge1.Tolerance = element.BoundingBoxWidth / 5;
            ECircleGauge1.SamplingStep = 1; //每個點都要檢查
            ECircleGauge1.Measure(img);
            if (ECircleGauge1.NumValidSamples < ECircleGauge1.NumSamples * 0.9) //防呆機制，萬一給的不是長方形
            {
                return null;
            }
            else
            {
                return ECircleGauge1.MeasuredCircle;
            }
        }
        //回傳量測的寬與高
        public EPoint MeasureSpecial(EImageBW8 img, ECodedElement element)
        {
            //特殊形狀，只有量測精準寬與高
            //假設條件，圖案必須是上下左右對稱，有角度偏差會進行修正
            //量測方式: 以BoundingCenter為中心，修正角度後進行十字線，兩條PointGauge進行量測
            double tmpW = 0, tmpH = 0;
            EWorldShape1.SetSensorSize(img.Width, img.Height);
            EPointGauge1.Attach(EWorldShape1); //將LineGauge繫結到世界座標系統
            EPointGauge1.TransitionType = ETransitionType.BwOrWb; //設定邊緣轉換類型，兩端剛好有Bw與Wb
            EPointGauge1.SetCenterXY(element.BoundingBoxCenterX, element.BoundingBoxCenterY);
            EPointGauge1.Tolerance = element.BoundingBoxWidth / 2 + 10;
            EPointGauge1.ToleranceAngle = element.MinimumEnclosingRectangleAngle;
            EPointGauge1.Angle = element.MinimumEnclosingRectangleAngle;//要看那一個角度量出來比較準
            EPointGauge1.Thickness = 3; //增加厚度，避免小雜訊
                                        //EPointGauge1.Angle = element.EllipseAngle;
            EPointGauge1.Measure(img);
            EPointGauge1.SetZoom(scalingRatio);
            EPointGauge1.Draw(g, EDrawingMode.Actual, true);
            //檢查有沒有取到兩個點
            if (EPointGauge1.NumMeasuredPoints != 2) //如果量測到的point不到兩個，表示沒有量測到邊緣
            {
                return null;
            }
            else
            {
                EPoint tmpP1, tmpP2;
                tmpP1 = EPointGauge1.GetMeasuredPoint(0);
                tmpP2 = EPointGauge1.GetMeasuredPoint(1);
                tmpW = Math.Sqrt(Math.Pow(tmpP1.X - tmpP2.X, 2) + Math.Pow(tmpP1.Y - tmpP2.Y, 2));
                //量測另外一個垂直方向
                //EWorldShape1.SetSensorSize(EBW8Image1.Width, EBW8Image1.Height);
                //EPointGauge1.Attach(EWorldShape1); //將LineGauge繫結到世界座標系統
                //EPointGauge1.TransitionType = ETransitionType.BwOrWb; //設定邊緣轉換類型，兩端剛好有Bw與Wb
                //EPointGauge1.SetCenterXY(element.BoundingBoxCenterX, element.BoundingBoxCenterY);
                EPointGauge1.Tolerance = element.BoundingBoxHeight / 2 + 10;
                EPointGauge1.ToleranceAngle = element.MinimumEnclosingRectangleAngle + 270;
                EPointGauge1.Angle = element.MinimumEnclosingRectangleAngle + 270;//要看那一個角度量出來比較準，PS: 加90度居然無法量測，好怪
                                                                                  //EPointGauge1.Angle = element.EllipseAngle;
                EPointGauge1.Measure(img);

                EPointGauge1.SetZoom(scalingRatio);
                EPointGauge1.Draw(g, EDrawingMode.Actual, true);
                //檢查有沒有取到兩個點
                if (EPointGauge1.NumMeasuredPoints != 2) //如果量測到的point不到兩個，表示沒有量測到邊緣
                {
                    return null;
                }
                else
                {
                    tmpP1 = EPointGauge1.GetMeasuredPoint(0);
                    tmpP2 = EPointGauge1.GetMeasuredPoint(1);
                    tmpH = Math.Sqrt(Math.Pow(tmpP1.X - tmpP2.X, 2) + Math.Pow(tmpP1.Y - tmpP2.Y, 2));
                    return new EPoint((float)tmpW, (float)tmpH);
                }
            }
        }


        private void ShowMeasureResult(ObjectInfo objInfo)
        {
            lblWidth.Text = "";
            lblHeight.Text = "";
            lblDiameter.Text = "";
            switch (objInfo.shapeNo)
            {
                case 0:// rectangle
                    lblWidth.Text = objInfo.width.ToString("#.##");
                    lblHeight.Text = objInfo.height.ToString("#.##");
                    numWidth.Value = (Decimal)objInfo.stdWidth;
                    numHeight.Value = (Decimal)objInfo.stdHeight;
                    break;
                case 1:// square
                    lblWidth.Text = objInfo.width.ToString("#.##");
                    lblHeight.Text = objInfo.height.ToString("#.##");
                    numWidth.Value = (Decimal)objInfo.stdWidth;
                    numHeight.Value = (Decimal)objInfo.stdHeight;
                    break;
                case 2:// circle
                    lblDiameter.Text = objInfo.diameter.ToString("#.##");

                    break;
                case 3:// special
                    lblWidth.Text = objInfo.width.ToString("#.##");
                    lblHeight.Text = objInfo.height.ToString("#.##");
                    numWidth.Value = (Decimal)objInfo.stdWidth;
                    numHeight.Value = (Decimal)objInfo.stdHeight;
                    break;
            }
        }
        private void ShowMeasureErrResult(ObjectInfo objInfo) //顯示量測的誤差值
        {
            lblWidthErr.Text = "";
            lblHeightErr.Text = "";
            lblDiameterErr.Text = "";
            switch (objInfo.shapeNo)
            {
                case 0:
                    lblWidthErr.Text = objInfo.widthError.ToString("0.##");
                    lblHeightErr.Text = objInfo.heightError.ToString("0.##");
                    numWidth.Value = (Decimal)objInfo.stdWidth;
                    numHeight.Value = (Decimal)objInfo.stdHeight;
                    break;
                case 1:
                    lblWidthErr.Text = objInfo.widthError.ToString("0.##");
                    lblHeightErr.Text = objInfo.heightError.ToString("0.##");
                    numWidth.Value = (Decimal)objInfo.stdWidth;
                    numHeight.Value = (Decimal)objInfo.stdHeight;
                    break;
                case 2:
                    lblDiameterErr.Text = objInfo.diameterError.ToString("0.##");
                    break;
                case 3:
                    lblWidthErr.Text = objInfo.widthError.ToString("0.##");
                    lblHeightErr.Text = objInfo.heightError.ToString("0.##");
                    numWidth.Value = (Decimal)objInfo.stdWidth;
                    numHeight.Value = (Decimal)objInfo.stdHeight;
                    break;
            }
        }

        // -----------------------Camera-----------------------
        private void btn_Camera_Click(object sender, EventArgs e)
        {
            // 判斷停止還是開始
            if (isCapturing == false)
            {
                if (capture == null)
                {
                    capture = new VideoCapture(1);
                }

                capture.ImageGrabbed += Capture_ImageGrabbed;

                capture.Start(); //開始攝影

                btn_Camera.Text = "影像截圖";

                btn_Camera.Image = Properties.Resources.camera;
            }
            else
            {
                capture.Pause();

                btn_Camera.Image = Properties.Resources.video_camera;
                btn_Camera.Text = "開始攝影";

                if (bmp == null)
                {
                    MessageBox.Show("沒取到影像，請重新攝影。");
                }
                else
                {
                    //Bitmap 透過 EC24Image 轉成 EBW8Image
                    EC24Image1 = BitmapToEImageC24(ref bmp);
                    EBW8Image1.SetSize(EC24Image1);
                    EasyImage.Convert(EC24Image1, EBW8Image1); //open eVision 22.08 才能 work，舊版本的 paramaters 好像不太一樣

                    //計算Picturebox與顯示影像的比例，以便將影像縮放並且完整呈現到picturebox上。
                    scalingRatio = CalcRatioWithPictureBox(pbImage, EBW8Image1.Width, EBW8Image1.Height);
                }
            }

            isCapturing = !isCapturing;
        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                //取得影像
                //轉成 Bitmap
                capture.Retrieve(m);
                bmp = m.ToBitmap(); //不能使用 new Bitmap(m.Bitmap)

                if (bmp == null)
                {
                    return;
                }

                pbImage.Image = bmp;

                //btnDetectObject_Click(sender, e); //跨執行緒作業無效: 存取控制項 'listBox1' 時所使用的執行緒與建立控制項的執行緒不同。
                //btnRecord_Click(sender, e); //跨執行緒作業無效: 存取控制項 'listBox1' 時所使用的執行緒與建立控制項的執行緒不同。
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }



        // -----------------------Adjust-----------------------

        private void btn_Adjust_Click(object sender, EventArgs e)
        {
            // 如果沒有 EBWIImage1
            if (EBW8Image1 == null || (EBW8Image1.Width == 0 && EBW8Image1.Height == 0))
            {
                MessageBox.Show("請先載入圖片或相機截圖");
                return;
            }

            Adjust_Horizontal(sender, e); //鏡像

            Adjust_Vertical(sender, e); //垂直翻轉

            Adjust_Fixed(sender, e); //放大 或 縮小會需要多次矯正

            EBW8Image2.CopyTo(EBW8Image1); //讓 EBW8IImage1 為正確的圖像

            // 畫出 EBW8Image1
            scalingRatio = CalcRatioWithPictureBox(pbImage, EBW8Image2.Width, EBW8Image2.Height);

            pbImage.Image = null;
            pbImage.Refresh();
            EBW8Image1.Draw(g, scalingRatio);

            // 畫出 finder
            EPatternFinder1FoundPatterns = EPatternFinder1.Find(EBW8Image1); //找 ERoi1 的位置
            EPatternFinder2FoundPatterns = EPatternFinder2.Find(EBW8Image1); //找 ERoi2 的位置
            EPatternFinder1FoundPatterns[0].Draw(g, scalingRatio);
            EPatternFinder2FoundPatterns[0].Draw(g, scalingRatio);
        }


        private void Adjust_Fixed(object sender, EventArgs e)
        {
            // 位置校正 & 水平校正
            EPatternFinder1FoundPatterns = EPatternFinder1.Find(EBW8Image1); //找 ERoi1 的位置

            // 如果沒找到
            if (EPatternFinder1FoundPatterns[0].Score < 0.8)
            {
                MessageBox.Show("找不到類似圖形，請確認圖像是否正確。");
                return;
            }

            finder1CenterX = EPatternFinder1FoundPatterns[0].Center.X;
            finder1CenterY = EPatternFinder1FoundPatterns[0].Center.Y;

            EBW8Image2 = new EImageBW8();
            EBW8Image2.SetSize(EBW8ImageStd);

            adjustRatio = 1 / EPatternFinder1FoundPatterns[0].Scale;

            // ERoi1Center 需要手動調整
            EasyImage.ScaleRotate(EBW8Image1, finder1CenterX, finder1CenterY, ERoi1Center.X, ERoi1Center.Y, adjustRatio, adjustRatio, EPatternFinder1FoundPatterns[0].Angle, EBW8Image2, 0);
        }

        private void Adjust_Horizontal(object sender, EventArgs e)
        {
            // 水平校正後
            Adjust_Fixed(sender, e); //水平旋轉 和 大小縮放
            // 在 EBW8Image2 找 EROI2
            EPatternFinder2FoundPatterns = EPatternFinder2.Find(EBW8Image2);

            // 如果 finder2 分數太低
            // 鏡像旋轉
            if (EPatternFinder2FoundPatterns[0].Score < 0.8)
            {
                EBW8Image2 = new EImageBW8();
                EBW8Image2.SetSize(EBW8Image1);

                EBW8Image1Center = new Point(EBW8Image1.Width / 2, EBW8Image1.Height / 2);

                EasyImage.ScaleRotate(EBW8Image1, EBW8Image1Center.X, EBW8Image1Center.Y, EBW8Image1Center.X, EBW8Image1Center.Y, -1.00f, 1.00f, 0, EBW8Image2, 0);

                EBW8Image2.CopyTo(EBW8Image1);
            }
            else
            {
                //Console.WriteLine("Don't need horizontal.");
            }
        }

        private void Adjust_Vertical(object sender, EventArgs e)
        {
            // 水平校正後
            Adjust_Fixed(sender, e); //水平旋轉 和 大小縮放
            // 在 EBW8Image2 找 EROI2
            EPatternFinder1FoundPatterns = EPatternFinder1.Find(EBW8Image2); //找 ERoi1 的位置
            EPatternFinder2FoundPatterns = EPatternFinder2.Find(EBW8Image2); //找 ERoi2 的位置

            // 如果 finder1Y > finder2Y 旋轉
            finder1CenterX = EPatternFinder1FoundPatterns[0].Center.Y;
            finder2CenterY = EPatternFinder2FoundPatterns[0].Center.Y;

            if (finder1CenterX > finder2CenterY)
            {
                EBW8Image2 = new EImageBW8();
                EBW8Image2.SetSize(EBW8Image1);

                EBW8Image1Center = new Point(EBW8Image1.Width / 2, EBW8Image1.Height / 2);

                EasyImage.ScaleRotate(EBW8Image1, EBW8Image1Center.X, EBW8Image1Center.Y, EBW8Image1Center.X, EBW8Image1Center.Y, 1.00f, -1.00f, 0, EBW8Image2, 0);

                EBW8Image2.CopyTo(EBW8Image1);
            }
            else
            {
                //Console.WriteLine("Don't need vertical.");
            }
        }

        private void Learn(object sender, EventArgs e)
        {
            EBW8ImageStd.Load(Environment.CurrentDirectory + "\\PressItem.png");

            EPatternFinder1 = new EPatternFinder();
            // 先學習不規則圖形
            // 可用於校正水平位置
            // Attach the roi to its parent
            EBW8Roi1.Attach(EBW8ImageStd);
            EBW8Roi1.SetPlacement(67, 558, 632, 242);
            EPatternFinder1.Learn(EBW8Roi1);

            EPatternFinder1.AngleTolerance = 50.00f;
            EPatternFinder1.ScaleTolerance = 0.60f;

            EPatternFinder2 = new EPatternFinder();
            // 第二個標準點
            // 用於 鏡像 和 垂直翻轉
            EBW8Roi2.Attach(EBW8ImageStd);
            EBW8Roi2.SetPlacement(714, 753, 594, 78);
            EPatternFinder2.Learn(EBW8Roi2);

            // 由於前面都會校正一次，所以容忍值不用太高，速度會更快
            EPatternFinder2.AngleTolerance = 10.00f;
            EPatternFinder2.ScaleTolerance = 0.10f;
        }

        // -----------------------Batch-----------------------
        private void btn_Batch_Search_Click(object sender, EventArgs e)
        {
            batchIndexes.Clear();

            if (ObjectGSet.Count == 0) //防呆
            {
                MessageBox.Show("請先量測標準樣本。");
                return;
            }

            if (clickedObj == null)
            {
                MessageBox.Show("請先點選圖案。");
                return;
            }

            ObjectInfo tempObj;

            for (uint i = 0; i < ObjectGSet.Count; i++)
            {
                tempObj = (ObjectInfo)ObjectGSet[(int)i];
                if (tempObj.shapeNo == clickedObj.shapeNo)
                {
                    batchIndexes.Add(i);
                }
                else
                {
                    continue;
                }
            }


            pbImage.Refresh(); //先清除目前圖像
            EBW8Image1.Draw(g, scalingRatio); //繪製底圖

            ECodedElement element;

            foreach (uint i in batchIndexes)
            {
                element = codedImage1ObjectSelection.GetElement(i);
                codedImage1.Draw(g, new ERGBColor(0, 0, 255), element, scalingRatio); //再繪製 HighLight
                element.Dispose();
            }
        }

        private void btnBatchSetting_Click(object sender, EventArgs e)
        {
            if (batchIndexes.Count == 0)
            {
                MessageBox.Show("請先搜尋相同尺寸。");
                return;
            }

            pbImage.Refresh(); //先清除目前圖像
            EBW8Image1.Draw(g, scalingRatio); //繪製底圖

            ObjectInfo obj;
            ECodedElement element;

            foreach (uint i in batchIndexes)
            {
                obj = (ObjectInfo)ObjectGSet[(int)i];
                element = codedImage1ObjectSelection.GetElement(i);

                // 把每一個 obj 的標準尺寸，設成手動調整的數字
                obj.stdWidth = (float)numWidth.Value;
                obj.stdHeight = (float)numHeight.Value;
                codedImage1.Draw(g, new ERGBColor(0, 0, 255), element, scalingRatio);

                element.Dispose();
            }

            batchIndexes.Clear();
        }

        // -----------------------Method-----------------------
        private int IsBigger(float referance, float testNum)
        {
            if (Math.Abs(testNum) > referance)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private void ListBoxAddObj(ListBox listBox, ObjectInfo obj)
        {
            string objIndex = obj.index.ToString("000");
            string objCenterX = obj.centerX.ToString("#.#");
            string objCenterY = obj.centerY.ToString("#.#");

            listBox.Items.Add(objIndex + "(" + objCenterX + "," + objCenterY + ")");
        }

        private void ListBoxAddObj(ListBox listBox, ECodedElement obj, uint i)
        {
            string objIndex = i.ToString("000");
            string objCenterX = obj.BoundingBoxCenterX.ToString("#.#");
            string objCenterY = obj.BoundingBoxCenterY.ToString("#.#");

            listBox.Items.Add(objIndex + "(" + objCenterX + "," + objCenterY + ")");
        }

        private void ListBoxAddObj(ListBox listBox, EObjectSelection EObjectSelection)
        {
            listBox.Items.Clear();

            uint length = EObjectSelection.ElementCount;
            ECodedElement element;

            for (uint i = 0; i < length; i++)
            {
                element = EObjectSelection.GetElement(i);

                string objIndex = i.ToString("000");
                string objCenterX = element.BoundingBoxCenterX.ToString("#.#");
                string objCenterY = element.BoundingBoxCenterY.ToString("#.#");

                listBox.Items.Add(objIndex + "(" + objCenterX + "," + objCenterY + ")");

                element.Dispose();
            }

            listBox.Refresh();
        }

        public float CalcRatioWithPictureBox(PictureBox pb, float imageWidth, float imageHeight)
        {
            if (pb == null)
            {
                MessageBox.Show("No pictureBox.");
                return 0;
            }

            if (imageWidth == 0 || imageHeight == 0)
            {
                MessageBox.Show("長、寬不能為 0。");
                return 0;
            }


            float pbWidth = pb.Width;
            float pbHeight = pb.Height;
            float pbRatio = pbWidth / pbHeight;

            float imageRatio = imageWidth / imageHeight;

            float ratioBetween;

            if (imageRatio > pbRatio)
                ratioBetween = pbWidth / imageWidth;
            else
                ratioBetween = pbHeight / imageHeight;

            return ratioBetween;
        }

        private Boolean IsClickInObject(Point center_Point, Point click_Point, float objWidth, float objHeight)
        {
            float center_X = center_Point.X;
            float center_Y = center_Point.Y;

            float click_X = click_Point.X;
            float click_Y = click_Point.Y;

            float x_distance = Math.Abs(center_X - click_X);
            float y_distance = Math.Abs(center_Y - click_Y);

            if (x_distance < (objWidth / 2) && y_distance < (objHeight / 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private EImageC24 BitmapToEImageC24(ref Bitmap bitmap)
        {
            EImageC24 EC24ImageTemp = new EImageC24();

            try
            {
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

                bmpData =
                    bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    bitmap.PixelFormat); // 如果 bitmap.PixelFormat == Format8bppIndexed，會呈現出三個黑白影像

                EC24ImageTemp.SetImagePtr(bitmap.Width, bitmap.Height, bmpData.Scan0);

                bitmap.UnlockBits(bmpData);

            }
            catch (EException e)//EException為eVision的例外處理
            {
                Console.WriteLine(e.ToString());
            }

            return EC24ImageTemp;
        }

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the binary file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the binary file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the binary file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

    }
}
