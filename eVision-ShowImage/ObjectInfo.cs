using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Euresys.Open_eVision_22_08;
using System.Drawing;

namespace eVision_ShowImage
{
    [Serializable]
    class ObjectInfo
    {
        public int shapeNo = -1; //-1: unknown, 0: rectangle, 1: circle, 2: special
        //public EPoint center; //注意:原本使用這個物件，Serialize的時候會導致StackOverflow的例外
        public float centerX,centerY;
        public float stdWidth, stdHeight; //標準的寬高
        public float width, height; //量測尺寸的寬高
        public float stdDiameter; //標準直徑
        public float diameter; //量測到的直徑
        public int checkResult; //紀錄該物件是否被檢查過，以及結果 -1: 還沒有檢查 0:OK  1:NG
        public uint index; //紀錄codedImage中的索引
        public float widthError, heightError, diameterError;
        public ObjectInfo()
        {
            width = 0;
            height = 0;
            diameter = 0;
            checkResult = -1;//預設沒有比對過
            shapeNo = -1;
        }
        public String getShapeName()
        {
            switch (shapeNo)
            {
                case -1:
                    return "unknown";
                case 0:
                    return "rectangle";
                case 1:
                    return "square";
                case 2:
                    return "circle";
                case 3:
                    return "special";
            }
            return "";
        }
        
        //計算給予另一個物件的距離是否在範圍內
        public bool CheckDistance(ObjectInfo obj, float threshold)
        {
            if (Math.Sqrt(Math.Pow(centerX - obj.centerX, 2) + Math.Pow(centerY - obj.centerY, 2)) < threshold)
                return true;
            else
                return false;
        }
    }
}
